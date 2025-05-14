using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CellarioRDWLogListener
{
    public partial class FormGUI : Form
    {
        private FileSystemWatcher logFileWatcher;
        private FileSystemWatcher newFileDetector;
        private System.Threading.Timer logCheckTimer;
        private bool isRunning = false;
        private string logFilePath;
        private string resultsPathParent;
        private string barcode;
        private string orderID;

        private DateTime lastUpdateTime = DateTime.MinValue;
        private readonly TimeSpan debounceDelay = TimeSpan.FromSeconds(1);

        public FormGUI()
        {
            InitializeComponent();
            Console.SetOut(new ConsoleRedirector(textConsole, this));
        }

        private void btnBrowseLogFile(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Log files (*.log)|*.log|Log files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtLogFile.Text = dialog.FileName;
            }
        }

        private void btnBrowseResultsFolder(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtResultsFolder.Text = dialog.SelectedPath;
            }
        }

        private void btnStartListening(object sender, EventArgs e)
        {
            logFilePath = txtLogFile.Text;
            resultsPathParent = txtResultsFolder.Text;

            if (!File.Exists(logFilePath) || !Directory.Exists(resultsPathParent))
            {
                MessageBox.Show("Please provide valid paths.");
                return;
            }

            DisplayOnConsole($"Now listening: {logFilePath}");
            isRunning = true;

            btnStart.Enabled = false;
            btnStart.BackColor = Color.Gray;
            btnStop.Enabled = true;
            btnStop.BackColor = Color.Red;

            lblStatus.ForeColor = Color.White;
            lblStatus.BackColor = Color.Green;
            lblStatus.Text = "Running";

            logCheckTimer = new System.Threading.Timer(
                callback: _ => CheckForReadOperation(),
                state: null,
                dueTime: TimeSpan.Zero,
                period: TimeSpan.FromSeconds(2));
        }

        private void btnStopListening(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                return;
            }

            isRunning = false;

            btnStart.Enabled = true;
            btnStart.BackColor = Color.Green;
            btnStop.Enabled = false;
            btnStop.BackColor = Color.Gray;

            lblStatus.ForeColor = Color.Black;
            lblStatus.BackColor = Color.White;
            lblStatus.Text = "Idle        ";

            logFileWatcher?.Dispose();
            logCheckTimer?.Dispose();
            DisplayOnConsole("Stopped listening.");
        }

        private void CheckForReadOperation()
        {
            try
            {
                Invoke(new Action(() =>
                {
                    var lines = ReadAllLinesSafe(logFilePath);
                    string lastLine = lines.LastOrDefault();

                    if (lastLine != null && lastLine.Contains("Executing method"))
                    {
                        DisplayOnConsole("Read operation (executing) detected.");

                        for (int i = lines.Count - 1; i >= 0; i--)
                        {
                            if (lines[i].Contains("Launching Command - 'Read'", StringComparison.OrdinalIgnoreCase))
                            {
                                barcode = ExtractBarcode(lines[i]);
                                barcode = barcode.Replace(",", "");
                                DisplayOnConsole($"Barcode: {barcode}");
                                break;
                            }
                        }

                        StartListeningForChanges();
                        logCheckTimer?.Dispose();
                    }
                }));
            }
            catch (ObjectDisposedException) { /* Ignore on close */ }
            catch (Exception ex)
            {
                DisplayOnConsole($"Error (Check For Read Operation): {ex.Message}");
            }
        }

        private void StartListeningForChanges()
        {
            // Remove the watcher for old log file (new log file is created every day)
            logFileWatcher?.Dispose();

            DisplayOnConsole("Waiting for read operation to finish...");

            // TODO: Just being extra careful with LastWrite and Size...maybe its overkill?
            logFileWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(logFilePath),
                Filter = Path.GetFileName(logFilePath),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
            };
            logFileWatcher.Changed += (s, e) => OnLogChanged();
            logFileWatcher.EnableRaisingEvents = true;
        }

        private void OnLogChanged()
        {
            logFileWatcher?.Dispose();
            Thread.Sleep(2000);

            try
            {
                if (!string.IsNullOrEmpty(barcode))
                {
                    string latestFolder = GetLatestResultFolder(resultsPathParent);
                    if (latestFolder != null)
                    {
                        string originalName = Path.GetFileName(latestFolder);
                        DisplayOnConsole($"Latest read results folder: {originalName}");
                        string newFolderName = $"{barcode}_{originalName}";
                        string newPath = Path.Combine(resultsPathParent, newFolderName);

                        if (!Directory.Exists(newPath))
                        {
                            Directory.Move(latestFolder, newPath);
                            DisplayOnConsole($"Renamed: {newFolderName}");
                        }
                    }
                    else
                    {
                        DisplayOnConsole("No recent results folder found.");
                    }
                }
                else
                {
                    DisplayOnConsole("No barcode found, so no renaming of results folder.");
                }

                DisplayOnConsole($"Now listening: {logFilePath}");
                logCheckTimer = new System.Threading.Timer(
                    callback: _ => CheckForReadOperation(),
                    state: null,
                    dueTime: TimeSpan.Zero,
                    period: TimeSpan.FromSeconds(2));
            }
            catch (Exception ex)
            {
                DisplayOnConsole("Error (On Log Changed): " + ex.Message);
            }
        }

        private string ExtractBarcode(string barcodeLine)
        {
            var match = Regex.Match(barcodeLine, @"SampleName:\s*(\S+)", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : null;
        }

        private string GetLatestResultFolder(string parentResultsFolder)
        {
            var directories = Directory.GetDirectories(parentResultsFolder)
                                       .OrderByDescending(Directory.GetCreationTime)
                                       .ToList();

            return directories.FirstOrDefault();
        }

        private void DisplayOnConsole(string message)
        {
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            textConsole.ForeColor = Color.Gray;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{timeStamp}   ");
            textConsole.ForeColor = Color.DarkBlue;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"|   {message}");
        }

        private void clearConsole_Click(object sender, EventArgs e)
        {
            textConsole.Clear();
        }

        private List<string> ReadAllLinesSafe(string logPath)
        {
            List<string> lines = new List<string>();
            try
            {
                using (var fs = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        lines.Add(sr.ReadLine());
                    }
                }
            }
            catch (IOException ex)
            {
                DisplayOnConsole($"Error (File Assist): {ex.Message}");
            }
            return lines;
        }
    }
}

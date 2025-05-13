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

        private void btnStopListening(object sender, EventArgs e)
        {
            logFileWatcher?.Dispose();
            btnStart.Enabled = true;
            lblStatus.ForeColor = Color.Black;
            lblStatus.Text = "Idle        ";
            DisplayOnConsole("Stopped listening.");
        }

        private void btnStartListening(object sender, EventArgs e)
        {
            string logPath = txtLogFile.Text;
            string resultsPath = txtResultsFolder.Text;

            if (!File.Exists(logPath) || !Directory.Exists(resultsPath))
            {
                MessageBox.Show("Please provide valid paths.");
                return;
            }

            btnStart.Enabled = false;
            StartListening(logPath, resultsPath);
            lblStatus.ForeColor = Color.Red;
            lblStatus.Text = "Running";
        }

        private void StartListening(string logPath, string resultsPath)
        {
            // Remove the watcher for old log file (new log file is created every day)
            logFileWatcher?.Dispose();

            DisplayOnConsole($"Now listening: {logPath}");

            // TODO: Just being extra careful with LastWrite and Size...maybe its overkill?
            logFileWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(logPath),
                Filter = Path.GetFileName(logPath),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
            };
            logFileWatcher.Changed += (s, e) => OnLogChanged(e.FullPath, resultsPath);
            logFileWatcher.EnableRaisingEvents = true;
        }

        private void OnLogChanged(string logPath, string parentResultsFolder)
        {
            DateTime rightNow = DateTime.Now;
            if ((rightNow - lastUpdateTime) < debounceDelay) // skip
            {
                return;
            }
            lastUpdateTime = rightNow;

            Thread.Sleep(500); // TODO: Allow RDW to finish writing...don't know if this is too little or too much?

            try
            {
                string[] lines = File.ReadAllLines(logPath);
                string lastLine = lines.LastOrDefault();
                string barcode = null;

                if (lastLine != null && lastLine.Contains("Read - Command Complete finished"))
                {
                    for (int i = lines.Length - 1; i >= 0; i--)
                    {
                        if (lines[i].Contains("Launching Command - 'Read'", StringComparison.OrdinalIgnoreCase))
                        {
                            DisplayOnConsole("Read operation completion detected.");
                            barcode = ExtractBarcode(lines[i]);
                            barcode = barcode.Replace(",", "");
                            DisplayOnConsole($"Barcode: {barcode}");
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(barcode))
                    {
                        string latestFolder = GetLatestResultFolder(parentResultsFolder);
                        if (latestFolder != null)
                        {
                            string originalName = Path.GetFileName(latestFolder);
                            DisplayOnConsole($"Latest results folder: {originalName}");
                            string newFolderName = $"{barcode}_{originalName}";
                            string newPath = Path.Combine(parentResultsFolder, newFolderName);

                            if (!Directory.Exists(newPath))
                            {
                                Directory.Move(latestFolder, newPath);
                                DisplayOnConsole($"Renamed: {newFolderName}");
                            }
                        }
                        else
                        {
                            DisplayOnConsole("No recent result folder found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayOnConsole("Error: " + ex.Message);
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
    }
}

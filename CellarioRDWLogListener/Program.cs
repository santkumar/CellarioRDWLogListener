using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace CellarioRDWLogListener
{
    class Program
    {
        /*        static string logDirectoryRDW = @"C:\Program Files\HighRes Biosolutions\RemoteDriverWrapper\Logs";
                static string logFilePathRDW = @"C:\Program Files\HighRes Biosolutions\RemoteDriverWrapper\Logs\Wrapper_Driver_Driver.TecanSpark 1.1.log";
                static string sparkOutputFolderPath = @"D:\Tecan\Workspaces";
                static DateTime lastUpdateTime = DateTime.MinValue;
                static readonly TimeSpan debounceDelay = TimeSpan.FromSeconds(2);

                static FileSystemWatcher logFileWatcher;
                static FileSystemWatcher newFileDetector;
                static string currentLogFilePath;
        */
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormGUI());
            /*            Console.WriteLine("Starting Cellario remote driver wrapper log listener...");

                        // Detect new RDW log files
                        newFileDetector = new FileSystemWatcher
                        {
                            Path = logDirectoryRDW,
                            Filter = "Wrapper_Driver_Driver.TecanSpark 1.1.log",
                            NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime
                        };
                        newFileDetector.Created += OnNewLogFileDetected;
                        newFileDetector.EnableRaisingEvents = true;

                        // Start with latest existing RDW log file
                        string latestLog = GetLatestLogFile();
                        if (latestLog != null) // TODO: Handle other cases?
                            StartListeningLogFile(latestLog);

                        Console.WriteLine("Listening...Press any key to exit."); // TODO: It should never stop by mistake!
                        Console.ReadKey();
            */
        }

/*        static string GetLatestLogFile()
        {
            // TODO: I remember two new log files are created every day...we only need to select the specific driver file
            return Directory.GetFiles(logDirectoryRDW, "Wrapper_Driver_Driver.TecanSpark 1.1.log")
                            .OrderByDescending(File.GetCreationTime)
                            .FirstOrDefault();
        }

        static void OnNewLogFileDetected(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"New log file detected: {e.FullPath}");
            StartListeningLogFile(e.FullPath);
        }

        static void StartListeningLogFile(string path)
        {
            // Remove the watcher for old log file (new log file is created every day)
            logFileWatcher?.Dispose();

            currentLogFilePath = path;

            // TODO: Just being extra careful with LastWrite and Size...maybe its overkill?
            logFileWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(path),
                Filter = Path.GetFileName(path),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
            };
            logFileWatcher.Changed += OnLogChanged;
            logFileWatcher.EnableRaisingEvents = true;

            Console.WriteLine($"Now watching: {path}");
        }

        static void OnLogChanged(object sender, FileSystemEventArgs e)
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
                string[] lines = File.ReadAllLines(e.FullPath);
                string lastLine = lines.LastOrDefault();
                string sparkResultFolderPath = null;
                string barcode = null;

                if (lastLine != null && lastLine.Contains("Read - Command Complete finished"))
                {
                    for (int i = lines.Length - 1; i >= 0; i--)
                    {
                        if (lines[i].Contains("Output file saved to: ", StringComparison.OrdinalIgnoreCase))
                        {
                            sparkResultFolderPath = ExtractSparkOutputFolder(lines[i]);
                            Console.WriteLine($"Spark output folder: {sparkResultFolderPath}");
                        }

                        if (lines[i].Contains("Launching Command - 'Read'", StringComparison.OrdinalIgnoreCase))
                        {
                            barcode = ExtractBarcode(lines[i]);
                            barcode = barcode.Replace(",", "");
                            Console.WriteLine($"Barcode: {barcode}");
                            break;
                        }                        
                    }

                    if (!string.IsNullOrEmpty(barcode))
                    {
                        string latestFolder = GetLatestResultFolder();
                        if (latestFolder != null)
                        {
                            string originalName = Path.GetFileName(latestFolder);
                            string newFolderName = $"{barcode}_{originalName}";
                            string newPath = Path.Combine(sparkOutputFolderPath, newFolderName);

                            if (!Directory.Exists(newPath))
                            {
                                Directory.Move(latestFolder, newPath);
                                Console.WriteLine($"Renamed: {originalName} -> {newFolderName}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No recent result folder found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static string ExtractBarcode(string barcodeLine)
        {
            var match = Regex.Match(barcodeLine, @"SampleName:\s*(\S+)", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : null;
        }

        static string ExtractSparkOutputFolder(string inputString)
        {
            var match = Regex.Match(inputString, @"(D:\\Tecan\\Workspaces\\[^\\]*_\d{8}_\d{6})", RegexOptions.IgnoreCase);
            return match.Success ? match.Value : null;
        }

        static string GetLatestResultFolder()
        {
            var directories = Directory.GetDirectories(sparkOutputFolderPath)
                                       .OrderByDescending(Directory.GetCreationTime)
                                       .ToList();

            return directories.FirstOrDefault();
        }
*/
    }
}
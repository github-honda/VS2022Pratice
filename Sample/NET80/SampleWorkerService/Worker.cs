using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;

namespace SampleWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private const string ConfigFilePath = @"C:\temp\config.json";

        #region Source and LogName
        /*

        public static void CreateEventSource(string source, string logName);
        source
        The source name by which the application is registered on the local computer.

        logName
        The name of the log the source's entries are written to. 
        Possible values include Application, System, or a custom event log.

        */

        const string _EventLogSource = "MyWorkerService"; // WriteEntry() requires a source name
        const string _EventLogName = "Application"; // EventLog.CreateEventSource()   The name of the event log to write to 

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            if (!EventLog.SourceExists(_EventLogSource))
            {
                EventLog.CreateEventSource(_EventLogSource, _EventLogName);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int iDelaySeconds = 16; // 預設延遲時間, 若可讀到設定值, 則可覆蓋此值.
            string sErrorFile = Path.Combine(@"C:\temp\OutputBox", "ErrorLog.txt"); // 預設錯誤檔案, 若可讀到設定值, 則可覆蓋此值.
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var config = LoadConfig();
                    if (config == null)
                    {
                        _logger.LogError("Failed to load configuration.");
                        await Task.Delay(TimeSpan.FromSeconds(iDelaySeconds), stoppingToken);
                        continue;
                    }

                    if (config.Enable == 0)
                    {
                        LogEvent("Service paused (Enable=0)");
                        await Task.Delay(TimeSpan.FromSeconds(iDelaySeconds), stoppingToken);
                        continue;
                    }

                    await ProcessFilesAsync(config);
                }
                catch (Exception ex)
                {
                    //File.WriteAllText(@"C:\temp\OutputBox\ErrorLog.txt", ex.Message);
                    File.WriteAllText(sErrorFile, ex.Message);
                    LogEvent($"Error occurred: {ex.Message}");
                    _logger.LogError(ex, "An error occurred.");
                }

                await Task.Delay(TimeSpan.FromSeconds(iDelaySeconds), stoppingToken);
            }


        }
        private Config? LoadConfig()
        {
            var json = File.ReadAllText(ConfigFilePath);
            return JsonSerializer.Deserialize<Config>(json);
        }

        private async Task ProcessFilesAsync(Config config)
        {
            if (!Directory.Exists(config.InputFolder))
            {
                LogEvent("Input folder does not exist.");
                return;
            }

            var files = Directory.GetFiles(config.InputFolder);
            if (files.Length == 0)
            {
                LogEvent("No files to process.");
                return;
            }

            var copyTasks = new ConcurrentBag<Task>();

            Parallel.ForEach(files, file =>
            {
                var fileName = Path.GetFileName(file);

                try
                {
                    copyTasks.Add(Task.Run(() =>
                    {
                        File.Copy(file, Path.Combine(config.OutputFolder, fileName), true);
                        File.Copy(file, Path.Combine(config.BackupFolder, fileName), true);
                        LogEvent($"Copied file: {fileName}");
                    }));
                }
                catch (Exception ex)
                {
                    LogEvent($"Error copying file {fileName}: {ex.Message}");
                }
            });

            await Task.WhenAll(copyTasks);

            // 刪除檔案（確保刪除操作安全）
            foreach (var file in files)
            {
                try
                {
                    File.Delete(file);
                    LogEvent($"Deleted file: {Path.GetFileName(file)}");
                }
                catch (Exception ex)
                {
                    LogEvent($"Error deleting file {Path.GetFileName(file)}: {ex.Message}");
                }
            }
        }

        private void LogEvent(string message)
        {
            Console.WriteLine(message);
            EventLog.WriteEntry(_EventLogSource, message, EventLogEntryType.Information);
        }

    }
}

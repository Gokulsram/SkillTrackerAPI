using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using System;

namespace SkillTracker.Shared
{
    public static class Serilogging
    {
        public static void CreateLogger(IConfigurationSection logConfig)
        {
            string filePath = logConfig[nameof(LogSetting.LogFilePath)];
            string errorFilePath = logConfig[nameof(LogSetting.ErrorLogFilePath)];
            int fileSize = Convert.ToInt32(logConfig[nameof(LogSetting.LogFileSizeinMBLimit)]);
            int fileCount = Convert.ToInt32(logConfig[nameof(LogSetting.RetainedFileCountLimit)]);

            Log.Logger = new LoggerConfiguration()
                     // .ReadFrom.Configuration(null)
                     .Enrich.FromLogContext()
             .Enrich.WithEnvironmentUserName()
             .Enrich.WithMachineName()
             .Enrich.WithProcessName()
             .Enrich.WithThreadId()
             .Enrich.FromLogContext()
             .WriteTo.Logger(Logger => Logger
                                // Information Logging
                                .Filter.ByIncludingOnly(x =>
                                x.Level == LogEventLevel.Information
                                )
                   .WriteTo.Async(a => a.File(filePath,
                             outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} Server Name: {MachineName} Thread Id: {ThreadId} - {Level} - {Message} {NewLine} {Exception} {NewLine}",
                             shared: true,
                             flushToDiskInterval: TimeSpan.FromSeconds(1),
                             rollingInterval: RollingInterval.Day,
                                                     //fileSizeLimitBytes: (fileSize * 1024),
                                                     rollOnFileSizeLimit: true,
                             retainedFileCountLimit: fileCount,
                             restrictedToMinimumLevel: LogEventLevel.Information
                                      )
                              )
                           )
                    // Error Logging
                    .WriteTo.Logger(Logger => Logger
                  .WriteTo.Async(a => a.File(errorFilePath,
                             outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} Server Name: {MachineName} Thread Id: {ThreadId} -  {Level} - {Message} {NewLine} {Exception} {NewLine}",
                             shared: true,
                             flushToDiskInterval: TimeSpan.FromSeconds(1),
                                                     //fileSizeLimitBytes: (fileSize * 1024),
                                                     rollingInterval: RollingInterval.Day,
                             rollOnFileSizeLimit: true,
                             retainedFileCountLimit: fileCount,
                             restrictedToMinimumLevel: LogEventLevel.Warning)
                              )
                           )
                  .CreateLogger();
        }
    }
}

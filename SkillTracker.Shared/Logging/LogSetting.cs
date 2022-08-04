namespace SkillTracker.Shared
{
    public class LogSetting
    {
        public string LogFilePath { get; set; }
        public string ErrorLogFilePath { get; set; }
        public int RetainedFileCountLimit { get; set; }
        public int LogFileSizeinMBLimit { get; set; }
    }
}

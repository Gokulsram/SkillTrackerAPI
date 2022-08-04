namespace SkillTracker.Core
{
    public static class HttpStatusDetail
    {
        public static int SuccessCode = 200;
        public static int FailureCode = 101;
        public static int AlreadyExistsCode = 102;
        public static int NotExistsCode = 103;
        public static int InternalErrorCode = 104;
        public static int ReferenceErrorCode = 500;
        public static string SuccessMessage = "Success";
        public static string FailureMessage = "Failure";
        public static string AlreadyExistsMessage = "{0} already exists!";
        public static string NotExistsMessage = "ID {0} does not exists!";
        public static string InternalErrorMessage = "Internal Server Error";
        public static string ReferenceErrorMessage = "{0} referenced in some other object. So you can't delete";
        public static string UserNotExistsMessage = "UserID does not exists!";
        public static string ProfileUpdateAllowed = "Profile update allowed only after 10 days of added profile";
        public static string SkillNameNotFound = "Skill Name not found in the Databse!";
        public static string NoNewSkill = "You can't able to add new skills, only updates are allowed";
    }
}

namespace Tdev702.Auth.Routes;

public static partial class ApiRoutes
{
    public static class Auth
    {
        private const string Base = $"{BaseApi}/auth";

        public static string Login = $"{Base}/login";
        public static string Register = $"{Base}/register";
        public static string Refresh = $"{Base}/refresh";
        public static string ConfirmEmail = $"{Base}/confirm-email/{{userId}}/{{token}}";
        public static string ResendConfirmation = $"{Base}/resend-confirmation";
        public static string ForgotPassword = $"{Base}/forgot-password";
        public static string ResetPassword = $"{Base}/reset-password";
        public static string Verify2FA = $"{Base}/verify-2fa";
    }
}
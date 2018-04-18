using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib11.StringTools;
using System;

namespace PassbookTally.DomainLib.Authorization
{
    public static class AccessControlExtensions
    {
        public static Action<string> OnUnauthorizedAccess;


        public static bool CanAddBankTransaction(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Add Bank Transaction", "Cashier", "Supervisor", "Admin");

        public static bool CanEditBankTransaction(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Edit Bank Transaction", "Cashier", "Supervisor", "Admin");

        public static bool CanDeleteBankTransaction(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Delete Bank Transaction", "Cashier", "Supervisor", "Admin");


        public static bool CanAddVoucherRequest(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Add Voucher Request", "Supervisor", "Admin");

        public static bool CanEditVoucherRequest(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Edit Voucher Request", "Supervisor", "Admin");

        public static bool CanDeleteVoucherRequest(this ICredentialsProvider creds, bool alertIfNotAllowed) => creds.Check(alertIfNotAllowed,
            "Delete Voucher Request", "Supervisor", "Admin");


        private static bool Check(this ICredentialsProvider args,
            bool alertIfNotAllowed, string intendedAction, params string[] allowedRoles)
        {
            if (args == null) return false;
            if (!args.IsValidUser) return false;
            if (args.Credentials == null) return false;

            var creds = args.Credentials;
            if (creds.Roles.IsBlank()) return false;

            foreach (var role in allowedRoles)
                if (creds.Roles.Contains(role)) return true;

            if (alertIfNotAllowed)
                OnUnauthorizedAccess?.Invoke($"“{creds.HumanName}” ({creds.Roles}) {L.f} is not allowed to {L.f} “{intendedAction}”.");

            return false;
        }
    }
}

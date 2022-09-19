using System;
using TKR.Shared.database;

namespace TKR.Shared.utils
{
    public static class StatusInfoUtil
    {
        public static string GetInfo(this DbLoginStatus status)
        {
            switch (status)
            {
                case DbLoginStatus.InvalidCredentials:
                    return "Bad Login";

                case DbLoginStatus.AccountNotExists:
                    return "Bad Login";

                case DbLoginStatus.OK:
                    return "OK";
            }
            throw new ArgumentException("status");
        }

        public static string GetInfo(this DbRegisterStatus status)
        {
            switch (status)
            {
                case DbRegisterStatus.UsedName:
                    return "Duplicate Email"; // maybe not wise to give this info out...
                case DbRegisterStatus.OK:
                    return "OK";
            }
            throw new ArgumentException("status");
        }

        public static string GetInfo(this DbCreateStatus status)
        {
            switch (status)
            {
                case DbCreateStatus.ReachCharLimit:
                    return "Too many characters";

                case DbCreateStatus.OK:
                    return "OK";
            }
            throw new ArgumentException("status");
        }
    }
}

using common.database;
using System;

namespace common.utils
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

        public static string GetInfo(this RegisterStatus status)
        {
            switch (status)
            {
                case RegisterStatus.UsedName:
                    return "Duplicate Email"; // maybe not wise to give this info out...
                case RegisterStatus.OK:
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

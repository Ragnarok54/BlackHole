using System;
using System.IO;
using System.Reflection;

namespace BlackHole.Common
{
    public static class Constants
    {
        #region Messages
        
        public const string RemovedMessageText = "{[(Removed_Message)]}";
        public const int RemoveMessageTimeInMinutes = 5;

        #endregion

        #region Hub Methods

        public const string ReceiveHubMessageMethod = "ReceiveOne";
        public const string StatusActiveHubMethod = "StatusUpdateActive";
        public const string StatusInactiveHubMethod = "StatusUpdateInactive";
        public const string StatusHubAllActive = "StatusHubAllActive";

        #endregion

        public const int JwtExpireTimeInHours = 24;
    }
}

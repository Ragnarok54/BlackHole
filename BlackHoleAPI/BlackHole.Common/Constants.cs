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

        public static readonly string ApplicationFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static readonly byte[] _picture = File.ReadAllBytes(Path.Combine(ApplicationFolder, "Resources", "DefaultIcon.png"));

        public static readonly string DefaultPicture = Convert.ToBase64String(_picture);
    }
}

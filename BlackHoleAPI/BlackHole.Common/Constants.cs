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

        #endregion

        public const int JwtExpireTimeInHours = 12;
    }
}

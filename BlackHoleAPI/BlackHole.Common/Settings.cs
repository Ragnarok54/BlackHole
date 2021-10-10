using System;
using Microsoft.Extensions.Configuration;


namespace BlackHole.Common
{
    public static class Settings
    {
        private static IConfiguration _configuration;

        public static void SetConfig(IConfiguration configuration)
        {
            if (_configuration == null)
            {
                _configuration = configuration;
            }
        }

        public static string DatabaseConnectionString
        {
            get
            {
                var connectionString = _configuration.GetConnectionString("DatabaseConnectionString");

                if (!string.IsNullOrEmpty(connectionString))
                {
                    return connectionString;
                }

                throw new InvalidOperationException("Invalid configuration value for database connectiong string");
            }
        }

        public static string TokenSecret
        {
            get
            {
                var tokenSecret = _configuration["AppSettings:TokenSecret"];

                if (!string.IsNullOrEmpty(tokenSecret))
                {
                    return tokenSecret;
                }

                throw new InvalidOperationException("Invalid configuration value for JWT Token Secret");
            }
        }

        public static byte[] TokenSecretBytes
        {
            get
            {
                return System.Text.Encoding.ASCII.GetBytes(TokenSecret);
            }
        }
    }
}

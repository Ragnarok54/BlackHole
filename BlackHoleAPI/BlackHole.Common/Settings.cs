﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace BlackHole.Common
{
    public static class Settings
    {
        private static IConfiguration _configuration;

        public static void SetConfig(IConfiguration configuration)
        {
            _configuration ??= configuration;
        }

        public static string Version
        {
            get
            {
                var version = _configuration["AppSettings:Version"];
                
                if (string.IsNullOrEmpty(version))
                {
                    throw new InvalidOperationException("Invalid configuration value for Version");
                }

                return version;
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

        public static IEnumerable<string> CorsOrigins
        {
            get
            {
                var origins = _configuration["AppSettings:CorsOrigins"];

                if (!string.IsNullOrEmpty(origins))
                {
                    return origins.Split(",");
                }

                throw new InvalidOperationException("Invalid configuration value for Cors Origins");
            }
        }
    }
}

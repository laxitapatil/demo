using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;

        // Minutes for access token
        public int AccessTokenExpirationMinutes { get; set; } = 60; // 1 hour

        // Days for refresh token
        public int RefreshTokenExpirationDays { get; set; } = 7;
    }
}

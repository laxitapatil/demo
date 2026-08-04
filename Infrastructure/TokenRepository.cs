using Core;
using Core.Domain;
using Core.Response;
using Infrastructure.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class TokenRepository
    {
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly DBContext _context;
        private readonly JwtSettings _jwtSettings;

        public TokenRepository(
            UserManager<AspNetUsers> userManager,
            DBContext context,
            IOptions<JwtSettings> jwtOptions)
        {
            _userManager = userManager;
            _context = context;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<LoginResponse> GenerateTokensAsync(AspNetUsers user, string ipAddress)
        {
            // Fetch roles from UserManager
            var roles = (await _userManager.GetRolesAsync(user)).ToList();

            var accessToken = GenerateJwtToken(user, roles);
            var refreshToken = GenerateRefreshToken(ipAddress);

            refreshToken.aspnet_users_id = user.Id;

            _context.RefreshTokens.Add(refreshToken);
            RemoveOldRefreshTokens(user.Id);

            await _context.SaveChangesAsync();

            return new LoginResponse
            {
                Id = user.Id,
                User_id = user.User_id,
                Username = user.UserName ?? string.Empty,
                Name = user.Name,
                Email = user.Email,

                Token = accessToken.Token,
                Expiry = accessToken.Expires,

                RefreshToken = refreshToken.Token,
                RefreshTokenExpiresAt = refreshToken.Expires,

                Role = roles
            };
        }

        public async Task<LoginResponse?> RefreshTokenAsync(string refreshToken, string ipAddress)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == refreshToken);

            if (token == null || !token.Is_active)
                return null;

            var user = await _userManager.FindByIdAsync(token.aspnet_users_id);
            if (user == null)
                return null;

            var roles = (await _userManager.GetRolesAsync(user)).ToList();

            var accessToken = GenerateJwtToken(user, roles);

            var newRefreshToken = GenerateRefreshToken(ipAddress);

            token.Revoked = DateTime.UtcNow;
            token.Revoked_by_ip = ipAddress;
            token.Replaced_by_token = newRefreshToken.Token;

            newRefreshToken.aspnet_users_id = user.Id;

            _context.RefreshTokens.Add(newRefreshToken);
            RemoveOldRefreshTokens(user.Id);

            await _context.SaveChangesAsync();

            return new LoginResponse
            {
                Id = user.Id,
                User_id = user.User_id,
                Username = user.UserName ?? string.Empty,
                Name = user.Name,
                Email = user.Email,

                Token = accessToken.Token,
                Expiry = accessToken.Expires,

                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiresAt = newRefreshToken.Expires,

                Role = roles
            };
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken, string ipAddress)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == refreshToken);

            if (token == null || !token.Is_active)
                return false;

            token.Revoked = DateTime.UtcNow;
            token.Revoked_by_ip = ipAddress;

            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();

            return true;
        }

        // ----- helpers -----
        private (string Token, DateTime Expires) GenerateJwtToken(AspNetUsers user, List<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Id", user.Id),                  // string (Identity Id)
                new Claim("UserId", user.User_id.ToString()), // int (business user id)
                new Claim("Name", user.Name)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return (tokenString, expires);
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                Created_date = DateTime.UtcNow,
                Created_by_ip = ipAddress
            };
        }

        private void RemoveOldRefreshTokens(string userId)
        {
            var tokensToRemove = _context.RefreshTokens
                .Where(r =>
                r.aspnet_users_id == userId &&
                    (
                        r.Revoked != null ||
                        r.Expires <= DateTime.UtcNow
                    ) &&
                        r.Created_date.AddDays(_jwtSettings.RefreshTokenExpirationDays) <= DateTime.UtcNow
                );

            if (tokensToRemove.Any())
                _context.RefreshTokens.RemoveRange(tokensToRemove);
        }
    }
}

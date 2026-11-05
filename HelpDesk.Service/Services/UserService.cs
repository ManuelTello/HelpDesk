using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HelpDesk.DAL.Data;
using HelpDesk.DAL.Models;
using HelpDesk.Service.Abstractions;
using HelpDesk.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HelpDesk.Service.Services;

public class UserService : IUserService
{
    private readonly ApplicationContext _dbContext;

    public UserService(ApplicationContext context)
    {
        _dbContext = context;
    }

    public async Task CreateUser(string username, string fullname, string password, UserRole role)
    {
        bool alreadyExists = await _dbContext.Users.AnyAsync(u => u.UserName == username);
        if (alreadyExists)
        {
            throw new UserNameAlreadyTaken(username);
        }
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        User user = new User()
        {
            UserName = username,
            FullName = fullname,
            Password = hashedPassword,
            Role = role,
        };
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<string> ValidateCredentials(string userName, string password, string issuer, string audience, string key, double expirationTime)
    {
        User user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == userName) ?? throw new UserInvalidCredentailsException();
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
        if (!isPasswordValid)
        {
            throw new UserInvalidCredentailsException();
        }

        Claim[] claims =
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken securityToken = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, expires: DateTime.Now.AddMinutes(expirationTime), signingCredentials: credentials);
        string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;
    }
}

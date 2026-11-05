using HelpDesk.DAL.Models;

namespace HelpDesk.Service.Abstractions;

public interface IUserService
{
    public Task CreateUser(string username, string fullname, string password, UserRole role);

    public Task<string> ValidateCredentials(string username, string password, string issuer, string audience, string key, double expirationTime);
}

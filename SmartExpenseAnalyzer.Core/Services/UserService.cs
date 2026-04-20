using SmartExpenseAnalyzer.Core.Interfaces;
using SmartExpenseAnalyzer.Core.Models;
using SmartExpenseAnalyzer.Shared.Helpers;

namespace SmartExpenseAnalyzer.Core.Services;

public class UserService : IUserService {
    private readonly IUserRepository _repo;
    public UserService(IUserRepository repo) {
        _repo = repo;
    }

    public User? Login(string username, string password) {
        var user = _repo.GetByUsername(username);
        if (user != null && PasswordHasher.VerifyPassword(password, user.PasswordHash)) {
            return user;
        }
        return null;
    }

    public void Register(string username, string password) {
        if (_repo.GetByUsername(username) != null) {
            throw new System.Exception("Username already exists");
        }
        var user = new User {
            Username = username,
            PasswordHash = PasswordHasher.HashPassword(password)
        };
        _repo.Add(user);
    }
}

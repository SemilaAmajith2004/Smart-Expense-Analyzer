using System.Linq;
using SmartExpenseAnalyzer.Core.Interfaces;
using SmartExpenseAnalyzer.Core.Models;
using SmartExpenseAnalyzer.Infrastructure.Data;

namespace SmartExpenseAnalyzer.Infrastructure.Repositories;

public class UserRepository : IUserRepository {
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) {
        _context = context;
    }

    public User? GetByUsername(string username) => 
        _context.CurrentDb.Users.FirstOrDefault(u => u.Username == username);

    public void Add(User user) {
        user.Id = _context.CurrentDb.Users.Count > 0 ? _context.CurrentDb.Users.Max(u => u.Id) + 1 : 1;
        _context.CurrentDb.Users.Add(user);
        _context.SaveChanges();
    }
}

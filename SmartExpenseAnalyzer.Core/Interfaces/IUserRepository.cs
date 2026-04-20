namespace SmartExpenseAnalyzer.Core.Interfaces;
using SmartExpenseAnalyzer.Core.Models;

public interface IUserRepository {
    User? GetByUsername(string username);
    void Add(User user);
}

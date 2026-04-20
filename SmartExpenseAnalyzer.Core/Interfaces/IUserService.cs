using SmartExpenseAnalyzer.Core.Models;
namespace SmartExpenseAnalyzer.Core.Interfaces;
public interface IUserService {
    User? Login(string username, string password);
    void Register(string username, string password);
}

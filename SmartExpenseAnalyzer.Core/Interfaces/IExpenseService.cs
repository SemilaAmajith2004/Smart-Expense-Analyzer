using System.Collections.Generic;
using SmartExpenseAnalyzer.Core.Models;
namespace SmartExpenseAnalyzer.Core.Interfaces;
public interface IExpenseService {
    void AddExpense(Expense expense);
    IEnumerable<Expense> GetExpenses(int userId);
    void DeleteExpense(int id);
}

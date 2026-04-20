using System.Collections.Generic;
using SmartExpenseAnalyzer.Core.Interfaces;
using SmartExpenseAnalyzer.Core.Models;

namespace SmartExpenseAnalyzer.Core.Services;

public class ExpenseService : IExpenseService {
    private readonly IExpenseRepository _repo;
    public ExpenseService(IExpenseRepository repo) {
        _repo = repo;
    }

    public void AddExpense(Expense expense) => _repo.Add(expense);
    public IEnumerable<Expense> GetExpenses(int userId) => _repo.GetByUserId(userId);
    public void DeleteExpense(int id) => _repo.Delete(id);
}

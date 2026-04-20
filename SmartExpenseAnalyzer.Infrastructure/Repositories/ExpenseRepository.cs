using System.Collections.Generic;
using System.Linq;
using SmartExpenseAnalyzer.Core.Interfaces;
using SmartExpenseAnalyzer.Core.Models;
using SmartExpenseAnalyzer.Infrastructure.Data;

namespace SmartExpenseAnalyzer.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository {
    private readonly AppDbContext _context;
    public ExpenseRepository(AppDbContext context) {
        _context = context;
    }

    public IEnumerable<Expense> GetByUserId(int userId) => 
        _context.CurrentDb.Expenses.Where(e => e.UserId == userId);

    public void Add(Expense expense) {
        expense.Id = _context.CurrentDb.Expenses.Count > 0 ? _context.CurrentDb.Expenses.Max(e => e.Id) + 1 : 1;
        _context.CurrentDb.Expenses.Add(expense);
        _context.SaveChanges();
    }

    public void Delete(int id) {
        var expense = _context.CurrentDb.Expenses.FirstOrDefault(e => e.Id == id);
        if (expense != null) {
            _context.CurrentDb.Expenses.Remove(expense);
            _context.SaveChanges();
        }
    }
}

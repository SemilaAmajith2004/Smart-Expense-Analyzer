namespace SmartExpenseAnalyzer.Core.Interfaces;
using SmartExpenseAnalyzer.Core.Models;
using System.Collections.Generic;

public interface IExpenseRepository {
    IEnumerable<Expense> GetByUserId(int userId);
    void Add(Expense expense);
    void Delete(int id);
}

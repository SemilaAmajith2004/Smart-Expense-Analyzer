using System;
using System.Windows.Forms;
using SmartExpenseAnalyzer.Core.Services;
using SmartExpenseAnalyzer.Infrastructure.Data;
using SmartExpenseAnalyzer.Infrastructure.Repositories;

namespace SmartExpenseAnalyzer.UI;

internal static class Program {
    [STAThread]
    static void Main() {
        ApplicationConfiguration.Initialize();

        // Setup manual DI for simplicity
        var dbContext = new AppDbContext();
        var userRepo = new UserRepository(dbContext);
        var expenseRepo = new ExpenseRepository(dbContext);
        
        var userService = new UserService(userRepo);
        var expenseService = new ExpenseService(expenseRepo);

        Application.Run(new Forms.LoginForm(userService, expenseService));
    }
}

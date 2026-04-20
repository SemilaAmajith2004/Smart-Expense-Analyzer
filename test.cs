using System;
using SmartExpenseAnalyzer.Infrastructure.Data;
using SmartExpenseAnalyzer.Core.Models;
using SmartExpenseAnalyzer.Infrastructure.Repositories;

namespace TestApp {
    class Program {
        static void Main() {
            try {
                var dbContext = new AppDbContext();
                var userRepo = new UserRepository(dbContext);
                userRepo.Add(new User { Username = "test", PasswordHash = "test" });
                Console.WriteLine("User added! File exists: " + System.IO.File.Exists("data.json"));
            }catch(Exception ex){
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

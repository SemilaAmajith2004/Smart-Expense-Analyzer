using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SmartExpenseAnalyzer.Core.Models;

namespace SmartExpenseAnalyzer.Infrastructure.Data;

public class AppDbContext {
    private readonly string _filePath = "data.json";
    
    public Database CurrentDb { get; set; } = new Database();

    public AppDbContext() {
        Load();
    }

    public void Load() {
        if (File.Exists(_filePath)) {
            var json = File.ReadAllText(_filePath);
            CurrentDb = JsonSerializer.Deserialize<Database>(json) ?? new Database();
        }
    }

    public void SaveChanges() {
        var json = JsonSerializer.Serialize(CurrentDb);
        File.WriteAllText(_filePath, json);
    }
}

public class Database {
    public List<User> Users { get; set; } = new();
    public List<Expense> Expenses { get; set; } = new();
}

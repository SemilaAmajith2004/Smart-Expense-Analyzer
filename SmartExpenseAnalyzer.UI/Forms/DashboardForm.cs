using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartExpenseAnalyzer.Core.Interfaces;
using SmartExpenseAnalyzer.Core.Models;

namespace SmartExpenseAnalyzer.UI.Forms;

public class DashboardForm : Form {
    private readonly User _user;
    private readonly IExpenseService _expenseService;
    private DataGridView grid = new DataGridView();
    private Label lblTotal = new Label();
    
    private TextBox txtTitle = new TextBox();
    private TextBox txtAmount = new TextBox();
    private DateTimePicker dtpDate = new DateTimePicker();
    private TextBox txtCategory = new TextBox();
    private Button btnAdd = new Button();

    public DashboardForm(User user, IExpenseService expenseService) {
        _user = user;
        _expenseService = expenseService;
        InitializeComponent();
        RefreshData();
    }

    private void InitializeComponent() {
        Text = $"Dashboard - Welcome {_user.Username}";
        Size = new Size(600, 480);
        StartPosition = FormStartPosition.CenterScreen;

        grid.Location = new Point(20, 20);
        grid.Size = new Size(540, 200);
        grid.ReadOnly = true;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        lblTotal.Location = new Point(20, 240);
        lblTotal.AutoSize = true;
        lblTotal.Font = new Font(Font, FontStyle.Bold);

        var lblTitle = new Label { Text = "Title:", Location = new Point(20, 280), AutoSize = true };
        txtTitle.Location = new Point(80, 280);
        
        var lblAmount = new Label { Text = "Amount:", Location = new Point(200, 280), AutoSize = true };
        txtAmount.Location = new Point(260, 280);

        var lblDate = new Label { Text = "Date:", Location = new Point(20, 320), AutoSize = true };
        dtpDate.Location = new Point(80, 320);
        dtpDate.Format = DateTimePickerFormat.Short;
        
        var lblCategory = new Label { Text = "Category:", Location = new Point(200, 320), AutoSize = true };
        txtCategory.Location = new Point(260, 320);

        btnAdd.Text = "Add Expense";
        btnAdd.Location = new Point(400, 290);
        btnAdd.Size = new Size(120, 40);
        btnAdd.Click += BtnAdd_Click;

        Controls.Add(grid);
        Controls.Add(lblTotal);
        Controls.Add(lblTitle);
        Controls.Add(txtTitle);
        Controls.Add(lblAmount);
        Controls.Add(txtAmount);
        Controls.Add(lblDate);
        Controls.Add(dtpDate);
        Controls.Add(lblCategory);
        Controls.Add(txtCategory);
        Controls.Add(btnAdd);
    }

    private void BtnAdd_Click(object? sender, EventArgs e) {
        if (decimal.TryParse(txtAmount.Text, out decimal amount)) {
            _expenseService.AddExpense(new Expense {
                UserId = _user.Id,
                Title = txtTitle.Text,
                Amount = amount,
                Date = dtpDate.Value,
                Category = txtCategory.Text
            });
            RefreshData();
            txtTitle.Clear();
            txtAmount.Clear();
            txtCategory.Clear();
        } else {
            MessageBox.Show("Please enter a valid amount (e.g. 50.00).", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void RefreshData() {
        var expenses = _expenseService.GetExpenses(_user.Id).ToList();
        grid.DataSource = expenses;
        lblTotal.Text = $"Total Expenses: ${expenses.Sum(x => x.Amount):0.00}";
    }
}

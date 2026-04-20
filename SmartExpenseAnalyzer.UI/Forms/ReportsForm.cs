using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SmartExpenseAnalyzer.Core.Interfaces;
using SmartExpenseAnalyzer.Core.Models;
using SmartExpenseAnalyzer.UI.Controls;

namespace SmartExpenseAnalyzer.UI.Forms;

public class ReportsForm : Form {
    private readonly User _user;
    private readonly IExpenseService _expenseService;
    
    private ChartControl pieChart = new ChartControl();
    private Button btnWeekly = new Button();
    private Button btnMonthly = new Button();
    private Button btnReset = new Button();
    private Label lblTitle = new Label();

    private readonly Color bgColor = Color.FromArgb(30, 30, 36); 
    private readonly Color panelColor = Color.FromArgb(43, 43, 54);
    private readonly Color primaryColor = Color.FromArgb(0, 150, 255); 
    private readonly Color textColor = Color.White;

    public ReportsForm(User user, IExpenseService expenseService) {
        _user = user;
        _expenseService = expenseService;
        ApplyDesign();
        InitializeComponent();
        LoadData("All");
    }

    private void ApplyDesign() {
        this.BackColor = bgColor;
        this.FormBorderStyle = FormBorderStyle.None; 
        this.StartPosition = FormStartPosition.CenterParent;
        this.Size = new Size(550, 680);
        this.MouseDown += Form_MouseDown;
    }

    // Borderless dragging capability
    public const int WM_NCLBUTTONDOWN = 0xA1;
    public const int HT_CAPTION = 0x2;
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    private void Form_MouseDown(object? sender, MouseEventArgs e) {
        if (e.Button == MouseButtons.Left) {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
    }

    private void InitializeComponent() {
        var headerPanel = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(20, 20, 25) };
        headerPanel.MouseDown += Form_MouseDown;

        lblTitle.Text = $"Analytics & Charts";
        lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold); 
        lblTitle.ForeColor = primaryColor; 
        lblTitle.AutoSize = true; 
        lblTitle.Location = new Point(20, 15);
        lblTitle.MouseDown += Form_MouseDown;
        
        var btnClose = new Label { Text = "✕", Location = new Point(510, 15), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 14F, FontStyle.Bold), Cursor = Cursors.Hand };
        btnClose.Click += (s, e) => this.Close();
        btnClose.MouseEnter += (s, e) => btnClose.ForeColor = Color.Crimson;
        btnClose.MouseLeave += (s, e) => btnClose.ForeColor = Color.Gray;

        headerPanel.Controls.Add(lblTitle);
        headerPanel.Controls.Add(btnClose);

        // Filter Buttons
        btnWeekly.Text = "This Week";
        btnWeekly.Location = new Point(30, 85); btnWeekly.Size = new Size(130, 40);
        btnWeekly.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnWeekly.FlatStyle = FlatStyle.Flat; btnWeekly.BackColor = panelColor; btnWeekly.ForeColor = textColor;
        btnWeekly.FlatAppearance.BorderSize = 1; btnWeekly.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 70);
        btnWeekly.Click += (s, e) => LoadData("Weekly");

        btnMonthly.Text = "This Month";
        btnMonthly.Location = new Point(175, 85); btnMonthly.Size = new Size(130, 40);
        btnMonthly.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnMonthly.FlatStyle = FlatStyle.Flat; btnMonthly.BackColor = panelColor; btnMonthly.ForeColor = textColor;
        btnMonthly.FlatAppearance.BorderSize = 1; btnMonthly.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 70);
        btnMonthly.Click += (s, e) => LoadData("Monthly");

        btnReset.Text = "Reset Filter";
        btnReset.Location = new Point(320, 85); btnReset.Size = new Size(130, 40);
        btnReset.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnReset.FlatStyle = FlatStyle.Flat; btnReset.BackColor = primaryColor; btnReset.ForeColor = textColor;
        btnReset.FlatAppearance.BorderSize = 0;
        btnReset.Click += (s, e) => LoadData("All");

        // Pie Chart Area
        pieChart.Location = new Point(25, 150);
        pieChart.Size = new Size(500, 500);
        pieChart.Font = new Font("Segoe UI", 11F);
        pieChart.ForeColor = textColor;
        pieChart.BackColor = bgColor; 

        Controls.Add(headerPanel);
        Controls.Add(btnWeekly);
        Controls.Add(btnMonthly);
        Controls.Add(btnReset);
        Controls.Add(pieChart);
    }

    private void LoadData(string mode) {
        var allExpenses = _expenseService.GetExpenses(_user.Id).ToList();
        var today = DateTime.Today;
        
        System.Collections.Generic.IEnumerable<Expense> filtered;

        // Reset button colors
        btnWeekly.BackColor = panelColor;
        btnMonthly.BackColor = panelColor;
        
        if (mode == "Weekly") {
            // Find start of week (assuming Monday as start to be practical, but generic DayOfWeek calculation works too)
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var weekStart = today.AddDays(-1 * diff).Date;
            filtered = allExpenses.Where(e => e.Date.Date >= weekStart && e.Date.Date <= today);
            lblTitle.Text = "Analytics - This Week";
            btnWeekly.BackColor = Color.MediumOrchid; // Highlight active
        } 
        else if (mode == "Monthly") {
            filtered = allExpenses.Where(e => e.Date.Month == today.Month && e.Date.Year == today.Year);
            lblTitle.Text = "Analytics - This Month";
            btnMonthly.BackColor = Color.MediumOrchid; // Highlight active
        } 
        else {
            filtered = allExpenses;
            lblTitle.Text = "Analytics - All Time";
        }

        pieChart.UpdateData(filtered);
    }
}

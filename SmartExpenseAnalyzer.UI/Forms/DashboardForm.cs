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

    private readonly Color bgColor = Color.FromArgb(30, 30, 36); 
    private readonly Color panelColor = Color.FromArgb(43, 43, 54);
    private readonly Color primaryColor = Color.FromArgb(0, 150, 255); 
    private readonly Color textColor = Color.White;

    public DashboardForm(User user, IExpenseService expenseService) {
        _user = user;
        _expenseService = expenseService;
        ApplyDesign();
        InitializeComponent();
        RefreshData();
    }

    private void ApplyDesign() {
        this.BackColor = bgColor;
        this.FormBorderStyle = FormBorderStyle.None; 
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Size = new Size(800, 550); // Reverted size, moved chart to another form
        this.MouseDown += Form_MouseDown;
    }

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
        var mainFont = new Font("Segoe UI", 11F);
        
        var headerPanel = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(20, 20, 25) };
        headerPanel.MouseDown += Form_MouseDown;

        var lblHeader = new Label { Text = $"Dashboard - Welcome {_user.Username}", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = primaryColor, AutoSize = true, Location = new Point(20, 15) };
        lblHeader.MouseDown += Form_MouseDown;
        
        var btnClose = new Label { Text = "✕", Location = new Point(760, 15), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 14F, FontStyle.Bold), Cursor = Cursors.Hand };
        btnClose.Click += (s, e) => this.Close();
        btnClose.MouseEnter += (s, e) => btnClose.ForeColor = Color.Crimson;
        btnClose.MouseLeave += (s, e) => btnClose.ForeColor = Color.Gray;

        headerPanel.Controls.Add(lblHeader);
        headerPanel.Controls.Add(btnClose);

        grid.Location = new Point(30, 80);
        grid.Size = new Size(740, 280);
        grid.ReadOnly = true;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.BackgroundColor = panelColor;
        grid.BorderStyle = BorderStyle.None;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.DefaultCellStyle.BackColor = panelColor;
        grid.DefaultCellStyle.ForeColor = textColor;
        grid.DefaultCellStyle.SelectionBackColor = primaryColor;
        grid.DefaultCellStyle.Font = mainFont;
        grid.RowHeadersVisible = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 20, 25);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = primaryColor;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.ColumnHeadersHeight = 40;

        lblTotal.Location = new Point(30, 380);
        lblTotal.AutoSize = true;
        lblTotal.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTotal.ForeColor = Color.SpringGreen;

        var btnReports = new Button { Text = "View Charts & Reports", Location = new Point(530, 375), Size = new Size(240, 35) };
        btnReports.Font = new Font("Segoe UI", 11F, FontStyle.Bold); btnReports.Cursor = Cursors.Hand;
        btnReports.FlatStyle = FlatStyle.Flat; btnReports.FlatAppearance.BorderSize = 0;
        btnReports.BackColor = Color.MediumOrchid; btnReports.ForeColor = Color.White;
        btnReports.Click += (s, e) => new ReportsForm(_user, _expenseService).ShowDialog();

        var inputPanel = new Panel { Location = new Point(30, 420), Size = new Size(740, 100), BackColor = panelColor };
        
        var lblTitle = new Label { Text = "Title", ForeColor = Color.LightGray, Font = mainFont, Location = new Point(15, 10), AutoSize = true };
        txtTitle.Location = new Point(15, 35); txtTitle.Size = new Size(180, 40); 
        txtTitle.Font = new Font("Segoe UI", 12F); txtTitle.BackColor = bgColor; txtTitle.ForeColor = textColor; txtTitle.BorderStyle = BorderStyle.FixedSingle;

        var lblAmount = new Label { Text = "Amount", ForeColor = Color.LightGray, Font = mainFont, Location = new Point(210, 10), AutoSize = true };
        txtAmount.Location = new Point(210, 35); txtAmount.Size = new Size(120, 40);
        txtAmount.Font = new Font("Segoe UI", 12F); txtAmount.BackColor = bgColor; txtAmount.ForeColor = textColor; txtAmount.BorderStyle = BorderStyle.FixedSingle;

        var lblDate = new Label { Text = "Date", ForeColor = Color.LightGray, Font = mainFont, Location = new Point(345, 10), AutoSize = true };
        dtpDate.Location = new Point(345, 35); dtpDate.Size = new Size(130, 40);
        dtpDate.Font = new Font("Segoe UI", 12F); dtpDate.Format = DateTimePickerFormat.Short;

        var lblCategory = new Label { Text = "Category", ForeColor = Color.LightGray, Font = mainFont, Location = new Point(490, 10), AutoSize = true };
        txtCategory.Location = new Point(490, 35); txtCategory.Size = new Size(110, 40);
        txtCategory.Font = new Font("Segoe UI", 12F); txtCategory.BackColor = bgColor; txtCategory.ForeColor = textColor; txtCategory.BorderStyle = BorderStyle.FixedSingle;

        btnAdd.Text = "+ ADD";
        btnAdd.Location = new Point(615, 35); btnAdd.Size = new Size(110, 32);
        btnAdd.Font = new Font("Segoe UI", 11F, FontStyle.Bold); btnAdd.Cursor = Cursors.Hand;
        btnAdd.FlatStyle = FlatStyle.Flat; btnAdd.FlatAppearance.BorderSize = 0;
        btnAdd.BackColor = primaryColor; btnAdd.ForeColor = Color.White;
        btnAdd.Click += BtnAdd_Click;

        inputPanel.Controls.Add(lblTitle); inputPanel.Controls.Add(txtTitle);
        inputPanel.Controls.Add(lblAmount); inputPanel.Controls.Add(txtAmount);
        inputPanel.Controls.Add(lblDate); inputPanel.Controls.Add(dtpDate);
        inputPanel.Controls.Add(lblCategory); inputPanel.Controls.Add(txtCategory);
        inputPanel.Controls.Add(btnAdd);

        Controls.Add(headerPanel);
        Controls.Add(grid);
        Controls.Add(lblTotal);
        Controls.Add(btnReports);
        Controls.Add(inputPanel);
    }

    private void BtnAdd_Click(object? sender, EventArgs e) {
        try {
            if (decimal.TryParse(txtAmount.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal amount)) {
                
                string finalCategory = string.IsNullOrWhiteSpace(txtCategory.Text) ? "Other" : txtCategory.Text;

                _expenseService.AddExpense(new Expense {
                    UserId = _user.Id,
                    Title = txtTitle.Text,
                    Amount = amount,
                    Date = dtpDate.Value,
                    Category = finalCategory
                });
                RefreshData();
                txtTitle.Clear();
                txtAmount.Clear();
                txtCategory.Clear();
            } else {
                MessageBox.Show("Please enter a valid numeric amount (e.g. 50.00).", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        } catch (Exception ex) {
            MessageBox.Show($"An error occurred:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void RefreshData() {
        var expenses = _expenseService.GetExpenses(_user.Id).ToList();
        grid.DataSource = expenses;
        lblTotal.Text = $"Total Expenses: ${expenses.Sum(x => x.Amount):0.00}";
        grid.Columns["UserId"].Visible = false;
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;
using SmartExpenseAnalyzer.Core.Interfaces;

namespace SmartExpenseAnalyzer.UI.Forms;

public class LoginForm : Form {
    private readonly IUserService _userService;
    private readonly IExpenseService _expenseService;

    // UI Elements
    private Label lblTitle = new Label();
    private TextBox txtUsername = new TextBox();
    private TextBox txtPassword = new TextBox();
    private Button btnLogin = new Button();
    private Button btnRegister = new Button();
    private Label lblStatus = new Label();

    // Theming Colors
    private readonly Color bgColor = Color.FromArgb(30, 30, 36); 
    private readonly Color panelColor = Color.FromArgb(43, 43, 54);
    private readonly Color primaryColor = Color.FromArgb(0, 150, 255); 
    private readonly Color textColor = Color.White;

    public LoginForm(IUserService userService, IExpenseService expenseService) {
        _userService = userService;
        _expenseService = expenseService;

        ApplyDesign();
        InitializeComponent();
    }

    private void ApplyDesign() {
        this.BackColor = bgColor;
        this.FormBorderStyle = FormBorderStyle.None; 
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Size = new Size(380, 420);

        this.MouseDown += Form_MouseDown;
    }

    // For dragging borderless form
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
        var mainFont = new Font("Segoe UI", 11F, FontStyle.Regular);
        var titleFont = new Font("Segoe UI", 18F, FontStyle.Bold);

        lblTitle.Text = "Smart Expense Analyzer";
        lblTitle.Font = titleFont;
        lblTitle.ForeColor = primaryColor;
        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(40, 50);
        lblTitle.MouseDown += Form_MouseDown;

        var lblUser = new Label { Text = "Username", ForeColor = textColor, Font = mainFont, Location = new Point(40, 120), AutoSize = true };
        txtUsername.Location = new Point(40, 150);
        txtUsername.Width = 300;
        txtUsername.Font = new Font("Segoe UI", 12F);
        txtUsername.BackColor = panelColor;
        txtUsername.ForeColor = textColor;
        txtUsername.BorderStyle = BorderStyle.FixedSingle;

        var lblPass = new Label { Text = "Password", ForeColor = textColor, Font = mainFont, Location = new Point(40, 200), AutoSize = true };
        txtPassword.Location = new Point(40, 230);
        txtPassword.Width = 300;
        txtPassword.Font = new Font("Segoe UI", 12F);
        txtPassword.PasswordChar = '•';
        txtPassword.BackColor = panelColor;
        txtPassword.ForeColor = textColor;
        txtPassword.BorderStyle = BorderStyle.FixedSingle;

        btnLogin.Text = "LOGIN";
        btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnLogin.Location = new Point(40, 290);
        btnLogin.Size = new Size(140, 45);
        btnLogin.FlatStyle = FlatStyle.Flat;
        btnLogin.FlatAppearance.BorderSize = 0;
        btnLogin.BackColor = primaryColor;
        btnLogin.ForeColor = Color.White;
        btnLogin.Cursor = Cursors.Hand;
        btnLogin.Click += BtnLogin_Click;

        btnRegister.Text = "REGISTER";
        btnRegister.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnRegister.Location = new Point(200, 290);
        btnRegister.Size = new Size(140, 45);
        btnRegister.FlatStyle = FlatStyle.Flat;
        btnRegister.FlatAppearance.BorderSize = 2;
        btnRegister.FlatAppearance.BorderColor = primaryColor;
        btnRegister.BackColor = bgColor;
        btnRegister.ForeColor = primaryColor;
        btnRegister.Cursor = Cursors.Hand;
        btnRegister.Click += BtnRegister_Click;

        var btnClose = new Label { Text = "✕", Location = new Point(345, 15), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 14F, FontStyle.Bold), Cursor = Cursors.Hand };
        btnClose.Click += (s, e) => Application.Exit();
        btnClose.MouseEnter += (s, e) => btnClose.ForeColor = Color.Crimson;
        btnClose.MouseLeave += (s, e) => btnClose.ForeColor = Color.Gray;

        lblStatus.Location = new Point(40, 350);
        lblStatus.Width = 300;
        lblStatus.Font = mainFont;
        lblStatus.TextAlign = ContentAlignment.MiddleCenter;

        Controls.Add(lblTitle);
        Controls.Add(lblUser);
        Controls.Add(txtUsername);
        Controls.Add(lblPass);
        Controls.Add(txtPassword);
        Controls.Add(btnLogin);
        Controls.Add(btnRegister);
        Controls.Add(lblStatus);
        Controls.Add(btnClose);
    }

    private void BtnLogin_Click(object? sender, EventArgs e) {
        var user = _userService.Login(txtUsername.Text, txtPassword.Text);
        if (user != null) {
            var dashboard = new DashboardForm(user, _expenseService);
            dashboard.Show();
            this.Hide();
            dashboard.FormClosed += (s, args) => this.Close();
        } else {
            lblStatus.ForeColor = Color.Coral;
            lblStatus.Text = "Invalid credentials!";
        }
    }

    private void BtnRegister_Click(object? sender, EventArgs e) {
        try {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text)) {
                lblStatus.ForeColor = Color.Coral;
                lblStatus.Text = "Credentials cannot be empty.";
                return;
            }
            _userService.Register(txtUsername.Text, txtPassword.Text);
            lblStatus.ForeColor = Color.SpringGreen;
            lblStatus.Text = "Registered! Now login.";
        } catch (Exception ex) {
            lblStatus.ForeColor = Color.Coral;
            lblStatus.Text = ex.Message;
        }
    }
}

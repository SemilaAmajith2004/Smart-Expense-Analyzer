using System;
using System.Drawing;
using System.Windows.Forms;
using SmartExpenseAnalyzer.Core.Interfaces;

namespace SmartExpenseAnalyzer.UI.Forms;

public class LoginForm : Form {
    private readonly IUserService _userService;
    private readonly IExpenseService _expenseService;

    private TextBox txtUsername = new TextBox();
    private TextBox txtPassword = new TextBox();
    private Button btnLogin = new Button();
    private Button btnRegister = new Button();
    private Label lblStatus = new Label();

    public LoginForm(IUserService userService, IExpenseService expenseService) {
        _userService = userService;
        _expenseService = expenseService;

        InitializeComponent();
    }

    private void InitializeComponent() {
        Text = "Smart Expense Analyzer - Login";
        Size = new Size(320, 250);
        StartPosition = FormStartPosition.CenterScreen;

        var lblUser = new Label { Text = "Username:", Location = new Point(20, 20), AutoSize = true };
        txtUsername.Location = new Point(100, 20);
        txtUsername.Width = 150;

        var lblPass = new Label { Text = "Password:", Location = new Point(20, 60), AutoSize = true };
        txtPassword.Location = new Point(100, 60);
        txtPassword.Width = 150;
        txtPassword.PasswordChar = '*';

        btnLogin.Text = "Login";
        btnLogin.Location = new Point(100, 100);
        btnLogin.Click += BtnLogin_Click;

        btnRegister.Text = "Register";
        btnRegister.Location = new Point(180, 100);
        btnRegister.Click += BtnRegister_Click;

        lblStatus.Location = new Point(20, 140);
        lblStatus.Width = 260;
        lblStatus.ForeColor = Color.Red;

        Controls.Add(lblUser);
        Controls.Add(txtUsername);
        Controls.Add(lblPass);
        Controls.Add(txtPassword);
        Controls.Add(btnLogin);
        Controls.Add(btnRegister);
        Controls.Add(lblStatus);
    }

    private void BtnLogin_Click(object? sender, EventArgs e) {
        var user = _userService.Login(txtUsername.Text, txtPassword.Text);
        if (user != null) {
            var dashboard = new DashboardForm(user, _expenseService);
            dashboard.Show();
            this.Hide();
            dashboard.FormClosed += (s, args) => this.Close();
        } else {
            lblStatus.Text = "Invalid credentials!";
        }
    }

    private void BtnRegister_Click(object? sender, EventArgs e) {
        try {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text)) {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Credentials cannot be empty.";
                return;
            }
            _userService.Register(txtUsername.Text, txtPassword.Text);
            lblStatus.ForeColor = Color.Green;
            lblStatus.Text = "Registered! Now login.";
        } catch (Exception ex) {
            lblStatus.ForeColor = Color.Red;
            lblStatus.Text = ex.Message;
        }
    }
}

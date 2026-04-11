using System;
using System.Globalization;
using System.Security;

private btnLogin_Click(Object sender, EventArgs e)
{
    MessageBox.Show("Registerd Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    LoginForm login = new LoginForm();
    login.Show();
    this.Hide();
}
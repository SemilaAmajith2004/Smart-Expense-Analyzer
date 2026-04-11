private void btnLogin_Click (Object sender, EventArgs e)
{
    //This is a temporary login . There is a no database yet
    if (txtUsername.Text == "admin" && txtPassword.Text == "1234")
    {
        DashboardForm dashboard = new DashboardForm();
        dashboard.Show();
        this.Hide();
    }
    else
    {
        MessageBox.Show("Invalid user name or password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
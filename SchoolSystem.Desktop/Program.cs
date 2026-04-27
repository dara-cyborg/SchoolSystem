using SchoolSystem.Desktop.Forms.Auth;
using SchoolSystem.Desktop.Forms;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        using var loginForm = new frmLogin();
        if (loginForm.ShowDialog() != DialogResult.OK) {
            Application.Exit();
            return;
        }

        Application.Run(new frmMain());
    }
}
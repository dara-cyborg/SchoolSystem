using SchoolSystem.Desktop.Services;

namespace SchoolSystem.Desktop.Forms.Auth {
    public partial class frmLogin : Form
    {
        private readonly TextBox _txtName;
        private readonly TextBox _txtPassword;
        private readonly Button _btnLogin;
        private readonly Label _lblError;

        public frmLogin()
        {
            InitializeComponent();

            _txtName = GetRequiredControl<TextBox>("txtName");
            _txtPassword = GetRequiredControl<TextBox>("txtPassword");
            _btnLogin = GetRequiredControl<Button>("btnLogin");
            _lblError = GetRequiredControl<Label>("lblError");

            _lblError.Visible = false;

            _btnLogin.Click += btnLogin_Click;
            _txtPassword.KeyDown += txtPassword_KeyDown;
            _txtName.TextChanged += Input_TextChanged;
            _txtPassword.TextChanged += Input_TextChanged;
        }

        private async void btnLogin_Click(object? sender, EventArgs e)
        {
            _lblError.Visible = false;
            _btnLogin.Enabled = false;

            try
            {
                await ApiClient.Instance.LoginAsync(_txtName.Text.Trim(), _txtPassword.Text);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (InvalidOperationException ex)
            {
                _lblError.Text = ex.Message;
                _lblError.Visible = true;
                _btnLogin.Enabled = true;
            }
            catch (Exception ex)
            {
                _lblError.Text = ex.Message;
                _lblError.Visible = true;
                _btnLogin.Enabled = true;
            }
        }

        private void txtPassword_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.SuppressKeyPress = true;
            e.Handled = true;
            btnLogin_Click(_btnLogin, EventArgs.Empty);
        }

        private void Input_TextChanged(object? sender, EventArgs e)
        {
            _lblError.Text = string.Empty;
            _lblError.Visible = false;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private TControl GetRequiredControl<TControl>(string controlName) where TControl : Control
        {
            var matches = Controls.Find(controlName, true);
            if (matches.Length == 0 || matches[0] is not TControl control)
            {
                throw new InvalidOperationException($"Required control '{controlName}' was not found.");
            }

            return control;
        }
    }
}

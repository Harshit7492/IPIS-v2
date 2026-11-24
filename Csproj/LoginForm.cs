using DataBaseHandler;
using DataModel;
using Global_Data;
using Helper;
using LogManager;

namespace IPIS
{
    public partial class LoginForm : Form
    {
        Image imgTrain;
         private bool isPasswordVisible = false;
        public LoginForm()
        {
            InitializeComponent();
            this.AcceptButton = btnLogin;
            this.CancelButton = btnexit;
            try
            {
                string bgPath = AppHelper.GetImagePath("background.jpg");
                if (!string.IsNullOrEmpty(bgPath) && System.IO.File.Exists(bgPath))
                {
                    imgTrain = Image.FromFile(bgPath);
                    panel1.BackgroundImage = imgTrain;
                }
                else
                {
                    LoggerService.UserLoginLogger.Warning("LoginForm background image 'background.jpg' not found. Using default style.");
                }
            }
            catch (Exception ex)
            {
                LoggerService.UserLoginLogger.Error(ex, "Error setting LoginForm background image.");
            }

            //txtUsername.Text = "RDSO";
            //txtPassword.Text = "1234";
        }
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            { 

                var result = _dbService.GET_LOGIN( txtUsername.Text.Trim(), txtPassword.Text.Trim());

                if (result.StatusCode == 1)
                {
                    Config.currentlyloggedInid = result.UserId;
                    await GlobalData.SetConfigAsync(result.UserId);
                    LoginInfo user = GlobalData._lstLoginInfo.FirstOrDefault();
                    if (user != null)
                    {
                        _dbService.AddLogInHistory( user.Loginid,user.ACCESS_ID,user.user_Name);
                    }
                    this.DialogResult = DialogResult.OK;
                    LoggerService.UserLoginLogger.Information("Login: user {User} (UserId {UserId}) logged in.", user?.user_Name ?? txtUsername.Text.Trim(), result.UserId);
                    int playlistResult = _dbService.ACTIVE_PLAYLIST_FOR_LOGIN_USER(user.Loginid.ToString(),user.user_Name);
                    this.Close();
                }
                else if (result.StatusCode == 2)
                {
                    LoggerService.UserLoginLogger.Warning("Login refused: account inactive. User {User}.", txtUsername.Text.Trim());
                    MessageBox.Show( "Your account is inactive. Please contact the administrator.",  "Vardaan Electronics Private Limited",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    LoggerService.UserLoginLogger.Warning("Login failed: invalid username or password. User {User}.", txtUsername.Text.Trim());
                    MessageBox.Show(
                        "Invalid username or password!",
                        "Vardaan Electronics Private Limited",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LoggerService.UserLoginLogger.Error(ex, "Database login verification failed.");
            }
        }
        private void btnexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            txtPassword.UseSystemPasswordChar = !isPasswordVisible;
            btnTogglePassword.Text = isPasswordVisible ? "🙈" : "👁";
        }
    }
}

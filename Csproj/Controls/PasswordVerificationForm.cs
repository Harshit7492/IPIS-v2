using DataBaseHandler;
using Global_Data;
using LogManager;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace IPISv2.Controls
{
    public class PasswordVerificationForm : Form
    {
        private TextBox txtPassword;
        private Button btnVerify;
        private Button btnCancel;
        private Label lblTitle;
        private Label lblPassword;
        private string userId = GlobalData.CurrentUserId;

        public PasswordVerificationForm()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            Text = "Password Verification";

            Size = new Size(420, 230);

            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            MaximizeBox = false;
            MinimizeBox = false;

            lblTitle = new Label
            {
                Text = "Verify Password",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(125, 20)
            };

            lblPassword = new Label
            {
                Text = "Password",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Location = new Point(35, 75)
            };

            txtPassword = new TextBox
            {
                Location = new Point(120, 70),
                Width = 230,
                Font = new Font("Segoe UI", 11F),
                UseSystemPasswordChar = true
            };

            btnVerify = new Button
            {
                Text = "Verify",
                Location = new Point(120, 125),
                Size = new Size(100, 35)
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(250, 125),
                Size = new Size(100, 35)
            };

            btnVerify.Click += BtnVerify_Click;

            btnCancel.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            AcceptButton = btnVerify;
            CancelButton = btnCancel;

            Controls.Add(lblTitle);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnVerify);
            Controls.Add(btnCancel);
        }

        private void BtnVerify_Click(object sender, EventArgs e)
        {
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Please enter password.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPassword.Focus();
                return;
            }

            if (VerifyPassword(password))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(
                    "Invalid password.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private bool VerifyPassword(string enteredPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                    return false;

                string query = $@"
            SELECT password
            FROM LOGINUSER
            WHERE Loginid = {userId}
              AND IsActive = 1";

                string storedPassword = _dbService.ExecuteScalar(query);

                if (string.IsNullOrEmpty(storedPassword))
                    return false;

                return storedPassword == enteredPassword;
            }
            catch (Exception ex)
            {
                LoggerService.Logger.Error(ex, "Password verification failed.");
                return false;
            }
        }
    }
}
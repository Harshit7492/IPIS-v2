//namespace IPIS
//{
//    partial class LoginForm
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
//            txtPassword = new TextBox();
//            txtUsername = new TextBox();
//            panel1 = new Panel();
//            btnTogglePassword = new Button();
//            label2 = new Label();
//            btnexit = new Button();
//            btnLogin = new Button();
//            panel1.SuspendLayout();
//            SuspendLayout();
//            // 
//            // txtPassword
//            // 
//            txtPassword.Font = new Font("Cambria", 9.75F);
//            txtPassword.Location = new Point(470, 213);
//            txtPassword.Margin = new Padding(4, 5, 4, 5);
//            txtPassword.Name = "txtPassword";
//            txtPassword.PlaceholderText = "Please Enter Your Password";
//            txtPassword.Size = new Size(281, 30);
//            txtPassword.TabIndex = 4;
//            txtPassword.UseSystemPasswordChar = true;
//            // 
//            // txtUsername
//            // 
//            txtUsername.Font = new Font("Cambria", 9.75F);
//            txtUsername.Location = new Point(470, 155);
//            txtUsername.Margin = new Padding(4, 5, 4, 5);
//            txtUsername.Name = "txtUsername";
//            txtUsername.PlaceholderText = "Please Enter Your Username";
//            txtUsername.Size = new Size(281, 30);
//            txtUsername.TabIndex = 2;
//            // 
//            // panel1
//            // 
//            panel1.BorderStyle = BorderStyle.FixedSingle;
//            panel1.Controls.Add(btnTogglePassword);
//            panel1.Controls.Add(label2);
//            panel1.Controls.Add(btnexit);
//            panel1.Controls.Add(btnLogin);
//            panel1.Controls.Add(txtUsername);
//            panel1.Controls.Add(txtPassword);
//            panel1.Dock = DockStyle.Fill;
//            panel1.Location = new Point(0, 0);
//            panel1.Margin = new Padding(4, 5, 4, 5);
//            panel1.Name = "panel1";
//            panel1.Size = new Size(771, 407);
//            panel1.TabIndex = 3;
//            panel1.Paint += panel1_Paint;
//            // 
//            // btnTogglePassword
//            // 
//            btnTogglePassword.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
//            btnTogglePassword.Location = new Point(708, 213);
//            btnTogglePassword.Margin = new Padding(4, 5, 4, 5);
//            btnTogglePassword.Name = "btnTogglePassword";
//            btnTogglePassword.Size = new Size(43, 30);
//            btnTogglePassword.TabIndex = 5;
//            btnTogglePassword.Text = "👁";
//            btnTogglePassword.UseVisualStyleBackColor = true;
//            btnTogglePassword.Click += btnTogglePassword_Click;
//            // 
//            // label2
//            // 
//            label2.AutoSize = true;
//            label2.BackColor = Color.Transparent;
//            label2.Font = new Font("Cambria", 9.75F);
//            label2.ForeColor = Color.Blue;
//            label2.Location = new Point(431, 374);
//            label2.Margin = new Padding(4, 0, 4, 0);
//            label2.Name = "label2";
//            label2.Size = new Size(320, 23);
//            label2.TabIndex = 15;
//            label2.Text = "Vardaan Electronics Private Limited";
//            // 
//            // btnexit
//            // 
//            btnexit.Font = new Font("Cambria", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
//            btnexit.ForeColor = Color.Black;
//            btnexit.Location = new Point(624, 272);
//            btnexit.Margin = new Padding(4, 5, 4, 5);
//            btnexit.Name = "btnexit";
//            btnexit.Size = new Size(129, 48);
//            btnexit.TabIndex = 13;
//            btnexit.Text = "Exit";
//            btnexit.UseVisualStyleBackColor = true;
//            btnexit.Click += btnexit_Click;
//            // 
//            // btnLogin
//            // 
//            btnLogin.Font = new Font("Cambria", 9.75F);
//            btnLogin.ForeColor = Color.Black;
//            btnLogin.Location = new Point(470, 272);
//            btnLogin.Margin = new Padding(4, 5, 4, 5);
//            btnLogin.Name = "btnLogin";
//            btnLogin.Size = new Size(129, 48);
//            btnLogin.TabIndex = 12;
//            btnLogin.Text = "Login";
//            btnLogin.UseVisualStyleBackColor = true;
//            btnLogin.Click += btnLogin_Click;
//            // 
//            // LoginForm
//            // 
//            AutoScaleDimensions = new SizeF(10F, 25F);
//            AutoScaleMode = AutoScaleMode.Font;
//            BackColor = Color.White;
//            ClientSize = new Size(771, 407);
//            ControlBox = false;
//            Controls.Add(panel1);
//            DoubleBuffered = true;
//            FormBorderStyle = FormBorderStyle.FixedDialog;
//            Icon = (Icon)resources.GetObject("$this.Icon");
//            Margin = new Padding(4, 5, 4, 5);
//            Name = "LoginForm";
//            StartPosition = FormStartPosition.CenterScreen;
//            panel1.ResumeLayout(false);
//            panel1.PerformLayout();
//            ResumeLayout(false);
//        }


//        #endregion
//        private TextBox txtPassword;
//        private TextBox txtUsername;
//        private Panel panel1;
//        private Label label2;
//        private Button btnexit;
//        private Button btnLogin;
//        private Button btnTogglePassword;
//    }
//}


namespace IPIS
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            panel1 = new Panel();
            btnTogglePassword = new Button();
            label2 = new Label();
            btnexit = new Button();
            btnLogin = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Cambria", 9.75F);
            txtPassword.Location = new Point(470, 213);
            txtPassword.Margin = new Padding(4, 5, 4, 5);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Please Enter Your Password";
            txtPassword.Size = new Size(238, 30);
            txtPassword.TabIndex = 4;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Cambria", 9.75F);
            txtUsername.Location = new Point(470, 155);
            txtUsername.Margin = new Padding(4, 5, 4, 5);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Please Enter Your Username";
            txtUsername.Size = new Size(281, 30);
            txtUsername.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(btnTogglePassword);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(btnexit);
            panel1.Controls.Add(btnLogin);
            panel1.Controls.Add(txtUsername);
            panel1.Controls.Add(txtPassword);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(771, 407);
            panel1.TabIndex = 3;
            panel1.Paint += panel1_Paint;
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.Font = new Font("Segoe MDL2 Assets", 11F);
            btnTogglePassword.Location = new Point(712, 211);
            btnTogglePassword.Margin = new Padding(4, 5, 4, 5);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(37, 42);
            btnTogglePassword.TabIndex = 5;
            btnTogglePassword.Text = "";
            btnTogglePassword.UseVisualStyleBackColor = true;
            btnTogglePassword.Click += btnTogglePassword_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Cambria", 9.75F);
            label2.ForeColor = Color.Blue;
            label2.Location = new Point(431, 374);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(320, 23);
            label2.TabIndex = 15;
            label2.Text = "Vardaan Electronics Private Limited";
            // 
            // btnexit
            // 
            btnexit.Font = new Font("Cambria", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnexit.ForeColor = Color.Black;
            btnexit.Location = new Point(614, 272);
            btnexit.Margin = new Padding(4, 5, 4, 5);
            btnexit.Name = "btnexit";
            btnexit.Size = new Size(129, 48);
            btnexit.TabIndex = 13;
            btnexit.Text = "Exit";
            btnexit.UseVisualStyleBackColor = true;
            btnexit.Click += btnexit_Click;
            // 
            // btnLogin
            // 
            btnLogin.Font = new Font("Cambria", 9.75F);
            btnLogin.ForeColor = Color.Black;
            btnLogin.Location = new Point(470, 272);
            btnLogin.Margin = new Padding(4, 5, 4, 5);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(129, 48);
            btnLogin.TabIndex = 12;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(771, 407);
            ControlBox = false;
            Controls.Add(panel1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }


        #endregion
        private TextBox txtPassword;
        private TextBox txtUsername;
        private Panel panel1;
        private Label label2;
        private Button btnexit;
        private Button btnLogin;
        private Button btnTogglePassword;
    }
}
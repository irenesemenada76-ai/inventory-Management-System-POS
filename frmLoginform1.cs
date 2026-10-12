
using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace inventory_Management_System_POS {
    public partial class frmLoginform1 : Form {
        private readonly MySQL_Database _db;

        public frmLoginform1() {
            InitializeComponent();

            _db = new MySQL_Database();
        }

        // X BUTTON SA LOGIN FORM
        private void btnexit_Click(object sender, EventArgs e) {
            if (MessageBox.Show(
                "Do you want to exit?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes) {
                Application.Exit();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e) {
        }

        // LOGIN FORM LOAD
        private void frmLoginform1_Load(object sender, EventArgs e) {
            _db.ConnectToDatabase();

            txtUsername.Focus();
        }

        private void label3_Click(object sender, EventArgs e) {
        }

        private void label6_Click(object sender, EventArgs e) {
        }

        private void label8_Click(object sender, EventArgs e) {
        }

        private void label4_Click(object sender, EventArgs e) {
        }

        // SHOW / HIDE PASSWORD
        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            if (checkBox1.Checked) {
                txtPassword.UseSystemPasswordChar = false;
                txtPassword.PasswordChar = '\0';
                checkBox1.Text = "Hide Password";
            } else {
                txtPassword.UseSystemPasswordChar = true;
                checkBox1.Text = "Show Password";
            }
        }

        // LOGIN BUTTON
        private void button1_Click(object sender, EventArgs e) {
            try {
                if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Text)) {
                    MessageBox.Show(
                        "Please enter both username and password.",
                        "Input Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                if (_db.conn.State != ConnectionState.Open) {
                    _db.conn.Open();
                }

                string sql = @"
                    SELECT *
                    FROM tbl_account
                    WHERE user_name = @username
                    AND pass_word = @password";

                using (MySqlCommand cmd =
                    new MySqlCommand(sql, _db.conn)) {
                    cmd.Parameters.AddWithValue(
                        "@username",
                        txtUsername.Text.Trim());

                    cmd.Parameters.AddWithValue(
                        "@password",
                        txtPassword.Text);

                    using (MySqlDataReader dr = cmd.ExecuteReader()) {
                        if (dr.Read()) {
                            MessageBox.Show(
                                "Login successful!",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            this.Hide();

                            FrmMainform frmMain =
                                new FrmMainform(_db);

                            frmMain.ShowDialog();

                            this.Close();
                        } else {
                            MessageBox.Show(
                                "Invalid username or password.",
                                "Login Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // CLEAR BUTTON
        private void button2_Click(object sender, EventArgs e) {
            txtUsername.Clear();
            txtPassword.Clear();

            checkBox1.Checked = false;

            txtUsername.Focus();
        }

        // SIGN UP / CREATE ACCOUNT
        private void linkLabel1_LinkClicked(
            object sender,
            LinkLabelLinkClickedEventArgs e) {
            frmCreateAccount createAccount =
                new frmCreateAccount(_db);

            // Kung ma-close ang Create Account,
            // ipakita balik ang EXISTING Login Form.
            createAccount.FormClosed +=
                (s, args) => {
                    this.Show();
                    txtUsername.Focus();
                };

            this.Hide();

            createAccount.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
        }

        private void txtPassword_TextChanged(object sender, EventArgs e) {
        }

        private void txtPassword_Enter(object sender, EventArgs e) {
        }

        // PRESS ENTER TO LOGIN
        private void txtPassword_KeyDown(
            object sender,
            KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                btnLogin.PerformClick();
            }
        }
    }
}
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace inventory_Management_System_POS {
    public partial class frmCreateAccount : Form {
        public readonly MySQL_Database _db;

        public frmCreateAccount() {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            frmLoginform1 login = new frmLoginform1();
            login.ShowDialog();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {

        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void label6_Click(object sender, EventArgs e) {

        }

        private void frmCreateAccount_Load(object sender, EventArgs e) {

        }

        private void panel1_Paint(object sender, PaintEventArgs e) {

        }

        private void pictureBox1_Click(object sender, EventArgs e) {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e) {
            try {
                this.Hide();
                var _loginForm = new frmLoginform1();
                _loginForm.ShowDialog();
                this.Close();

            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label10_Click(object sender, EventArgs e) {
           
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            if (checkBox1.Checked == true) {
                txtPassword.UseSystemPasswordChar = false;
                checkBox1.Text = "Hide Password";
            }
        }

        private void label11_Click(object sender, EventArgs e) {

        }

        private void button2_Click(object sender, EventArgs e) {
            txtAccID.Clear();
            txtAccName.Clear();
            cbAccountType.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            txtUsername.Clear();
            txtPassword.Clear();
            txtAccID.Focus();
        }

        private void button1_Click(object sender, EventArgs e) {
            try {
                if (txtAccName.Text == "" || cbAccountType.Text == ""
                    || txtUsername.Text == "" || txtPassword.Text == "") {
                    MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                if (_db.conn.State != ConnectionState.Open) {
                    _db.conn.Open();
                }

                string sql = "INSERT INTO tbl_account (account_id, account_name, acc_type,gender, user_name, pass_word) " +
                    "VALUES (NULL, @account_id, @account_name, @acc_type, @gender, @user_name @pass_word)";
                using (MySqlCommand cmd = new MySqlCommand(sql, _db.conn)) {
                    cmd.Parameters.AddWithValue("@account_id", txtAccID.Text);
                    cmd.Parameters.AddWithValue("@account_name", txtAccName.Text);
                    cmd.Parameters.AddWithValue("@acc_type", cbAccountType.Text);
                    cmd.Parameters.AddWithValue("@gender", radioButton1.Checked ? "Male" : "Female");
                    cmd.Parameters.AddWithValue("@user_name", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@pass_word", txtPassword.Text);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnClear.PerformClick();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
}

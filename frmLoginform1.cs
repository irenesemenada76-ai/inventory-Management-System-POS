using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace inventory_Management_System_POS {
    
    public partial class frmLoginform1 : Form {
        private readonly MySQL_Database _db;
        
        public frmLoginform1() {
            InitializeComponent();
            _db = new MySQL_Database();
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to exit?", "Confirm Exit",
               MessageBoxButtons.YesNo, 
               MessageBoxIcon.Question) == DialogResult.Yes) {
               Application.ExitThread();

            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e) {

        }

        private void frmLoginform1_Load(object sender, EventArgs e) {
            _db.ConnectToDatabase();
        }

        private void label3_Click(object sender, EventArgs e) {

        }

        private void label6_Click(object sender, EventArgs e) {

        }

        private void label8_Click(object sender, EventArgs e) {

        }

        private void label4_Click(object sender, EventArgs e) {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            if (checkBox1.Checked == true) {
                txtPassword.UseSystemPasswordChar = false;
                checkBox1.Text = "Hide Password";
            } 
            else 
            {
                txtPassword.UseSystemPasswordChar = true;
                checkBox1.Text = "Show Password";
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            try {
                if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text)) {
                    MessageBox.Show("Please enter both username and password.", "InputError", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (_db.conn.State != ConnectionState.Open) {
                    _db.conn.Open();
                }
                string sql = @"SELECT * FROM tbl_account WHERE user_name = @username AND pass_word = @password";
                using (MySqlCommand cmd = new MySqlCommand(sql, _db.conn)) {
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                    using (MySqlDataReader dr = cmd.ExecuteReader()) {
                        if (dr.Read()) {
                            MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide ();
                            var FrmMain = new FrmMainform();
                            FrmMain.ShowDialog();
                            this.Close();
                        } else {
                            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e) {
            txtUsername.Clear();
            txtPassword.Clear();   
            txtUsername.Focus();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            frmCreateAccount createAccount = new frmCreateAccount();
            createAccount.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e) {

        }
    }
    }


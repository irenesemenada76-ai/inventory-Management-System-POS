using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace inventory_Management_System_POS {
    public partial class frmCreateAccount : Form {
        private readonly MySQL_Database _db;
        private bool isEditMode = false;

        // =========================================================
        // EDIT MODE PROPERTY
        // =========================================================
        public bool IsEditMode {
            get { return isEditMode; }
            set {
                isEditMode = value;

                // Account ID cannot be changed when editing
                txtAccID.ReadOnly = isEditMode;

                if (isEditMode) {
                    label1.Text = "Update Account";
                    btnSave.Text = "Update";
                } else {
                    label1.Text = "Create Account";
                    btnSave.Text = "Save";
                }
            }
        }

        // =========================================================
        // CONSTRUCTOR
        // =========================================================
        public frmCreateAccount(MySQL_Database db) {
            InitializeComponent();

            _db = db ?? throw new ArgumentNullException(nameof(db));

            // CENTER FORM
            this.StartPosition = FormStartPosition.CenterScreen;

            // FIXED / NOT RESIZABLE
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // =====================================================
            // PASSWORD DEFAULT
            // =====================================================
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.PasswordChar = '\0';

            checkBox1.Checked = false;
            checkBox1.Text = "Show Password";

            // =====================================================
            // ACCOUNT TYPE
            // =====================================================
            cbAccountType.DropDownStyle =
                ComboBoxStyle.DropDownList;

            LoadAccountTypes();
        }

        // =========================================================
        // LOAD ACCOUNT TYPE OPTIONS
        // =========================================================
        private void LoadAccountTypes() {
            AddAccountTypeIfMissing("Admin");
            AddAccountTypeIfMissing("User");
            AddAccountTypeIfMissing("Guest");

            cbAccountType.DropDownStyle =
                ComboBoxStyle.DropDownList;
        }

        private void AddAccountTypeIfMissing(string accountType) {
            bool exists = false;

            foreach (object item in cbAccountType.Items) {
                if (item != null &&
                    string.Equals(
                        item.ToString().Trim(),
                        accountType,
                        StringComparison.OrdinalIgnoreCase)) {
                    exists = true;
                    break;
                }
            }

            if (!exists) {
                cbAccountType.Items.Add(accountType);
            }
        }

        // =========================================================
        // SET ACCOUNT TYPE
        // Used when editing an existing account
        // =========================================================
        public void SetAccountType(string accountType) {
            if (string.IsNullOrWhiteSpace(accountType)) {
                cbAccountType.SelectedIndex = -1;
                return;
            }

            string value = accountType.Trim();

            for (int i = 0;
                 i < cbAccountType.Items.Count;
                 i++) {
                if (string.Equals(
                    cbAccountType.Items[i].ToString().Trim(),
                    value,
                    StringComparison.OrdinalIgnoreCase)) {
                    cbAccountType.SelectedIndex = i;
                    return;
                }
            }

            cbAccountType.Items.Add(value);

            cbAccountType.SelectedIndex =
                cbAccountType.Items.Count - 1;
        }

        // =========================================================
        // ALREADY HAVE AN ACCOUNT
        // =========================================================
        private void linkLabel1_LinkClicked(
            object sender,
            LinkLabelLinkClickedEventArgs e) {
            this.Close();
        }

        // =========================================================
        // X BUTTON
        // =========================================================
        private void pictureBox1_Click_1(
            object sender,
            EventArgs e) {
            this.Close();
        }

        private void textBox1_TextChanged(
            object sender,
            EventArgs e) {
        }

        private void label2_Click(
            object sender,
            EventArgs e) {
        }

        private void label6_Click(
            object sender,
            EventArgs e) {
        }

        // =========================================================
        // FORM LOAD
        // =========================================================
        private void frmCreateAccount_Load(
            object sender,
            EventArgs e) {
            this.StartPosition =
                FormStartPosition.CenterScreen;

            LoadAccountTypes();

            // PASSWORD HIDDEN BY DEFAULT
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.PasswordChar = '\0';

            checkBox1.Checked = false;
            checkBox1.Text = "Show Password";

            cbAccountType.DropDownStyle =
                ComboBoxStyle.DropDownList;

            // IMPORTANT:
            // If this form is NOT in edit mode,
            // Account ID can be edited.
            txtAccID.ReadOnly = isEditMode;
        }

        private void panel1_Paint(
            object sender,
            PaintEventArgs e) {
        }

        private void pictureBox1_Click(
            object sender,
            EventArgs e) {
        }

        // =========================================================
        // SHOW / HIDE PASSWORD
        // =========================================================
        private void checkBox1_CheckedChanged(
            object sender,
            EventArgs e) {
            if (checkBox1.Checked) {
                txtPassword.UseSystemPasswordChar = false;
                txtPassword.PasswordChar = '\0';

                checkBox1.Text = "Hide Password";
            } else {
                txtPassword.UseSystemPasswordChar = true;
                txtPassword.PasswordChar = '\0';

                checkBox1.Text = "Show Password";
            }
        }

        private void label11_Click(
            object sender,
            EventArgs e) {
        }

        // =========================================================
        // CLEAR BUTTON
        // =========================================================
        private void button2_Click(
            object sender,
            EventArgs e) {
            // =====================================================
            // SAVE ACCOUNT ID FIRST
            // Account ID MUST NOT be cleared
            // =====================================================
            string savedAccountID = txtAccID.Text;

            // =====================================================
            // CLEAR ACCOUNT NAME
            // =====================================================
            txtAccName.Clear();

            // =====================================================
            // RESET ACCOUNT TYPE
            // =====================================================
            cbAccountType.SelectedIndex = -1;

            // =====================================================
            // RESET GENDER
            // =====================================================
            radioButton1.Checked = false;
            radioButton2.Checked = false;

            // =====================================================
            // CLEAR USERNAME
            // =====================================================
            txtUsername.Clear();

            // =====================================================
            // CLEAR PASSWORD
            // =====================================================
            txtPassword.Clear();

            // =====================================================
            // RESET PASSWORD VISIBILITY
            // =====================================================
            checkBox1.Checked = false;

            txtPassword.UseSystemPasswordChar = true;
            txtPassword.PasswordChar = '\0';

            checkBox1.Text = "Show Password";

            // =====================================================
            // RESTORE ACCOUNT ID
            // =====================================================
            txtAccID.Text = savedAccountID;

            // =====================================================
            // IMPORTANT:
            // DO NOT RESET EDIT MODE HERE
            // If editing, Account ID stays ReadOnly
            // and button remains "Update".
            // =====================================================
            if (isEditMode) {
                txtAccID.ReadOnly = true;
                label1.Text = "Update Account";
                btnSave.Text = "Update";

                // Focus Account Name because ID is locked
                txtAccName.Focus();
            } else {
                txtAccID.ReadOnly = false;
                label1.Text = "Create Account";
                btnSave.Text = "Save";

                txtAccID.Focus();
            }
        }

        // =========================================================
        // SAVE / UPDATE BUTTON
        // =========================================================
        private void button1_Click(
            object sender,
            EventArgs e) {
            try {
                // =================================================
                // CHECK REQUIRED FIELDS
                // =================================================
                if (string.IsNullOrWhiteSpace(txtAccID.Text) ||
                    string.IsNullOrWhiteSpace(txtAccName.Text) ||
                    string.IsNullOrWhiteSpace(txtUsername.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Text)) {
                    MessageBox.Show(
                        "Please fill in all required fields.",
                        "Input Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // =================================================
                // GET GENDER
                // =================================================
                string gender = "";

                if (radioButton1.Checked) {
                    gender = "Male";
                } else if (radioButton2.Checked) {
                    gender = "Female";
                } else {
                    MessageBox.Show(
                        "Please select a gender.",
                        "Input Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // =================================================
                // CHECK ACCOUNT TYPE
                // =================================================
                if (cbAccountType.SelectedIndex == -1) {
                    MessageBox.Show(
                        "Please select an account type.",
                        "Input Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    cbAccountType.Focus();
                    return;
                }

                string accountType =
                    cbAccountType.SelectedItem.ToString();

                // =================================================
                // DATABASE
                // =================================================
                using (var conn = _db.GetConnection()) {
                    conn.Open();

                    // =================================================
                    // EDIT / UPDATE MODE
                    // =================================================
                    if (isEditMode) {
                        string sql = @"
                            UPDATE tbl_account
                            SET account_name = @name,
                                acc_type = @type,
                                gender = @gender,
                                user_name = @username,
                                pass_word = @password
                            WHERE account_id = @id";

                        using (MySqlCommand cmd =
                            new MySqlCommand(sql, conn)) {
                            cmd.Parameters.AddWithValue(
                                "@id",
                                txtAccID.Text.Trim());

                            cmd.Parameters.AddWithValue(
                                "@name",
                                txtAccName.Text.Trim());

                            cmd.Parameters.AddWithValue(
                                "@type",
                                accountType);

                            cmd.Parameters.AddWithValue(
                                "@gender",
                                gender);

                            cmd.Parameters.AddWithValue(
                                "@username",
                                txtUsername.Text.Trim());

                            cmd.Parameters.AddWithValue(
                                "@password",
                                txtPassword.Text);

                            int rowsAffected =
                                cmd.ExecuteNonQuery();

                            if (rowsAffected == 0) {
                                MessageBox.Show(
                                    "No account was updated. " +
                                    "Account ID was not found.",
                                    "Update Failed",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                                return;
                            }
                        }

                        MessageBox.Show(
                            "Account successfully updated!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        this.Close();
                    }
                    // =================================================
                    // CREATE / INSERT MODE
                    // =================================================
                    else {
                        string sql = @"
                            INSERT INTO tbl_account
                            (
                                account_id,
                                account_name,
                                acc_type,
                                gender,
                                user_name,
                                pass_word
                            )
                            VALUES
                            (
                                @id,
                                @name,
                                @type,
                                @gender,
                                @username,
                                @password
                            )";

                        using (MySqlCommand cmd =
                            new MySqlCommand(sql, conn)) {
                            cmd.Parameters.AddWithValue(
                                "@id",
                                txtAccID.Text.Trim());

                            cmd.Parameters.AddWithValue(
                                "@name",
                                txtAccName.Text.Trim());

                            cmd.Parameters.AddWithValue(
                                "@type",
                                accountType);

                            cmd.Parameters.AddWithValue(
                                "@gender",
                                gender);

                            cmd.Parameters.AddWithValue(
                                "@username",
                                txtUsername.Text.Trim());

                            cmd.Parameters.AddWithValue(
                                "@password",
                                txtPassword.Text);

                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show(
                            "Account successfully saved!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        this.Close();
                    }
                }
            } catch (MySqlException ex) {
                // =================================================
                // DUPLICATE ACCOUNT ID / DATABASE ERROR
                // =================================================
                MessageBox.Show(
                    ex.Message,
                    "Database Error!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            } catch (Exception ex) {
                MessageBox.Show(
                    ex.Message,
                    "Error!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // ACCOUNT TYPE SELECTION
        // =========================================================
        private void cbAccountType_SelectedIndexChanged(
            object sender,
            EventArgs e) {
            // Selection only.
            // Manual typing is disabled.
        }

        private void txtPassword_TextChanged(
            object sender,
            EventArgs e) {
        }
    }
}
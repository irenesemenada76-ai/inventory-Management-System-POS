using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace inventory_Management_System_POS {
    public partial class FrmAccounts : Form {
        private readonly MySQL_Database _db;

        // =====================================================
        // CONSTRUCTOR
        // =====================================================
        public FrmAccounts(MySQL_Database db) {
            InitializeComponent();

            _db = db ?? throw new ArgumentNullException(nameof(db));

            // =================================================
            // FORM SETTINGS
            // =================================================
            this.AutoSize = false;
            this.AutoScaleMode = AutoScaleMode.None;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;

            // =================================================
            // DATAGRIDVIEW SETTINGS
            // =================================================
            dataGridView1.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;

            dataGridView1.EditMode =
                DataGridViewEditMode.EditProgrammatically;

            // =================================================
            // REMOVE GRAY EMPTY AREA
            // =================================================

            // Same color as the FrmAccounts background
            dataGridView1.BackgroundColor =
                Color.FromArgb(157, 185, 213);

            // Actual account rows remain white
            dataGridView1.DefaultCellStyle.BackColor =
                Color.White;

            dataGridView1.DefaultCellStyle.ForeColor =
                Color.Black;

            dataGridView1.RowsDefaultCellStyle.BackColor =
                Color.White;

            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor =
                Color.White;

            // Header
            dataGridView1.EnableHeadersVisualStyles = false;

            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor =
                Color.White;

            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.Black;

            // =================================================
            // BUTTONS
            // =================================================
            button1.Visible = true;
            button2.Visible = true;

            button1.Enabled = true;
            button2.Enabled = true;

            button1.Anchor =
                AnchorStyles.Bottom | AnchorStyles.Right;

            button2.Anchor =
                AnchorStyles.Bottom | AnchorStyles.Right;

            // =================================================
            // TABLE
            // =================================================
            dataGridView1.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right |
                AnchorStyles.Bottom;
        }

        // =====================================================
        // FORM LOAD
        // =====================================================
        private void FrmAccounts_Load(object sender, EventArgs e) {
            if (this.Parent != null) {
                this.Dock = DockStyle.Fill;
            }

            AdjustLayout();

            RetrieveAccount("");
        }

        // =====================================================
        // ADJUST LAYOUT
        // =====================================================
        private void AdjustLayout() {
            if (this.ClientSize.Width <= 0 ||
                this.ClientSize.Height <= 0) {
                return;
            }

            int leftMargin = 5;
            int rightMargin = 5;
            int bottomMargin = 12;
            int buttonGap = 10;
            int gridGap = 12;

            // =================================================
            // DELETE BUTTON
            // =================================================
            button2.Anchor =
                AnchorStyles.Bottom | AnchorStyles.Right;

            button2.Left =
                this.ClientSize.Width -
                button2.Width -
                rightMargin;

            button2.Top =
                this.ClientSize.Height -
                button2.Height -
                bottomMargin;

            // =================================================
            // EDIT BUTTON
            // =================================================
            button1.Anchor =
                AnchorStyles.Bottom | AnchorStyles.Right;

            button1.Left =
                button2.Left -
                button1.Width -
                buttonGap;

            button1.Top =
                button2.Top;

            // =================================================
            // DATAGRIDVIEW
            // =================================================
            dataGridView1.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right |
                AnchorStyles.Bottom;

            dataGridView1.Left = leftMargin;

            dataGridView1.Width =
                this.ClientSize.Width -
                leftMargin -
                rightMargin;

            int gridHeight =
                button1.Top -
                dataGridView1.Top -
                gridGap;

            if (gridHeight > 50) {
                dataGridView1.Height = gridHeight;
            }

            // Make empty part of table blue
            dataGridView1.BackgroundColor =
                Color.FromArgb(157, 185, 213);
        }

        // =====================================================
        // AUTOMATIC LAYOUT
        // =====================================================
        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);
            AdjustLayout();
        }

        // =====================================================
        // CLOSE
        // =====================================================
        private void pictureBox1_Click(
            object sender,
            EventArgs e) {
            this.Close();
        }

        private void pictureBox1_Click_1(
            object sender,
            EventArgs e) {
            this.Close();
        }

        // =====================================================
        // EMPTY EVENTS
        // =====================================================
        private void label2_Click(
            object sender,
            EventArgs e) {
        }

        private void panel1_Paint(
            object sender,
            PaintEventArgs e) {
        }

        private void lblCountRec_Click(
            object sender,
            EventArgs e) {
        }

        // =====================================================
        // SEARCH ENTER
        // =====================================================
        private void txtsrch_Enter(
            object sender,
            EventArgs e) {
            if (txtsrch.Text == "Enter Name To Change") {
                txtsrch.Text = "";
                txtsrch.ForeColor = Color.Black;
            }
        }

        // =====================================================
        // SEARCH LEAVE
        // =====================================================
        private void txtsrch_Leave(
            object sender,
            EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtsrch.Text)) {
                txtsrch.Text = "Enter Name To Change";
                txtsrch.ForeColor = Color.Gray;

                RetrieveAccount("");
            }
        }

        // =====================================================
        // SEARCH
        // =====================================================
        private void txtsrch_TextChanged(
            object sender,
            EventArgs e) {
            if (txtsrch.Text == "Enter Name To Change") {
                return;
            }

            RetrieveAccount(txtsrch.Text.Trim());
        }

        // =====================================================
        // RETRIEVE ACCOUNTS
        // =====================================================
        public void RetrieveAccount(string search) {
            try {
                using (var conn = _db.GetConnection()) {
                    conn.Open();

                    string sql = @"
                        SELECT account_id,
                               account_name,
                               acc_type,
                               gender,
                               user_name,
                               pass_word,
                               created_at
                        FROM tbl_account
                        WHERE account_id LIKE @search
                           OR account_name LIKE @search
                           OR user_name LIKE @search
                        ORDER BY created_at ASC";

                    using (var cmd = new MySqlCommand(sql, conn)) {
                        cmd.Parameters.AddWithValue(
                            "@search",
                            "%" + search + "%");

                        using (var reader = cmd.ExecuteReader()) {
                            int number = 0;

                            dataGridView1.Rows.Clear();

                            while (reader.Read()) {
                                number++;

                                dataGridView1.Rows.Add(
                                    number,
                                    reader["account_id"].ToString(),
                                    reader["account_name"].ToString(),
                                    reader["acc_type"].ToString().Trim(),
                                    reader["gender"].ToString(),
                                    reader["user_name"].ToString(),
                                    reader["pass_word"].ToString(),
                                    reader["created_at"].ToString()
                                );
                            }

                            dataGridView1.ClearSelection();

                            lblCountRec.Text =
                                "TOTAL RECORD'S FOUND: " + number;
                        }
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(
                    ex.Message,
                    "Error!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =====================================================
        // PASSWORD MASKING
        // =====================================================
        private void dataGridView1_CellFormatting(
            object sender,
            DataGridViewCellFormattingEventArgs e) {
            if (e.ColumnIndex == 6 && e.Value != null) {
                e.Value = new string(
                    '*',
                    e.Value.ToString().Length);
            }
        }

        private void dataGridView1_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e) {
        }

        // =====================================================
        // EDIT BUTTON
        // =====================================================
        private void button1_Click(
            object sender,
            EventArgs e) {
            try {
                if (dataGridView1.SelectedRows.Count == 0) {
                    MessageBox.Show(
                        "Please select an account first.",
                        "Message",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                DataGridViewRow row =
                    dataGridView1.SelectedRows[0];

                string accountId =
                    row.Cells[1].Value?.ToString() ?? "";

                string accountName =
                    row.Cells[2].Value?.ToString() ?? "";

                string accountType =
                    row.Cells[3].Value?.ToString().Trim() ?? "";

                string gender =
                    row.Cells[4].Value?.ToString() ?? "";

                string username =
                    row.Cells[5].Value?.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(accountId)) {
                    MessageBox.Show(
                        "Account ID is missing.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                string password = "";

                using (var conn = _db.GetConnection()) {
                    conn.Open();

                    string sql = @"
                        SELECT pass_word
                        FROM tbl_account
                        WHERE account_id = @id
                        LIMIT 1";

                    using (var cmd = new MySqlCommand(sql, conn)) {
                        cmd.Parameters.AddWithValue(
                            "@id",
                            accountId);

                        object result =
                            cmd.ExecuteScalar();

                        if (result != null &&
                            result != DBNull.Value) {
                            password = result.ToString();
                        }
                    }
                }

                using (frmCreateAccount editForm =
                    new frmCreateAccount(_db)) {
                    editForm.txtAccID.Text =
                        accountId;

                    editForm.txtAccName.Text =
                        accountName;

                    int typeIndex = -1;

                    for (int i = 0;
                         i < editForm.cbAccountType.Items.Count;
                         i++) {
                        string item =
                            editForm.cbAccountType.Items[i]
                            .ToString().Trim();

                        if (string.Equals(
                            item,
                            accountType,
                            StringComparison.OrdinalIgnoreCase)) {
                            typeIndex = i;
                            break;
                        }
                    }

                    if (typeIndex >= 0) {
                        editForm.cbAccountType.SelectedIndex =
                            typeIndex;
                    } else if (!string.IsNullOrWhiteSpace(accountType)) {
                        editForm.cbAccountType.Items.Add(
                            accountType);

                        editForm.cbAccountType.SelectedIndex =
                            editForm.cbAccountType.Items.Count - 1;
                    }

                    if (gender.Equals(
                        "Male",
                        StringComparison.OrdinalIgnoreCase)) {
                        editForm.radioButton1.Checked = true;
                        editForm.radioButton2.Checked = false;
                    } else if (gender.Equals(
                          "Female",
                          StringComparison.OrdinalIgnoreCase)) {
                        editForm.radioButton1.Checked = false;
                        editForm.radioButton2.Checked = true;
                    }

                    editForm.txtUsername.Text =
                        username;

                    editForm.txtPassword.Text =
                        password;

                    editForm.txtPassword.UseSystemPasswordChar =
                        true;

                    editForm.checkBox1.Checked =
                        false;

                    editForm.checkBox1.Text =
                        "Show Password";

                    editForm.label1.Text =
                        "Update Account";

                    editForm.btnSave.Text =
                        "Update";

                    editForm.txtAccID.ReadOnly =
                        true;

                    editForm.IsEditMode =
                        true;

                    editForm.ShowDialog();
                }

                string currentSearch =
                    txtsrch.Text == "Enter Name To Change"
                        ? ""
                        : txtsrch.Text.Trim();

                RetrieveAccount(currentSearch);
            } catch (Exception ex) {
                MessageBox.Show(
                    ex.Message,
                    "Error!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =====================================================
        // DELETE BUTTON
        // =====================================================
        private void button2_Click(
            object sender,
            EventArgs e) {
            try {
                if (dataGridView1.SelectedRows.Count == 0) {
                    MessageBox.Show(
                        "Please select an account first.",
                        "Message",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                DataGridViewRow row =
                    dataGridView1.SelectedRows[0];

                string accountId =
                    row.Cells[1].Value?.ToString() ?? "";

                string accountName =
                    row.Cells[2].Value?.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(accountId)) {
                    MessageBox.Show(
                        "Account ID is missing.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                DialogResult confirm =
                    MessageBox.Show(
                        "Are you sure you want to delete this account?\n\n" +
                        "Account ID: " + accountId +
                        "\nAccount Name: " + accountName,
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) {
                    return;
                }

                using (var conn = _db.GetConnection()) {
                    conn.Open();

                    string sql = @"
                        DELETE FROM tbl_account
                        WHERE account_id = @id";

                    using (var cmd = new MySqlCommand(sql, conn)) {
                        cmd.Parameters.AddWithValue(
                            "@id",
                            accountId);

                        int rowsAffected =
                            cmd.ExecuteNonQuery();

                        if (rowsAffected == 0) {
                            MessageBox.Show(
                                "The account was not found.",
                                "Delete Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                }

                MessageBox.Show(
                    "Account successfully deleted!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                string currentSearch =
                    txtsrch.Text == "Enter Name To Change"
                        ? ""
                        : txtsrch.Text.Trim();

                RetrieveAccount(currentSearch);
            } catch (Exception ex) {
                MessageBox.Show(
                    ex.Message,
                    "Error!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace inventory_Management_System_POS {
    public partial class FrmMainform : Form {
        private readonly MySQL_Database _db;
        public FrmMainform(MySQL_Database db) {
            InitializeComponent();
            _db = db;
        }

        private void label1_Click(object sender, EventArgs e) {
            label2.Text = DateTime.Now.ToLongDateString();
            label1.Text = DateTime.Now.ToLongTimeString();
        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

        }

        private void panel1_Paint(object sender, PaintEventArgs e) {

        }

        private void timer1_Tick(object sender, EventArgs e) {
            label2.Text = DateTime.Now.ToLongTimeString();
            label1.Text = DateTime.Now.ToLongDateString();
        }

        private void toolStripDropDownButton3_Click(object sender, EventArgs e) {

        }

        private void manageAccountsToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e) {
           var frm = new FrmAccounts(_db);
            OpenChildform(frm);
        }

        private void exitAppToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit the application?",
                "Exit Application",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes) {
                Application.Exit();
            }
        }

        private void resetDatabaseToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private List<Form> _openChildform = new List<Form>();

        private void OpenChildform(Form childform) {

            foreach (var frm in _openChildform) {
                frm.Close();
                frm.Dispose();
            }
            _openChildform.Clear();
            childform.TopLevel = false;
            panel2.Controls.Add(childform);
            childform.BringToFront();
            childform.Show();
            _openChildform.Add(childform);
        }



        private void FrmMainform_Load(object sender, EventArgs e) {

        }

        private void panel2_Paint(object sender, PaintEventArgs e) {

        }

        private void logoutToolStripMenuItem1_Click(object sender, EventArgs e) {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e) {

        }

        private void toolStripDropDownButton2_Click(object sender, EventArgs e) {

        }
    }
    }



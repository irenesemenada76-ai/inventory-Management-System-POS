using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace inventory_Management_System_POS {
    public class MySQL_Database {
        public MySqlConnection conn = new MySqlConnection();

        private readonly string _connectionString =
            "server=localhost;port=3306;Database=inventory_system;User id=root;Password=;";

        public MySQL_Database() {
            conn.ConnectionString = _connectionString;
        }

        public void ConnectToDatabase() {
            try {
                using (var testConn = new MySqlConnection(_connectionString)) {
                    testConn.Open();
                    if (testConn.State == System.Data.ConnectionState.Open) {
                        MessageBox.Show("Database connection successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } else {
                        MessageBox.Show("Database connection failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public MySqlConnection GetConnection() {
            return new MySqlConnection(_connectionString);
        }
    }
}
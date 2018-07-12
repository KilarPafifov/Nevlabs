using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nevlabs
{
    public partial class Form1 : Form
    {
        public Form1()   
        {
            InitializeComponent();
            button1.Click += button1_Click;
            openFileDialog1.Filter = "Text files(*.scv)|*.csv|All files(*.*)|*.*";
        }
        
        private void importCSV()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\"
            + @"килар\source\repos\Nevlabs\Nevlabs\Database1.mdf;Integrated Security=True";

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            
            string filename = openFileDialog1.FileName;
            string[] lines = File.ReadAllLines(Path.GetFullPath(filename));
            string[] buf = new string[4];

            foreach (string elem in lines)
            {
                buf = elem.Split('\t');

                SqlCommand insertProfiles = new SqlCommand("INSERT INTO [Profiles] " +
               "(Name, DateOfBirth, Email, PhoneNumber) VALUES (@Name, @DateOfBirth, @Email, @PhoneNumber)"
               , connection);

                insertProfiles.Parameters.AddWithValue("Name", buf[0]);
                insertProfiles.Parameters.AddWithValue("DateOfBirth", buf[1]);
                insertProfiles.Parameters.AddWithValue("Email", buf[2]);
                insertProfiles.Parameters.AddWithValue("PhoneNumber", buf[3]);
                insertProfiles.ExecuteNonQuery();
            }
            connection.Close();
        }

        private void clearTable()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\"
            + @"килар\source\repos\Nevlabs\Nevlabs\Database1.mdf;Integrated Security=True";

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand clearTable = new SqlCommand("DELETE FROM Profiles", connection);
            clearTable.ExecuteNonQuery();
            connection.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            //clearTable();
            importCSV();

            MessageBox.Show("Файл успешно импортирован");
        }
    }
}

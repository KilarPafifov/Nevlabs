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
            List<Profile> list = new List<Profile>();
            
            foreach (string elem in lines)
            {
                buf = elem.Split('\t');
                Profile profile = new Profile(buf[0], buf[1], buf[2], buf[3]);
                list.Add(profile);

            }

            foreach(Profile elem in list)
            {

                SqlCommand insertProfiles = new SqlCommand("INSERT INTO [Profiles] " +
               "(Name, DateOfBirth, Email, PhoneNumber) VALUES (@Name, @DateOfBirth, @Email, @PhoneNumber)"
               , connection);

                insertProfiles.Parameters.AddWithValue("Name", elem.name);
                insertProfiles.Parameters.AddWithValue("DateOfBirth", elem.date);
                insertProfiles.Parameters.AddWithValue("Email", elem.email);
                insertProfiles.Parameters.AddWithValue("PhoneNumber", elem.phone);
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

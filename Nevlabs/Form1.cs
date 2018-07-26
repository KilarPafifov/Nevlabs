using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            button3.Click += button3_Click;
            openFileDialog1.Filter = "Text files(*.scv)|*.csv|All files(*.*)|*.*";
        }

        private void SetProfiles(List<Profiles> list)
        {
            var context = new Database1Entities();

            foreach (Profiles profile in list)
            {
                context.Profiles.Add(profile);
            }        
        }

        private List<Profiles> GetProfiles()
        {
            var Context = new Database1Entities();
            var profiles = Context.Profiles.ToList();
            return profiles;
        }
        private List<string> ExportCSV()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\"
            + @"килар\source\repos\Nevlabs\Nevlabs\Database1.mdf;Integrated Security=True";

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();


            SqlCommand outputProfiles = new SqlCommand(" SELECT * FROM Profiles "
                , connection);
            SqlDataReader reader = outputProfiles.ExecuteReader();

            List<string> list = new List<string>();

            while (reader.Read())
            {
                string[] stringsForReader = new string[reader.FieldCount];
                string stringsForList = "";

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    stringsForReader[i] = reader[i].ToString();
                }

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (i == reader.FieldCount - 1)
                    {
                        stringsForList += stringsForReader[i].Trim();
                    }

                    else
                    {
                        stringsForList += stringsForReader[i].Trim() + '\t';
                    }
                }

                list.Add(stringsForList);
            }

            reader.Close();
            connection.Close();
            
            return list;
        }
        private void ImportCSV()
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

        private void ClearTable()
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

            ClearTable();
            ImportCSV();
            
            
            MessageBox.Show("Файл успешно импортирован");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\"
            + @"килар\source\repos\Nevlabs\Nevlabs\Database1.mdf;Integrated Security=True";

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = "SELECT name, dateOfBirth, email, phoneNumber FROM Profiles";

            if (radioButton1.Checked)
            {
                 query += " order by name";
            }

            else if (radioButton2.Checked)
            {
                query += " order by dateOfBirth";
            }

            SqlCommand outputProfiles = new SqlCommand(query, connection);
            SqlDataReader reader = outputProfiles.ExecuteReader();
            
            string[] parseredRows = new string[reader.FieldCount];
            
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    parseredRows[i] = reader[i].ToString();
                }
                
                dataGridView1.Rows.Add(parseredRows);   
            }    

            reader.Close();
            connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            List<string> list = ExportCSV();
            string filename = openFileDialog1.FileName;
            
            File.WriteAllLines(Path.GetFullPath(filename), list);
            MessageBox.Show("Данные успешно экспортированы в файл CSV ");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Importer importer = new Importer();
            
            Thread thread = new Thread(importer.Importering);
            thread.Start();
           
        }

        private void ExportORM_Click(object sender, EventArgs e)
        {
            string[] parseredStrings = new string[4];
            List<Profiles> list = GetProfiles();
            foreach(Profiles profile in list)
            {
                parseredStrings[0] = profile.Name.ToString();
                parseredStrings[1] = profile.DateOfBirth.ToString();
                parseredStrings[2] = profile.Email.ToString();
                parseredStrings[3] = profile.PhoneNumber.ToString(); 
                dataGridView1.Rows.Add(parseredStrings);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClearTable();
            List < Profiles > list = new List<Profiles>();
            Profiles profile = new Profiles();
            profile.Name = "anton ftrolov j";
            profile.DateOfBirth = "1988";
            profile.Email = "an@maill.ru";
            profile.PhoneNumber = "77777";
            var context = new Database1Entities();
            context.Profiles.Add(profile);
            context.SaveChanges();

        }
    }
}

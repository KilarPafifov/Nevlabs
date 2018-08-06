﻿using System;
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
            openFileDialog1.Filter = "Text files(*.scv)|*.csv|All files(*.*)|*.*";            
        }
        
        
        private void CreateTable()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\"
            + @"килар\source\repos\Nevlabs\Nevlabs\bin\Debug\Database1.mdf;Integrated Security=True";

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand createTable = new SqlCommand("CREATE TABLE Profiles(" +
                "Name char(100), DateOfBirth char(100), Email char(100), PhoneNumber char(100))",
                connection);

            createTable.ExecuteNonQuery();
            connection.Close();
            
        }
        private void DropTable()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\"
            + @"килар\source\repos\Nevlabs\Nevlabs\bin\Debug\Database1.mdf;Integrated Security=True";

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand dropTable = new SqlCommand("DROP TABLE Profiles", connection);
            dropTable.ExecuteNonQuery();
            connection.Close();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Worker worker = new Worker();
            worker.ClearTable();
            MessageBox.Show("БД успешно очищена");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] parseredStrings = new string[4];
            Worker worker = new Worker();
            List<People> list = worker.GetPeople();
            foreach (People people in list)
            {
                parseredStrings[0] = people.Name.ToString();
                parseredStrings[1] = people.DateOfBirth.ToString();
                parseredStrings[2] = people.Email.ToString();
                parseredStrings[3] = people.PhoneNumber.ToString();
                dataGridView1.Rows.Add(parseredStrings);
            }
        }

        private void ExportORM_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string fileName = openFileDialog1.FileName;
            Worker worker = new Worker();
            List<People> list = worker.GetPeople();
            List<string> list2 = new List<string>();

            foreach (People people in list)
            {
                list2.Add(people.Name.Trim() + '\t' + people.DateOfBirth.Trim() + '\t' + 
                    people.Email.Trim() + '\t' + people.PhoneNumber.Trim());
            }
           
            File.WriteAllLines(Path.GetFullPath(fileName), list2);
            MessageBox.Show("Данные успешно экспортированы в файл CSV");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string filename = openFileDialog1.FileName;

            Worker worker = new Worker();
            worker.fileName = filename;

            Thread thread = new Thread(worker.Importering);
            thread.Start();

            MessageBox.Show("Данные успешно импортированы");
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nevlabs
{
    public class Importer
    {
        public string fileName { get; set; }

        public void Importering()
        {
            fileName = @"C:\Users\килар\Desktop\Прочее\анкеты.csv";
            string s = "sidorov gleb yanovic" + '\t' + "1999" + '\t' + "iv@mail.ru"
                + '\t' + "8888767";

            List<string> list = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(3);
                list.Add(Convert.ToString(i) + s);
            }
            
            File.WriteAllLines(Path.GetFullPath(fileName), list);
        }
    }
}

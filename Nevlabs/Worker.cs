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
    public class Worker
    {
        public string fileName { get; set; }
        private forNevlabsEntities context = new forNevlabsEntities();
        private void AddPeople(List<People> list)
        {
            foreach (People profile in list)
            {
                context.People.Add(profile);
                context.SaveChanges();
            }
        }
        public List<People> GetPeople()
        {
            var people = context.Database
                .SqlQuery<People>
                ("SELECT * FROM People ORDER BY Name, DateOfBirth").ToList();
            return people;
        }
        public void ClearTable()
        {
            context.People.RemoveRange(context.People);
            context.SaveChanges();
        }
        public void Importering()
        {
            string[] lines = File.ReadAllLines(Path.GetFullPath(fileName));
            string[] buf = new string[4];
            List<People> list = new List<People>();

            foreach (string elem in lines)
            {
                buf = elem.Split('\t');
                People people = new People();
                people.Name = buf[0];
                people.DateOfBirth = buf[1];
                people.Email = buf[2];
                people.PhoneNumber = buf[3];
                list.Add(people);
            }

            AddPeople(list);
        }
        public void Exportering()
        {

            List<People> list = GetPeople();
            List<string> list2 = new List<string>();

            foreach (People people in list)
            {
                list2.Add(people.Name.Trim() + '\t' + people.DateOfBirth.Trim() + '\t' +
                    people.Email.Trim() + '\t' + people.PhoneNumber.Trim());
            }

            File.WriteAllLines(Path.GetFullPath(fileName), list2);
        }

    }
}

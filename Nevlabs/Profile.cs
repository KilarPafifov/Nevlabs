using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nevlabs
{
    public class Profile
    {
        public string name { get; }
        public string date { get; }
        public string email { get; }
        public string phone { get; }


        public Profile(string name, string date, string email, string phone)
        {
            this.name = name;
            this.date = date;
            this.email = email;
            this.phone = phone;
        }

    }
}

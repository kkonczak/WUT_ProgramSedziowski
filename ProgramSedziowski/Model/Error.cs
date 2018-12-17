using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramSedziowski.Model
{
    public class Error
    {
        public string Name { get;}
        public string ApplicationName { get;}
        public DateTime Date { get; }

        public Error(string name, string applicationName)
        {
            Date = DateTime.Now;
            this.Name = name;
            this.ApplicationName = applicationName;
        }
    }
}

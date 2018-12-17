using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramSedziowski.Exceptions
{
    public class InvalidAnswerException : Exception
    {
        public InvalidAnswerException(string getted, string excepted)
            : base($"Oczekiwano \"{excepted}\", lecz dostano \"{getted}\"")
        {
        }

        public InvalidAnswerException(string message)
            : base(message)
        {

        }
    }
}

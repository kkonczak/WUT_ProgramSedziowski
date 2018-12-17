using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramSedziowski.Exceptions
{
    public class InvalidMoveException : InvalidAnswerException
    {
        public InvalidMoveException(Model.Point[] points)
            : base("{" + string.Join(";", points.Select(p => $"{{{p.x};{p.y}}}")) + "}"
                  , $"dwóch punktów pionowo bądź dwóch punktów poziomo")
        {
        }

        public InvalidMoveException(string message)
            : base(message)
        {

        }
    }
}

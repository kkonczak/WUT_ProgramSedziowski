using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramSedziowski.Exceptions
{
    public class PointOutBoardException : InvalidAnswerException
    {
        public PointOutBoardException(Model.Point[] points, int size)
            : base("{" + string.Join(";", points.Select(p => $"{{{p.x};{p.y}}}")) + "}"
                  , $"x>=0 i x<{size} i y>=0 i y<{size}")
        {
        }
    }
}

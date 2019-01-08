using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramSedziowski.Model
{
    public class RegisteredMove
    {
        public Point point1;
        public Point point2;
        public int gamerId;
        public DateTime time;

        public RegisteredMove() { }

        public RegisteredMove(Point point1, Point point2, int gamerId)
        {
            this.point1 = point1;
            this.point2 = point2;
            this.gamerId = gamerId;
            this.time = DateTime.Now;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramSedziowski.Model
{
    public class RegisteredMove
    {
        public Point point;
        public int gamerId;
        public DateTime time;

        public RegisteredMove() { }

        public RegisteredMove(Point point, int gamerId)
        {
            this.point = point;
            this.gamerId = gamerId;
            this.time = DateTime.Now;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProgramSedziowski.Model
{
    [Serializable]
    public class Game
    {
        public GameApplication gamer1;

        public GameApplication gamer2;

        public int boardSize;

        [XmlArray("MovesArray")]
        public List<RegisteredMove> moves = new List<RegisteredMove>();
    }
}

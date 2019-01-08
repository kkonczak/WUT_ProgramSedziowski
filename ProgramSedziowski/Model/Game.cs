using System;
using System.Collections.Generic;
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

        [XmlArray("StartMovesArray")]
        public List<Point> startPoints = new List<Point>();
    }
}

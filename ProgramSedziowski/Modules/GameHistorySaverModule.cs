using ProgramSedziowski.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProgramSedziowski.Modules
{
    public static class GameHistorySaverModule
    {
        public static void Save(Game game)
        {
            var serializer = new XmlSerializer(typeof(Game));
            if(!(new DirectoryInfo("History")).Exists)
            {
                Directory.CreateDirectory("History");
            }
            using (var serializerTextWriter = new StreamWriter($"History/{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}_{game.gamer1.Author}_{game.gamer2.Author}.gam"))
            {
                serializer.Serialize(serializerTextWriter, game);
            }
        }

        public static Game Open(string path)
        {
            using (var serializerTextReader = new StreamReader(path))
            {
                var serializer = new XmlSerializer(typeof(Game));
                return serializer.Deserialize(serializerTextReader) as Game;
            }
        }
    }
}

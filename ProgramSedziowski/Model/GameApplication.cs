using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramSedziowski.Model
{
    public class GameApplication : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                Notify(nameof(Name));
            }
        }

        public string Author { get; set; }

        public string CommandLine { get; set; }

        public string WorkingDirectory { get; set; }

        public string Path { get; set; }

        private GameResult _result = new GameResult();
        public GameResult Result
        {
            get { return _result; }
            set
            {
                _result = value;
                Notify(nameof(Result));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramSedziowski.Model
{
    [Serializable]
    public class GameResult : INotifyPropertyChanged
    {

        private int _winNum;
        public int WinNum
        {
            get
            {
                return _winNum;
            }
            set
            {
                _winNum = value;
                Notify(nameof(WinNum));
            }
        }

        private int _losNum;
        public int LosNum
        {
            get
            {
                return _losNum;
            }
            set
            {
                _losNum = value;
                Notify(nameof(LosNum));
            }
        }

        private int _disNum;
        public int DisNum
        {
            get
            {
                return _disNum;
            }
            set
            {
                _disNum = value;
                Notify(nameof(DisNum));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

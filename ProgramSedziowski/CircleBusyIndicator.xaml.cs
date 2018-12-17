using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProgramSedziowski
{
    /// <summary>
    /// Interaction logic for CircleBusyIndicator.xaml
    /// </summary>
    public partial class CircleBusyIndicator : UserControl
    {
        public static readonly DependencyProperty LabelTextProperty =
             DependencyProperty.Register(nameof(LabelText),
             typeof(string),
             typeof(CircleBusyIndicator),
             new UIPropertyMetadata("..."));
        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public CircleBusyIndicator()
        {
            InitializeComponent();
        }
    }
}

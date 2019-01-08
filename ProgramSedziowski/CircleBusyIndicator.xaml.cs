using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static readonly DependencyProperty CurrentTaskProgressProperty =
             DependencyProperty.Register(nameof(CurrentTaskProgress),
             typeof(int),
             typeof(CircleBusyIndicator),
             new UIPropertyMetadata(10));
        public string CurrentTaskProgress
        {
            get { return (string)GetValue(CurrentTaskProgressProperty); }
            set { SetValue(CurrentTaskProgressProperty, value); }
        }

        public CircleBusyIndicator()
        {
            InitializeComponent();
        }
    }

    public class PercentToAngleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value * 360 / 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value * 100 / 360;
        }
    }
}

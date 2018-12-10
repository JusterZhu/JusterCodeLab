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

namespace DevChartDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public class TimeAxisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double target = 0;

            double.TryParse(value.ToString(), out target);

            double interFace = 10.0 / 60;
            var initialAMTime = DateTime.Today.AddHours(9).AddMinutes(30);
            var initialPMTime = DateTime.Today.AddHours(13).AddMinutes(0);
            DateTime currentDateTime = DateTime.MinValue;

            if (target <= 20)
            {
                currentDateTime = initialAMTime.AddMinutes(target / interFace);
            }
            else
            {
                currentDateTime = initialPMTime.AddMinutes((target - 20) / interFace);
            }

            if (target == 0) return "09:30";
            if (target == 10) return "10:30";
            if (target == 20) return "11:30/13:00";
            if (target == 30) return "14:00";
            if (target == 40) return "15:00";

            return currentDateTime.ToString("HH:mm");
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

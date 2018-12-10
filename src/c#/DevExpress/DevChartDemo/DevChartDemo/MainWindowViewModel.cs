using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevChartDemo
{
    public class MainWindowViewModel
    {
        ObservableCollection<MainWindowModel> chartCollection;

        public MainWindowViewModel() {
            Init();
        }

        public ObservableCollection<MainWindowModel> ChartCollection
        {
            get
            {
                return chartCollection ?? (chartCollection = new ObservableCollection<MainWindowModel>());
            }

            set
            {
                chartCollection = value;
            }
        }

        private void Init() {
            for (int i = 0; i < 80; i++)
            {
                Random rand = new Random(Guid.NewGuid().GetHashCode());
                double val = rand.NextDouble();
                ChartCollection.Add(new MainWindowModel {
                     Hight = rand.NextDouble(),
                     Open = rand.NextDouble(),
                     Low = rand.NextDouble(),
                     Close = rand.NextDouble(),
                     Price = rand.NextDouble(),
                     Date=i
                });
            }
        }
    }
}

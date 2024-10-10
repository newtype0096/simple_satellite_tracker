using CelesTrakLib;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satellite_tracker.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            CelesTrak.get_stations(out var response);

            Console.WriteLine("");
        }
    }
}

using satellite_tracker.Models;
using satellite_tracker.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satellite_tracker.ViewModels
{
    public class Orbit3dViewModel : PaneViewModel
    {
        public static Orbit3dViewModel Default { get; } = new Orbit3dViewModel();

        public GlobeControl GlobeControl { get; set; }

        private Satellite _selectedSat;
        public Satellite SelectedSat
        {
            get => _selectedSat;
            set
            {
                SetProperty(ref _selectedSat, value);

                UpdateIndicator();
            }
        }

        public Orbit3dViewModel()
            : base("Orbit 3d")
        {

        }

        private void UpdateIndicator()
        {

        }
    }
}

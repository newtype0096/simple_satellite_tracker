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

        public Orbit3dViewModel()
            : base("Orbit 3d")
        {

        }
    }
}

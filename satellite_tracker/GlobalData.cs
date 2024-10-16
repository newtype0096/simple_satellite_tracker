using CelesTrakLib;
using MvvmDialogs;
using SatelliteTrackerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satellite_tracker
{
    public sealed class GlobalData
    {
        public static GlobalData Default { get; } = new GlobalData();

        public void Initialize()
        {
            CurrentDirectory = Directory.GetCurrentDirectory();

            SatelliteTracker = new SatelliteTracker();
            SatelliteTracker.Start();

            CelesTrak.Default.WorkingDirectory = Path.Combine(CurrentDirectory, "celestrak");
        }

        public void Destory()
        {
            SatelliteTracker.Stop();
        }

        public IDialogService DialogService { get; set; }

        public string CurrentDirectory { get; private set; }

        public SatelliteTracker SatelliteTracker { get; private set; }
    }
}

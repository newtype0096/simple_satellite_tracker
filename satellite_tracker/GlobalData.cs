using CelesTrakLib;
using MvvmDialogs;
using satellite_tracker.Utils;
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
            TrackingManager = new TrackingManager();

            CelesTrak.Default.WorkingDirectory = Path.Combine(CurrentDirectory, "celestrak");
        }

        public IDialogService DialogService { get; set; }

        public string CurrentDirectory { get; private set; }

        public TrackingManager TrackingManager { get; private set; }
    }
}

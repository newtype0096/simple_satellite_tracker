using CelesTrakLib;
using MvvmDialogs;
using System.IO;

namespace satellite_tracker
{
    public sealed class GlobalData
    {
        public static GlobalData Default { get; } = new GlobalData();

        public IDialogService DialogService { get; set; }

        public string CurrentDirectory { get; private set; }

        public string DataDirectory { get; private set; }

        public CelesTrakService CelesTrak { get; private set; }

        public void Initialize()
        {
            CurrentDirectory = Directory.GetCurrentDirectory();

            DataDirectory = Path.Combine(CurrentDirectory, "Data");
            Directory.CreateDirectory(DataDirectory);

            CelesTrak = new CelesTrakService(DataDirectory);
            CelesTrak.Start();
        }

        public void Destory()
        {
            CelesTrak.Stop();
        }
    }
}
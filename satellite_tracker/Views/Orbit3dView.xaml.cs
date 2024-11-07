﻿using satellite_tracker.ViewModels;
using System.Windows.Controls;

namespace satellite_tracker.Views
{
    /// <summary>
    /// Orbit3dView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Orbit3dView : UserControl
    {
        public Orbit3dView()
        {
            InitializeComponent();

            DataContext = Orbit3dViewModel.Default;

            Loaded += (sender, e) =>
            {
                Orbit3dViewModel.Default.GlobeControl = GlobeControl;
            };
        }
    }
}
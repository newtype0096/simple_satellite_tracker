﻿using satellite_tracker.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace satellite_tracker.Views
{
    /// <summary>
    /// SatelliteSearchWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SatelliteSearchWindow
    {
        public SatelliteSearchWindow()
        {
            InitializeComponent();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var vm = DataContext as SatelliteSearchWindowViewModel;
                vm?.FilterSatCat();
            }
        }
    }
}

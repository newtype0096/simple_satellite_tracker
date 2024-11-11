using satellite_tracker.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace satellite_tracker.Views
{
    /// <summary>
    /// Orbit3dView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Orbit3dView : UserControl
    {
        private const double CameraDPhi = 0.1;
        private const double CameraDTheta = 0.1;
        private const double CameraDR = 0.1;

        private bool IsMouseDown;
        private Point MouseMovePoint;

        public Orbit3dView()
        {
            InitializeComponent();

            DataContext = Orbit3dViewModel.Default;
        }

        private void MainViewport3D_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                MouseMovePoint = e.GetPosition(MainViewport3D);
                IsMouseDown = true;
            }
        }

        private void MainViewport3D_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsMouseDown && e.LeftButton == MouseButtonState.Pressed)
            {
                Point point = e.GetPosition(MainViewport3D);

                double x = point.X - MouseMovePoint.X;
                double y = point.Y - MouseMovePoint.Y;

                if (Math.Abs(x) > Math.Abs(y))
                {
                    Orbit3dViewModel.Default.CameraTheta += (x / MainViewport3D.ActualWidth * 3);
                }
                else
                {
                    Orbit3dViewModel.Default.CameraPhi += (y / MainViewport3D.ActualHeight * 3);
                    if (y > 0)
                    {
                        if (Orbit3dViewModel.Default.CameraPhi > Math.PI / 2.0) Orbit3dViewModel.Default.CameraPhi = Math.PI / 2.0;
                    }
                    else
                    {
                        if (Orbit3dViewModel.Default.CameraPhi < -Math.PI / 2.0) Orbit3dViewModel.Default.CameraPhi = -Math.PI / 2.0;
                    }
                }

                Orbit3dViewModel.Default.PositionCamera();

                MouseMovePoint = point;
            }
        }

        private void MainViewport3D_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsMouseDown = false;
        }

        private void MainViewport3D_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                Orbit3dViewModel.Default.CameraR -= CameraDR;

                if (Orbit3dViewModel.Default.CameraR < CameraDR)
                {
                    Orbit3dViewModel.Default.CameraR = CameraDR;
                }
            }
            else
            {
                Orbit3dViewModel.Default.CameraR += CameraDR;
            }

            Orbit3dViewModel.Default.PositionCamera();
        }
    }
}
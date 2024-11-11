using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media.Media3D;

namespace satellite_tracker.Models
{
    public class ViewportCamera : ObservableObject
    {
        private Point3D _cameraPosition;
        public Point3D CameraPosition
        {
            get => _cameraPosition;
            set => SetProperty(ref _cameraPosition, value);
        }

        private Vector3D _cameraLookDirection;
        public Vector3D CameraLookDirection
        {
            get => _cameraLookDirection;
            set => SetProperty(ref _cameraLookDirection, value);
        }

        private Vector3D _cameraUpDirection;
        public Vector3D CameraUpDirection
        {
            get => _cameraUpDirection;
            set => SetProperty(ref _cameraUpDirection, value);
        }
    }
}
using satellite_tracker.Models;
using satellite_tracker.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace satellite_tracker.ViewModels
{
    public class Orbit3dViewModel : PaneViewModel
    {
        public static Orbit3dViewModel Default { get; } = new Orbit3dViewModel();

        public double CameraPhi = Math.PI / 6.0;
        public double CameraTheta = Math.PI / 6.0;
        public double CameraR = 3.5;

        private ViewportCamera _viewportCamera = new ViewportCamera();
        public ViewportCamera ViewportCamera
        {
            get => _viewportCamera;
            set => SetProperty(ref _viewportCamera, value);
        }

        public Model3DCollection MainModel3dGroup { get; } = new Model3DCollection();

        public GlobeMesh GlobeMesh { get; } = new GlobeMesh();

        private Satellite _selectedSat;
        public Satellite SelectedSat
        {
            get => _selectedSat;
            set
            {
                SetProperty(ref _selectedSat, value);
            }
        }

        public Orbit3dViewModel()
            : base("Orbit 3d")
        {
            PositionCamera();

            DefineLights();
            DefineModel();
        }

        private void DefineLights()
        {
            AmbientLight ambient_light = new AmbientLight(Colors.Gray);
            DirectionalLight directional_light = new DirectionalLight(Colors.Gray, new Vector3D(-1.0, -3.0, -2.0));

            MainModel3dGroup.Add(ambient_light);
            MainModel3dGroup.Add(directional_light);
        }

        private void DefineModel()
        {
            MakeSphere(1, 0, 0, 0, 20, 30);
        }

        private void MakeSphere(double radius, double cx, double cy, double cz, int num_phi, int num_theta)
        {
            double dphi = Math.PI / num_phi;
            double dtheta = 2 * Math.PI / num_theta;

            // Remember the first point.
            int pt0 = GlobeMesh.Positions.Count;

            // Make the points.
            double phi1 = Math.PI / 2;
            for (int p = 0; p <= num_phi; p++)
            {
                double r1 = radius * Math.Cos(phi1);
                double y1 = radius * Math.Sin(phi1);

                double theta = 0;
                for (int t = 0; t <= num_theta; t++)
                {
                    GlobeMesh.Positions.Add(new Point3D(
                        cx + r1 * Math.Cos(theta), cy + y1, cz + -r1 * Math.Sin(theta)));
                    GlobeMesh.TextureCoordinates.Add(new Point(
                        (double)t / num_theta, (double)p / num_phi));
                    theta += dtheta;
                }
                phi1 -= dphi;
            }

            // Make the triangles.
            int i1, i2, i3, i4;
            for (int p = 0; p <= num_phi - 1; p++)
            {
                i1 = p * (num_theta + 1);
                i2 = i1 + (num_theta + 1);
                for (int t = 0; t <= num_theta - 1; t++)
                {
                    i3 = i1 + 1;
                    i4 = i2 + 1;
                    GlobeMesh.TriangleIndices.Add(pt0 + i1);
                    GlobeMesh.TriangleIndices.Add(pt0 + i2);
                    GlobeMesh.TriangleIndices.Add(pt0 + i4);

                    GlobeMesh.TriangleIndices.Add(pt0 + i1);
                    GlobeMesh.TriangleIndices.Add(pt0 + i4);
                    GlobeMesh.TriangleIndices.Add(pt0 + i3);
                    i1 += 1;
                    i2 += 1;
                }
            }
        }

        public void PositionCamera()
        {
            double y = CameraR * Math.Sin(CameraPhi);
            double hyp = CameraR * Math.Cos(CameraPhi);
            double x = hyp * Math.Cos(CameraTheta);
            double z = hyp * Math.Sin(CameraTheta);

            ViewportCamera.CameraPosition = new Point3D(x, y, z);
            ViewportCamera.CameraLookDirection = new Vector3D(-x, -y, -z);
            ViewportCamera.CameraUpDirection = new Vector3D(0, 1, 0);
        }
    }
}

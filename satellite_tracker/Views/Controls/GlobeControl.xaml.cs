using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace satellite_tracker.Views.Controls
{
    /// <summary>
    /// GlobeControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GlobeControl : UserControl
    {
        private Model3DGroup MainModel3Dgroup = new Model3DGroup();

        private PerspectiveCamera TheCamera;

        private double CameraPhi = Math.PI / 6.0;
        private double CameraTheta = Math.PI / 6.0;
        private double CameraR = 3.5;

        private const double CameraDPhi = 0.1;
        private const double CameraDTheta = 0.1;
        private const double CameraDR = 0.1;

        private bool IsMouseDown;
        private Point MouseMovePoint;

        public GlobeControl()
        {
            InitializeComponent();
        }

        private void GlobeControl_Loaded(object sender, RoutedEventArgs e)
        {
            TheCamera = new PerspectiveCamera();
            TheCamera.FieldOfView = 60;
            MainViewport3D.Camera = TheCamera;
            PositionCamera();

            DefineLights();
            DefineModel();

            ModelVisual3D model_visual = new ModelVisual3D();
            model_visual.Content = MainModel3Dgroup;

            MainViewport3D.Children.Add(model_visual);
        }

        private void GlobeControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    CameraPhi += CameraDPhi;
                    if (CameraPhi > Math.PI / 2.0) CameraPhi = Math.PI / 2.0;
                    break;

                case Key.Down:
                    CameraPhi -= CameraDPhi;
                    if (CameraPhi < -Math.PI / 2.0) CameraPhi = -Math.PI / 2.0;
                    break;

                case Key.Left:
                    CameraTheta += CameraDTheta;
                    break;

                case Key.Right:
                    CameraTheta -= CameraDTheta;
                    break;

                case Key.Add:
                case Key.OemPlus:
                    CameraR -= CameraDR;
                    if (CameraR < CameraDR) CameraR = CameraDR;
                    break;

                case Key.Subtract:
                case Key.OemMinus:
                    CameraR += CameraDR;
                    break;
            }

            PositionCamera();
        }

        private void DefineLights()
        {
            AmbientLight ambient_light = new AmbientLight(Colors.Gray);
            DirectionalLight directional_light = new DirectionalLight(Colors.Gray, new Vector3D(-1.0, -3.0, -2.0));

            MainModel3Dgroup.Children.Add(ambient_light);
            MainModel3Dgroup.Children.Add(directional_light);
        }

        private void DefineModel()
        {
            Model3DGroup globe_model = new Model3DGroup();
            MainModel3Dgroup.Children.Add(globe_model);

            ImageBrush globe_brush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/land_shallow_topo_2048.jpg")));
            Material globe_material = new DiffuseMaterial(globe_brush);
            MeshGeometry3D globe_mesh = null;
            MakeSphere(globe_model, ref globe_mesh, globe_material, 1, 0, 0, 0, 20, 30);
        }

        private void MakeSphere(Model3DGroup model_group, ref MeshGeometry3D sphere_mesh, Material sphere_material,
            double radius, double cx, double cy, double cz, int num_phi, int num_theta)
        {
            // Make the mesh if we must.
            if (sphere_mesh == null)
            {
                sphere_mesh = new MeshGeometry3D();
                GeometryModel3D new_model = new GeometryModel3D(sphere_mesh, sphere_material);
                model_group.Children.Add(new_model);
            }

            double dphi = Math.PI / num_phi;
            double dtheta = 2 * Math.PI / num_theta;

            // Remember the first point.
            int pt0 = sphere_mesh.Positions.Count;

            // Make the points.
            double phi1 = Math.PI / 2;
            for (int p = 0; p <= num_phi; p++)
            {
                double r1 = radius * Math.Cos(phi1);
                double y1 = radius * Math.Sin(phi1);

                double theta = 0;
                for (int t = 0; t <= num_theta; t++)
                {
                    sphere_mesh.Positions.Add(new Point3D(
                        cx + r1 * Math.Cos(theta), cy + y1, cz + -r1 * Math.Sin(theta)));
                    sphere_mesh.TextureCoordinates.Add(new Point(
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
                    sphere_mesh.TriangleIndices.Add(pt0 + i1);
                    sphere_mesh.TriangleIndices.Add(pt0 + i2);
                    sphere_mesh.TriangleIndices.Add(pt0 + i4);

                    sphere_mesh.TriangleIndices.Add(pt0 + i1);
                    sphere_mesh.TriangleIndices.Add(pt0 + i4);
                    sphere_mesh.TriangleIndices.Add(pt0 + i3);
                    i1 += 1;
                    i2 += 1;
                }
            }
        }

        private void PositionCamera()
        {
            double y = CameraR * Math.Sin(CameraPhi);
            double hyp = CameraR * Math.Cos(CameraPhi);
            double x = hyp * Math.Cos(CameraTheta);
            double z = hyp * Math.Sin(CameraTheta);

            TheCamera.Position = new Point3D(x, y, z);
            TheCamera.LookDirection = new Vector3D(-x, -y, -z);
            TheCamera.UpDirection = new Vector3D(0, 1, 0);
        }

        private void MainViewport3D_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                MouseMovePoint = e.GetPosition(MainViewport3D);
                IsMouseDown = true;
            }
        }

        private void MainViewport3D_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown && e.LeftButton == MouseButtonState.Pressed)
            {
                Point point = e.GetPosition(MainViewport3D);

                double x = point.X - MouseMovePoint.X;
                double y = point.Y - MouseMovePoint.Y;

                if (Math.Abs(x) > Math.Abs(y))
                {
                    CameraTheta += (x / MainViewport3D.ActualWidth * 3);
                }
                else
                {
                    CameraPhi += (y / MainViewport3D.ActualHeight * 3);
                    if (y > 0)
                    {
                        if (CameraPhi > Math.PI / 2.0) CameraPhi = Math.PI / 2.0;
                    }
                    else
                    {
                        if (CameraPhi < -Math.PI / 2.0) CameraPhi = -Math.PI / 2.0;
                    }
                }

                PositionCamera();

                MouseMovePoint = point;
            }
        }

        private void MainViewport3D_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IsMouseDown = false;
        }

        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {
            CameraPhi += CameraDPhi;
            if (CameraPhi > Math.PI / 2.0) CameraPhi = Math.PI / 2.0;

            PositionCamera();
        }

        private void buttonLeft_Click(object sender, RoutedEventArgs e)
        {
            CameraTheta += CameraDTheta;

            PositionCamera();
        }

        private void buttonDown_Click(object sender, RoutedEventArgs e)
        {
            CameraPhi -= CameraDPhi;
            if (CameraPhi < -Math.PI / 2.0) CameraPhi = -Math.PI / 2.0;

            PositionCamera();
        }

        private void buttonRight_Click(object sender, RoutedEventArgs e)
        {
            CameraTheta -= CameraDTheta;

            PositionCamera();
        }

        private void MainViewport3D_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                CameraR -= CameraDR;

                if (CameraR < CameraDR)
                {
                    CameraR = CameraDR;
                }
            }
            else
            {
                CameraR += CameraDR;
            }

            PositionCamera();
        }
    }
}
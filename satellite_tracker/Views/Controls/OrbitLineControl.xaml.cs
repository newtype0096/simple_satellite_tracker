using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace satellite_tracker.Views.Controls
{
    /// <summary>
    /// OrbitLineControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OrbitLineControl : UserControl
    {
        private OrbitLineVisualHost _orbitLineVisualHost;

        public OrbitLineControl()
        {
            InitializeComponent();

            _orbitLineVisualHost = new OrbitLineVisualHost();
            Content = _orbitLineVisualHost;
        }

        public void AddOrbitLine(int x1, int y1, int x2, int y2)
        {
            _orbitLineVisualHost.AddOrbitLine(new Point(x1, y1), new Point(x2, y2));
        }

        public void ClearOrbitLine()
        {
            _orbitLineVisualHost.ClearOrbitLine();
        }

        public void DrawOrbitLine()
        {
            _orbitLineVisualHost.DrawOrbitLine();
        }
    }

    public class OrbitLineVisualHost : FrameworkElement
    {
        private VisualCollection _visuals;
        private DrawingVisual _drawingVisual;
        private List<(Point, Point)> _lines = new List<(Point, Point)>();

        public OrbitLineVisualHost()
        {
            _drawingVisual = new DrawingVisual();

            _visuals = new VisualCollection(this);
            _visuals.Add(_drawingVisual);

            RenderOptions.SetEdgeMode(_drawingVisual, EdgeMode.Unspecified);
            RenderOptions.SetBitmapScalingMode(_drawingVisual, BitmapScalingMode.NearestNeighbor);
        }

        public void AddOrbitLine(Point start, Point end)
        {
            _lines.Add((start, end));
        }

        public void DrawOrbitLine()
        {
            using (DrawingContext dc = _drawingVisual.RenderOpen())
            {
                foreach (var line in _lines)
                {
                    var pen = new Pen(Brushes.Gold, 3)
                    {
                        LineJoin = PenLineJoin.Round,
                        EndLineCap = PenLineCap.Round
                    };

                    dc.DrawLine(pen, line.Item1, line.Item2);
                }
            }
        }

        public void ClearOrbitLine()
        {
            _lines.Clear();

            _drawingVisual.RenderOpen();
        }

        protected override int VisualChildrenCount => _visuals.Count;

        protected override Visual GetVisualChild(int index) => _visuals[index];
    }
}
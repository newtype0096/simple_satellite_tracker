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
    }

    public class OrbitLineVisualHost : FrameworkElement
    {
        private VisualCollection _visuals;

        public OrbitLineVisualHost()
        {
            _visuals = new VisualCollection(this);
        }

        public void AddOrbitLine(Point start, Point end)
        {
            var visual = new DrawingVisual();
            using (DrawingContext dc = visual.RenderOpen())
            {
                var pen = new Pen(Brushes.Gray, 1);
                dc.DrawLine(pen, start, end);
            }
            _visuals.Add(visual);
        }

        public void ClearOrbitLine()
        {
            _visuals.Clear();
        }

        protected override int VisualChildrenCount => _visuals.Count;

        protected override Visual GetVisualChild(int index) => _visuals[index];
    }
}
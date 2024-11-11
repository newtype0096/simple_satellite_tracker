using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace satellite_tracker.Models
{
    public class GlobeMesh : ObservableObject
    {
        private Point3DCollection _positions = new Point3DCollection();
        public Point3DCollection Positions
        {
            get => _positions;
            set => SetProperty(ref _positions, value);
        }

        private PointCollection _textureCoordinates = new PointCollection();
        public PointCollection TextureCoordinates
        {
            get => _textureCoordinates;
            set => SetProperty(ref _textureCoordinates, value);
        }

        private Int32Collection _triangleIndices = new Int32Collection();
        public Int32Collection TriangleIndices
        {
            get => _triangleIndices;
            set => SetProperty(ref _triangleIndices, value);
        }
    }
}

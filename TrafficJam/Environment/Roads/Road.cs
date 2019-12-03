#region

using System.Drawing;
using TrafficJam.Tools;

#endregion

namespace TrafficJam.Environment.Roads
{
    public abstract class Road
    {
        #region Fields

        private readonly Size _tileSize = new Size( 150, 150 );
        private readonly RoadOrientation _orientation = RoadOrientation.North;
        private readonly (int, int) _position;
        private readonly Point _location;
        private Image _texture;

        private Tool _tool;

        #endregion

        #region Properties

        public RoadOrientation Orientation
        {
            get { return _orientation; }
        }

        public (int, int) Position
        {
            get { return _position; }
        }

        public Image Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public Size TileSize
        {
            get { return _tileSize; }
        }

        public Point Location
        {
            get { return _location; }
        }

        public Tool Tool
        {
            get { return _tool; }
            set { _tool = value; }
        }

        #endregion

        #region Constructors

        protected Road( Map map, (int, int) position )
        {
            _position = position;
            _location = new Point( _position.Item1 * 150, _position.Item2 * 150 );

            _texture = (Image) map.TileResourceManager.GetObject( ToString( ) );
        }

        protected Road( Map map, (int, int) position, RoadOrientation orientation ) : this( map, position )
        {
            _orientation = orientation;
        }

        #endregion

        public abstract bool ToolAllowed( object tool );
    }
}
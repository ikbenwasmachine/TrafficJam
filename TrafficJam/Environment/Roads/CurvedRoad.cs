namespace TrafficJam.Environment.Roads
{
    public class CurvedRoad : Road
    {
        public CurvedRoad( Map map, (int, int) position ) : base( map, position )
        {
        }

        public CurvedRoad( Map map, (int, int) position, RoadOrientation orientation ) : base( map, position,
            orientation )
        {
        }

        public override bool ToolAllowed( object tool )
        {
            throw new System.NotImplementedException( );
        }

        public override string ToString( )
        {
            return "CurvedRoad";
        }
    }
}
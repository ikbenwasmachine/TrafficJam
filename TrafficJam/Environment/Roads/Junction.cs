namespace TrafficJam.Environment.Roads
{
    public class Junction : Road
    {
        public Junction( Map map, (int, int) position ) : base( map, position )
        {
        }

        public Junction( Map map, (int, int) position, RoadOrientation orientation ) : base( map, position,
            orientation )
        {
        }

        public override bool ToolAllowed( object tool )
        {
            throw new System.NotImplementedException( );
        }

        public override string ToString( )
        {
            return "Junction";
        }
    }
}
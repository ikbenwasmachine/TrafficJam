namespace TrafficJam.Environment.Roads
{
    public class VerticalRoad : Road
    {
        public VerticalRoad( Map map, (int, int) position ) : base( map, position )
        {
        }

        public override bool ToolAllowed( object tool )
        {
            throw new System.NotImplementedException( );
        }

        public override string ToString( )
        {
            return "VerticalRoad";
        }
    }
}
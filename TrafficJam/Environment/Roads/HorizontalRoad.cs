namespace TrafficJam.Environment.Roads
{
    public class HorizontalRoad : Road
    {
        public HorizontalRoad( Map map, (int, int) position ) : base( map, position )
        {
        }

        public override bool ToolAllowed( object tool )
        {
            throw new System.NotImplementedException( );
        }

        public override string ToString( )
        {
            return "HorizontalRoad";
        }
    }
}
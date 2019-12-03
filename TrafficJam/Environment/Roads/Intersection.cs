namespace TrafficJam.Environment.Roads
{
    public class Intersection : Road
    {
        public Intersection( Map map, (int, int) position ) : base( map, position )
        {
        }

        public override bool ToolAllowed( object tool )
        {
            throw new System.NotImplementedException( );
        }

        public override string ToString( )
        {
            return "Intersection";
        }
    }
}
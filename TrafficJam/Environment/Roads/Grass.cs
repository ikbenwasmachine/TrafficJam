namespace TrafficJam.Environment.Roads
{
    public class Grass : Road
    {
        public Grass( Map map, (int, int) position ) : base( map, position )
        {
        }

        public override bool ToolAllowed( object tool )
        {
            throw new System.NotImplementedException( );
        }

        public override string ToString( )
        {
            return "Grass";
        }
    }
}
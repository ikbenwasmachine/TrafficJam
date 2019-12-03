using System;
using System.Drawing;

namespace TrafficJam.Environment.Roads
{
    public enum RoadOrientation
    {
        North,
        East,
        South,
        West
    }

    public static class EnumExtensions
    {
        public static RotateFlipType GetRotateFlipType( this RoadOrientation source )
        {
            switch ( source )
            {
                case RoadOrientation.North:
                    return RotateFlipType.RotateNoneFlipNone;
                case RoadOrientation.East:
                    return RotateFlipType.Rotate90FlipNone;
                case RoadOrientation.South:
                    return RotateFlipType.Rotate180FlipNone;
                case RoadOrientation.West:
                    return RotateFlipType.Rotate270FlipNone;
                default:
                    throw new ArgumentOutOfRangeException( nameof(source), source, null );
            }
        }
    }
}
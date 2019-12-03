#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using TrafficJam.Environment.Roads;
using TrafficJam.Simulation;

#endregion

namespace TrafficJam.Environment
{
    public sealed class Map : Panel
    {
        #region Fields

        private readonly ResourceManager _resourceManager =
            new ResourceManager( "TrafficJam.Properties.RoadTiles", Assembly.GetExecutingAssembly( ) );

        private const int HorizontalTiles = 8;
        private const int VerticalTiles = 6;
        private const int TileSize = 150;

        private readonly Dictionary<(int, int), Road> _roadTiles = new Dictionary<(int, int), Road>( HorizontalTiles * VerticalTiles );
        private readonly List<Road> _entryPoints = new List<Road>( );
        private readonly List<List<Road>> _routes = new List<List<Road>>( );

        private Road _selectedTile;

        #endregion

        #region Properties

        public ResourceManager TileResourceManager
        {
            get { return _resourceManager; }
        }

        public List<Road> EntryPoints
        {
            get { return _entryPoints; }
        }

        public List<List<Road>> Routes
        {
            get { return _routes; }
        }

        #endregion

        public Map( int mapId )
        {
            Width = HorizontalTiles * TileSize;
            Height = VerticalTiles * TileSize;

            DoubleBuffered = true;

            SelectMap( mapId );

            Button b = new Button( )
            {
                Text = "Start",
                Location = new Point( 10, 10 )
            };
            b.Click += B_Click;
            Controls.Add( b );
        }

        private void B_Click( object sender, EventArgs e )
        {
            SimulationController controller = new SimulationController( this );

            controller.StartSimulation( );
        }


        //TODO Improve efficiency
        protected override void OnMouseMove( MouseEventArgs e )
        {
            Road road = GetTile( e.Location );

            if ( _selectedTile != road )
            {
                Refresh( );

                using ( Graphics graphics = CreateGraphics( ) )
                {
                    Rectangle newRectangle = new Rectangle( road.Location, road.TileSize );
                    graphics.DrawRectangle( new Pen( Color.Red, 2 ), newRectangle );

                    Invalidate( newRectangle );
                }

                _selectedTile = road;
            }

            //Console.WriteLine( $"Mouse: ({e.X}, {e.Y}) Tile: ({road.Position.Item1}, {road.Position.Item2})" );
        }

        private Road GetTile( Point point )
        {
            int x = point.X / 150;
            int y = point.Y / 150;

            return _roadTiles[(x, y)];
        }

        //Algorithm which calculates all possible routes between the EntryPoints
        //Source: https://en.wikipedia.org/wiki/Pathfinding
        private void CalculateRoutes( )
        {
            //The EntryPoints from which we still need to calculate routes
            List<Road> entryPoints = new List<Road>( _entryPoints );

            while ( entryPoints.Count != 0 )
            {
                //We can immediately remove the current EntryPoint
                Road currentEntryPoint = entryPoints[0];
                entryPoints.RemoveAt( 0 );

                foreach ( Road exitPoint in _entryPoints )
                {
                    //Skip the current EntryPoint because we don't want to calculate a route to ourselves
                    if ( exitPoint == currentEntryPoint )
                    {
                        continue;
                    }

                    //Calculate route
                    int counter = 0;
                    List<(Road, int)> queue = new List<(Road, int)>
                    {
                        (exitPoint, counter)
                    };

                    //Continue until we find the EntryPoint
                    while ( !queue.Exists( routePart => routePart.Item1.Position == currentEntryPoint.Position ) )
                    {
                        counter++;
                        List<(Road, int)> newRouteParts = new List<(Road, int)>( );

                        foreach ( (Road, int) routeElement in queue )
                        {
                            List<(Road, int)> adjacentTiles = GetAdjacentTiles( routeElement.Item1, counter );
                            List<(Road, int)> tempList = new List<(Road, int)>( adjacentTiles );

                            foreach ( (Road, int) adjacentTile in tempList )
                            {
                                //Remove any Grass tiles
                                if ( adjacentTile.Item1.GetType( ) == typeof(Grass) )
                                {
                                    adjacentTiles.Remove( adjacentTile );
                                }
                                else
                                {
                                    //Checks for an entry in the route for which the coordinates are the same, but which has a smaller counter
                                    if ( queue.Exists( routePart => routePart.Item1.Position == adjacentTile.Item1.Position && routePart.Item2 <= adjacentTile.Item2 ) )
                                    {
                                        adjacentTiles.Remove( adjacentTile );
                                    }
                                }
                            }

                            //TODO Check if we are adding duplicates, because we don't update our List during execution
                            newRouteParts.AddRange( adjacentTiles );
                        }

                        queue.AddRange( newRouteParts );
                    }

                    //TODO Calculate the actual route between the entry and exit points
                    List<Road> route = new List<Road>
                    {
                        currentEntryPoint
                    };
                    //TODO Select the adjacent RoadTile with the lowest counter, continue until the exit point is reached
                    //Get adjacent cells of EntryPoint
                    List<(Road, int)> temp = GetAdjacentTiles( currentEntryPoint );

                    //TODO Refactor to iterate only once
                    if ( temp.Count != 0 )
                    {
                        int bestOption = temp.Min( option => option.Item2 );

                        //Road result = temp.Select( temp.Min( option => option.Item2 ) );
                    }

                    //Select cell with lowest counter, choose one at random if there are two cells with the same counter
                    //Store the route
                }
            }
        }

        //Gets the adjacent tiles of the input Road
        private List<(Road, int)> GetAdjacentTiles( Road road, int counter = 0 )
        {
            List<(Road, int)> adjacentTiles = new List<(Road, int)>( );
            int x = road.Position.Item1;
            int y = road.Position.Item2;

            if ( y != 0 )
            {
                adjacentTiles.Add( (_roadTiles[(x, y - 1)], counter) );
            }

            if ( x != HorizontalTiles - 1 )
            {
                adjacentTiles.Add( (_roadTiles[(x + 1, y)], counter) );
            }

            if ( y != VerticalTiles - 1 )
            {
                adjacentTiles.Add( (_roadTiles[(x, y + 1)], counter) );
            }

            if ( x != 0 )
            {
                adjacentTiles.Add( (_roadTiles[(x - 1, y)], counter) );
            }

            return adjacentTiles;
        }

        //TODO Refactor map creating, we probably should hard-code less
        private void SelectMap( int mapId )
        {
            switch ( mapId )
            {
                case 1:
                    SelectRoadMap1( _roadTiles );
                    BackgroundImage = DrawRoads( _roadTiles );

                    //Define EntryPoints
                    _entryPoints.Add( _roadTiles[(3, 0)] );
                    _entryPoints.Add( _roadTiles[(0, 1)] );
                    _entryPoints.Add( _roadTiles[(7, 2)] );
                    _entryPoints.Add( _roadTiles[(2, 5)] );
                    _entryPoints.Add( _roadTiles[(7, 5)] );

                    CalculateRoutes( );

                    break;
                case 2:
                    SelectRoadMap2( _roadTiles );
                    BackgroundImage = DrawRoads( _roadTiles );
                    break;
                case 3:
                    SelectRoadMap3( _roadTiles );
                    BackgroundImage = DrawRoads( _roadTiles );
                    break;
                default:
                    throw new ArgumentOutOfRangeException( $"There is no map with id '{mapId}'" );
            }
        }

        #region Maps

        private void SelectRoadMap1( Dictionary<(int, int), Road> dict )
        {
            dict[(0, 0)] = new Grass( this, (0, 0) );
            dict[(1, 0)] = new Grass( this, (1, 0) );
            dict[(2, 0)] = new Grass( this, (2, 0) );
            dict[(3, 0)] = new VerticalRoad( this, (3, 0) );
            dict[(4, 0)] = new Grass( this, (4, 0) );
            dict[(5, 0)] = new Grass( this, (5, 0) );
            dict[(6, 0)] = new Grass( this, (6, 0) );
            dict[(7, 0)] = new Grass( this, (7, 0) );

            dict[(0, 1)] = new HorizontalRoad( this, (0, 1) );
            dict[(1, 1)] = new Junction( this, (1, 1), RoadOrientation.South );
            dict[(2, 1)] = new HorizontalRoad( this, (2, 1) );
            dict[(3, 1)] = new Intersection( this, (3, 1) );
            dict[(4, 1)] = new HorizontalRoad( this, (4, 1) );
            dict[(5, 1)] = new HorizontalRoad( this, (5, 1) );
            dict[(6, 1)] = new CurvedRoad( this, (6, 1), RoadOrientation.East );
            dict[(7, 1)] = new Grass( this, (7, 1) );

            dict[(0, 2)] = new Grass( this, (0, 2) );
            dict[(1, 2)] = new VerticalRoad( this, (1, 2) );
            dict[(2, 2)] = new Grass( this, (2, 2) );
            dict[(3, 2)] = new VerticalRoad( this, (3, 2) );
            dict[(4, 2)] = new Grass( this, (4, 2) );
            dict[(5, 2)] = new Grass( this, (5, 2) );
            dict[(6, 2)] = new Junction( this, (6, 2), RoadOrientation.East );
            dict[(7, 2)] = new HorizontalRoad( this, (7, 2) );

            dict[(0, 3)] = new Grass( this, (0, 3) );
            dict[(1, 3)] = new Junction( this, (1, 3), RoadOrientation.East );
            dict[(2, 3)] = new HorizontalRoad( this, (2, 3) );
            dict[(3, 3)] = new Intersection( this, (3, 3) );
            dict[(4, 3)] = new HorizontalRoad( this, (4, 3) );
            dict[(5, 3)] = new HorizontalRoad( this, (5, 3) );
            dict[(6, 3)] = new Junction( this, (6, 3) );
            dict[(7, 3)] = new CurvedRoad( this, (7, 3), RoadOrientation.East );

            dict[(0, 4)] = new Grass( this, (0, 4) );
            dict[(1, 4)] = new VerticalRoad( this, (1, 4) );
            dict[(2, 4)] = new Grass( this, (2, 4) );
            dict[(3, 4)] = new VerticalRoad( this, (3, 4) );
            dict[(4, 4)] = new CurvedRoad( this, (4, 4) );
            dict[(5, 4)] = new HorizontalRoad( this, (5, 4) );
            dict[(6, 4)] = new HorizontalRoad( this, (6, 4) );
            dict[(7, 4)] = new Junction( this, (7, 4), RoadOrientation.West );

            dict[(0, 5)] = new Grass( this, (0, 5) );
            dict[(1, 5)] = new CurvedRoad( this, (1, 5), RoadOrientation.West );
            dict[(2, 5)] = new Junction( this, (2, 5), RoadOrientation.South );
            dict[(3, 5)] = new Junction( this, (3, 5) );
            dict[(4, 5)] = new CurvedRoad( this, (4, 5), RoadOrientation.South );
            dict[(5, 5)] = new Grass( this, (5, 5) );
            dict[(6, 5)] = new Grass( this, (6, 5) );
            dict[(7, 5)] = new VerticalRoad( this, (7, 5) );
        }

        private void SelectRoadMap2( Dictionary<(int, int), Road> dict )
        {
            throw new NotImplementedException( );
        }

        private void SelectRoadMap3( Dictionary<(int, int), Road> dict )
        {
            throw new NotImplementedException( );
        }

        #endregion

        private Bitmap DrawRoads( Dictionary<(int, int), Road> roadTiles )
        {
            Bitmap roads = new Bitmap( Width, Height );

            using ( Graphics graphics = Graphics.FromImage( roads ) )
            {
                foreach ( KeyValuePair<(int, int), Road> roadTile in roadTiles )
                {
                    roadTile.Value.Texture.RotateFlip( roadTile.Value.Orientation.GetRotateFlipType( ) );
                    graphics.DrawImage( roadTile.Value.Texture, new Point( roadTile.Key.Item1 * TileSize, roadTile.Key.Item2 * TileSize ) );
                }
            }

            return roads;
        }
    }
}
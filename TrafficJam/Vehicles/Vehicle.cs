#region

using System;
using System.Collections.Generic;
using TrafficJam.Environment;
using TrafficJam.Environment.Roads;

#endregion

namespace TrafficJam.Vehicles
{
    public class Vehicle
    {
        private VehicleManager _manager;
        private List<Road> _route = new List<Road>( );

        public float X { get; set; }
        public float Y { get; set; }

        public Vehicle( float y, VehicleManager manager )
        {
            X = 1;
            Y = y;
            _manager = manager;
        }

        //Selects a route from the possible routes calculated by the Map
        private void SelectRoute( )
        {
            Random random = new Random( );
            int routeIndex = random.Next( _manager.Controller.Map.Routes.Count );

            _route = _manager.Controller.Map.Routes[routeIndex];
        }
    }
}
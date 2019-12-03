#region

using System.Collections.Generic;
using System.Drawing;
using TrafficJam.Simulation;

#endregion

namespace TrafficJam.Vehicles
{
    public class VehicleManager
    {
        #region Fields

        private List<Vehicle> _vehicles;
        private readonly SimulationController _controller;

        #endregion

        #region Properties

        public SimulationController Controller
        {
            get { return _controller; }
        }

        #endregion

        public VehicleManager( SimulationController controller )
        {
            _vehicles = new List<Vehicle>( 25 );
            _controller = controller;
        }

        public void AddVehicle( Vehicle vehicle )
        {
            if ( _vehicles.Count == 25 )
            {
                //Create new VehicleManager
            }
            else
            {
                _vehicles.Add( vehicle );
            }
        }

        public void MoveVehicles( )
        {
            using ( Graphics g = _controller.Map.CreateGraphics( ) )
            {
                foreach ( Vehicle vehicle in _vehicles )
                {
                    vehicle.X += 5;
                    g.FillRectangle( Brushes.Red, vehicle.X, vehicle.Y, 10, 5 );
                }
            }
        }
    }
}
#region

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrafficJam.Environment;
using TrafficJam.Environment.Roads;
using TrafficJam.Vehicles;

#endregion

namespace TrafficJam.Simulation
{
    public class SimulationController
    {
        #region Fields

        private readonly Map _map;
        private List<VehicleManager> _vehicleManagers;
        private List<Road> _entryPoints;
        private uint _currentFrame;

        #endregion

        #region Properties

        public Map Map
        {
            get { return _map; }
        }

        #endregion

        public SimulationController( Map map )
        {
            _map = map;

            _vehicleManagers = new List<VehicleManager>( );
            _entryPoints = new List<Road>( );

            _vehicleManagers.Add( AddVehicleManager( ) );
        }

        /*public async Task SimulationLoop( )
        {
            await UpdateVehicles( );
        }*/

        private async void NextFrame( )
        {
            _map.Refresh( );

            await Task.Run( ( ) => UpdateVehicles( ) );
            //await Task.Run( ( ) => GenerateVehicles( ) );

            _currentFrame++;
        }

        public void StartSimulation( )
        {
            for ( int i = 0; i < 10; i++ )
            {
                NextFrame( );
            }

            Console.WriteLine( "Done" );
        }

        private void UpdateVehicles( )
        {
            foreach ( VehicleManager vehicleManager in _vehicleManagers )
            {
                vehicleManager.MoveVehicles( );
            }

            Thread.Sleep( 1000 );
        }

        private void GenerateVehicles( )
        {
        }

        private VehicleManager AddVehicleManager( )
        {
            VehicleManager vehicleManager = new VehicleManager( this );

            for ( int i = 0; i < 25; i++ )
            {
                vehicleManager.AddVehicle( new Vehicle( i * 10, vehicleManager ) );
            }

            return vehicleManager;
        }
    }
}
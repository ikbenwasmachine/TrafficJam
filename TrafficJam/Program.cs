#region

using System.Threading;
using System.Windows.Forms;
using TrafficJam.Forms;

#endregion

namespace TrafficJam
{
    internal class Program
    {
        private static void Main( )
        {
            Thread.CurrentThread.Name = "Main";

            Application.Run( new MainWindow( ) );
        }
    }
}
#region

using System.Windows.Forms;
using TrafficJam.Environment;

#endregion

namespace TrafficJam.Forms
{
    public class MainWindow : Form
    {
        public MainWindow( )
        {
            InitializeComponents( );
        }

        private void InitializeComponents( )
        {
            AutoSize = true;

            Map m = new Map( 1 );
            Controls.Add( m );
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using version_2D;

namespace Version_2D_Visualization
{

   

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer dt = new DispatcherTimer();
        private Version_2D_Logic logic;

        public MainWindow()
        {
            InitializeComponent();
            this.logic = new Version_2D_Logic();
            this.canvas.LogicSetup(this.logic);

            this.dt.Interval = TimeSpan.FromMilliseconds(10);
            this.dt.Tick += Dt_Tick;
            this.dt.Start();
  
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            if (!this.logic.endgame)
            {
                //this.logic.endgame = this.logic.RobotIsHit_CollisionDetection();
                /*this.logic.endgame = */
                this.logic.endgame = this.logic.RobotIsHit_CollisionDetection_withLine();
                this.logic.map.OneTick();
                this.logic.UpdateBulletsToRects();
                this.canvas.InvalidateVisual();
            
            }
            else
            {
                this.dt.Stop();
                MessageBox.Show("Game Over");
            }

            //this.dt.Stop();
        }
    }
}

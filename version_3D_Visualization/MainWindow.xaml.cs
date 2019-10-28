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
using UniversalHelpers.Configurations;

namespace version_3D_Visualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer dt = new DispatcherTimer();
        private Version_3D_Logic logic;

        public MainWindow()
        {
            InitializeComponent();
            this.logic = new Version_3D_Logic();
            //this.Height = logic.window_height;
            //this.Width = logic.window_width;
            this.canvas_xy.LogicSetup(this.logic);
            this.canvas_xz.LogicSetup(this.logic);
            this.canvas_zy.LogicSetup(this.logic);

            this.dt.Interval = TimeSpan.FromMilliseconds(Config.Game_Speed);
            this.dt.Tick += Dt_Tick;
            this.dt.Start();

        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            if (!this.logic.endgame)
            {
                //this.logic.endgame = this.logic.RobotIsHit_CollisionDetection();
                /*this.logic.endgame = */
                if (this.logic.RobotIsHit_CollisionDetection_withLine())
                {
                    this.logic.endgame = this.logic.LoseLife();
                }

                this.logic.OneTick();
                this.logic.UpdateBulletsToRects();
                this.canvas_xy.InvalidateVisual();
                this.canvas_xz.InvalidateVisual();
                this.canvas_zy.InvalidateVisual();

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

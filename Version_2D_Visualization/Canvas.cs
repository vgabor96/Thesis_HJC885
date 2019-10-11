using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using version_2D;

namespace Version_2D_Visualization
{
    class Canvas : FrameworkElement
    {
        private Version_2D_Logic logic;


        public void LogicSetup(Version_2D_Logic logic)
        {
            this.logic = logic;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            

            if (this.logic != null)
            {
                Bullet item;
              drawingContext.DrawRectangle(Brushes.Gray, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

                //foreach (Rect item in logic.bullet_rects)
                //{

                //    drawingContext.DrawRectangle(Brushes.Red, null, item);
                //}

                for (int i = 0; i < logic.map.bullets.Count(); i++)
                {
                    item = logic.map.bullets[i];
                    drawingContext.DrawRectangle(Brushes.Red, null, new Rect(item.Current_Location.X, item.Current_Location.Y, logic.bullet_rects[i].Width, logic.bullet_rects[i].Height)); ;
                }

                drawingContext.DrawRectangle(Brushes.Blue, null, logic.Robot_rect);
              
            }
            base.OnRender(drawingContext);


        }
    }

    
}

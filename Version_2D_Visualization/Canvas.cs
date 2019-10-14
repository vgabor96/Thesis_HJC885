using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using UniversalHelpers.Enumerators;
using version_2D;
using Brushes = System.Windows.Media.Brushes;

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
                

                drawingContext.DrawRectangle(Brushes.Gray, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

                drawingContext.DrawText(new FormattedText("Lives: "+this.logic.life,CultureInfo.CurrentCulture,FlowDirection.LeftToRight,new Typeface("Arial"),20,Brushes.Green),new System.Windows.Point(20,20));
                //foreach (Rect item in logic.bullet_rects)
                //{

                //    drawingContext.DrawRectangle(Brushes.Red, null, item);
                //}

                //for (int i = 0; i < logic.map.bullets.Count(); i++)
                //{
                //    item = logic.map.bullets[i];
                //    drawingContext.DrawRectangle(Brushes.Red, null, new Rect(item.Current_Location.X, item.Current_Location.Y, logic.bullet_rects[i].Width, logic.bullet_rects[i].Height)); ;
                //}

                foreach (Rect item in logic.bullet_rects)
                {
                    
                    drawingContext.DrawRectangle(Brushes.Red, null,item);
                 

                }


                drawingContext.DrawRectangle(Brushes.Blue, null, logic.Robot_rect);

                //Bullet item;

                //drawingContext.DrawRectangle(Brushes.Gray, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

                //for (int i = 0; i < logic.map.mapObjects.GetLength(0); i++)
                //{
                //    for (int j = 0; j < logic.map.mapObjects.GetLength(1); j++)
                //    {
                //        switch (logic.map.mapObjects[i,j])
                //        {
                //            case (double)MapObjectType.Bullet:
                //                drawingContext.DrawRectangle(Brushes.Red, null, new Rect(logic.map.mapObjects[i, j].Current_Location.X, item.Current_Location.Y, logic.bullet_rects[i].Width, logic.bullet_rects[i].Height)); 
                //                break;
                //            case (double)MapObjectType.Robot:
                //                drawingContext.DrawRectangle(Brushes.Blue, null, logic.Robot_rect);
                //                break;
                //            default:
                //                break;

                //        }
                //    }
                //}

            }
            base.OnRender(drawingContext);


        }
    }

    
}

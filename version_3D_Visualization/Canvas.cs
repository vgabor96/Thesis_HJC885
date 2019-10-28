using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using UniversalHelpers.Configurations;
using version_3D;

namespace version_3D_Visualization
{
    class Canvas : FrameworkElement
    {
        private Version_3D_Logic logic;

        public void LogicSetup(Version_3D_Logic logic)
        {
            this.logic = logic;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {


            if (this.logic != null)
            {


                drawingContext.DrawRectangle(Brushes.Gray, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

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

                    drawingContext.DrawRectangle(Brushes.Red, null, item);



                }
                if (Config.Is_Line_Helper_On)
                {
                    for (int i = 0; i < logic.bullets.Count; i++)
                    {
                        Bullet3D b = logic.bullets[i];

                        for (int j = 0; j < b.destination_lines.Length; j++)
                        {
                            drawingContext.DrawLine(new System.Windows.Media.Pen(Brushes.Purple, 2), new System.Windows.Point((int)b.destination_lines[j].X1, (int)b.destination_lines[j].Y1), new System.Windows.Point((int)b.destination_lines[j].X2, (int)b.destination_lines[j].Y2));
                        }

                        for (int j = 0; j < b.next_location_lines.Length; j++)
                        {
                            drawingContext.DrawLine(new System.Windows.Media.Pen(Brushes.Black, 2), new System.Windows.Point((int)b.next_location_lines[j].X1, (int)b.next_location_lines[j].Y1), new System.Windows.Point((int)b.next_location_lines[j].X2, (int)b.next_location_lines[j].Y2));
                        }

                    }
                }

                foreach (Rect item in this.logic.Robot_rects)
                {
                    drawingContext.DrawRectangle(Brushes.Blue, null, item);
                }



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
                drawingContext.DrawText(new FormattedText("Lives: " + this.logic.life, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 20, Brushes.Green), new System.Windows.Point(20, 20));

            }
            base.OnRender(drawingContext);


        }
    }
}

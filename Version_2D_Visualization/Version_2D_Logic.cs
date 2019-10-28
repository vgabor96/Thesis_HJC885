using _2D_version;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UniversalHelpers.Classes2D;
using UniversalHelpers.Configurations;
using version_2D;

namespace Version_2D_Visualization
{
    class Version_2D_Logic
    {
        public Map map;
        public bool endgame;
        public  int life = Config.Default_HitPoints;
        public ObservableCollection<Rect> Robot_rects;
        public ObservableCollection<Rect> bullet_rects;
        public double window_width = Config.Default_Map_size_X;
        public double window_height = Config.Default_Map_size_Y;
        public UltimateVectorLogic uvl;

        public Version_2D_Logic()
        {
            endgame = false;

            //JAVÍTSAD MAJD!!!!!!!EZ CSAK TESZTNEK


            Initialize();
            //JAVÍTSAD MAJD!!!!!!!EZ CSAK TESZTNEK

        
        }

        private void Initialize()
        {
            this.uvl = new UltimateVectorLogic();

            List<Bullet> bs = new List<Bullet>();
            for (int i = 0; i < Config.Default_Bullet_Count; i++)
            {
                Bullet b = new Bullet();
                b.GenerateRandomBullet();
                bs.Add(b);
            }
            bs.Add(new Bullet(new Vector2(1, 1), 0, 0, 1, 50)); //Coordinate Check
            map = new Map(null, bs);
            bullet_rects = new ObservableCollection<Rect>();
            this.Robot_rects = new ObservableCollection<Rect>();
       

            foreach (Bullet item in map.bullets)
            {
                this.bullet_rects.Add(new Rect(item.Current_Location.X, item.Current_Location.Y, item.size, item.size));
            }
            foreach (Rect item in this.map.robot.robotbody)
            {
                this.Robot_rects.Add(item);
            }

            
        }
        //public bool RobotIsHit_CollisionDetection()
        //{

        //    foreach (Rect item in this.bullet_rects)
        //    { 
        //        if (item.IntersectsWith(this.Robot_rect))
        //        {

        //            return true;

        //        } 
        //    }
        //    return false;
        //}
        public bool RobotIsHit_CollisionDetection_withLine()
        {
            My_Coordinates robot_middlepos = new My_Coordinates(this.map.robot.Current_Location.X
                + (int)Math.Round(this.map.robot.Range / 2), this.map.robot.Current_Location.Y
                + (int)Math.Round(this.map.robot.Range / 2));
            foreach (Bullet item in this.map.bullets)
            {

                //if (item.IsHarmful && LineIntersectsRect(new System.Drawing.Point(item.Current_Location.X, item.Current_Location.Y),
                //    new System.Drawing.Point(item.next_location.X, item.next_location.Y), new Rectangle((int)this.Robot_rect.X, (int)this.Robot_rect.Y, (int)this.Robot_rect.Width, (int)this.Robot_rect.Height)))
                //{
                if (item.IsHarmful && uvl.IsRobotHit(this.map.robot, item))
                {

                    item.IsHarmful = false; // FALSE!!!
                    return true;


                }
            }
            return false;
        }

        public void UpdateBulletsToRects()
        {
            for (int i = 0; i < this.map.bullets.Count; i++)
            {
                this.bullet_rects[i] = new Rect(this.map.bullets[i].current_Location.X, this.map.bullets[i].current_Location.Y, this.bullet_rects[i].Width, this.bullet_rects[i].Height);
            }
        }


        //    public bool IsRobotHit(Robot robot, Bullet bullet)
        //    {
        //        for (int i = 0; i < bullet.next_location_lines.Length; i++)
        //        {
        //            if (LineIntersectsRect(new System.Windows.Point((int)bullet.next_location_lines[i].X1, (int)bullet.next_location_lines[i].Y1), new System.Windows.Point((int)bullet.next_location_lines[i].X2, (int)bullet.next_location_lines[i].Y2), robot.robotbody))
        //            {
        //                return true;
        //            }
        //        }
        //        return false;


        //    }
        //    //TODO: Do SMoething
        //    public static bool LineIntersectsRect(System.Windows.Point p1, System.Windows.Point p2, Rect r) 
        //{
        //        int robot_x = (int)r.X;
        //        int robot_y = (int)r.Y;
        //        int robot_width = (int)r.Width;
        //        int robot_height = (int)r.Height;
        //    return LineIntersectsLine(p1, p2, new System.Drawing.Point(robot_x, robot_y), new System.Drawing.Point(robot_x + robot_width, robot_y)) ||
        //           LineIntersectsLine(p1, p2, new System.Drawing.Point(robot_x + robot_width, robot_y), new System.Drawing.Point(robot_x + robot_width, robot_y + robot_height)) ||
        //           LineIntersectsLine(p1, p2, new System.Drawing.Point(robot_x + robot_width, robot_y + robot_height), new System.Drawing.Point(robot_x, robot_y + robot_height)) ||
        //           LineIntersectsLine(p1, p2, new System.Drawing.Point(robot_x, robot_y + robot_height), new System.Drawing.Point(robot_x, robot_y)) ||
        //           (r.Contains(p1) && r.Contains(p2));
        //}

        //private static bool LineIntersectsLine(System.Windows.Point l1p1, System.Windows.Point l1p2, System.Drawing.Point l2p1, System.Drawing.Point l2p2)
        //{
        //    float q = (float)((l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y));
        //    float d = (float)((l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X));

        //    if( d == 0 )
        //    {
        //        return false;
        //    }

        //    float r = q / d;

        //    q = (float)((l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y));
        //    float s = q / d;

        //    if( r < 0 || r > 1 || s < 0 || s > 1 )
        //    {
        //        return false;
        //    }

        //        return true;
        //}

        public bool LoseLife()
        {
            if (this.life >1)
            {
                this.life--;
                return false;
            }
            return true;
        }
    }
}

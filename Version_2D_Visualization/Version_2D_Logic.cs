using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UniversalHelpers.Classes2D;
using version_2D;

namespace Version_2D_Visualization
{
    class Version_2D_Logic
    {
        public Map map;
        public bool endgame;
        public Rect Robot_rect;
        public ObservableCollection<Rect> bullet_rects;
        public ObservableCollection<Rectangle> bullet_lines;

        public Version_2D_Logic()
        {
            endgame = false;

            //JAVÍTSAD MAJD!!!!!!!EZ CSAK TESZTNEK


            Initialize();
            //JAVÍTSAD MAJD!!!!!!!EZ CSAK TESZTNEK

        
        }

        private void Initialize()
        {
            List<Bullet> bs = new List<Bullet>();
            for (int i = 0; i < 6; i++)
            {
                Bullet b = new Bullet();
                b.GenerateRandomBullet();
                bs.Add(b);
            }
            map = new Map(null, bs);
            bullet_rects = new ObservableCollection<Rect>();

            foreach (Bullet item in map.bullets)
            {
                this.bullet_rects.Add(new Rect(item.Current_Location.X, item.Current_Location.Y, item.size, item.size));
            }
            

            this.Robot_rect = new Rect(map.robot.Current_Location.X, map.robot.Current_Location.Y, 10 + map.robot.Range, 10 + map.robot.Range);
        }
        public bool RobotIsHit_CollisionDetection()
        {
            
            foreach (Rect item in this.bullet_rects)
            { 
                if (item.IntersectsWith(this.Robot_rect))
                {
                    return true;
                } 
            }
            return false;
        }
        public bool RobotIsHit_CollisionDetection_withLine()
        {
            My_Coordinates robot_middlepos = new My_Coordinates(this.map.robot.Current_Location.X
                + (int)Math.Round(this.map.robot.Range / 2), this.map.robot.Current_Location.Y
                + (int)Math.Round(this.map.robot.Range / 2));
            foreach (Bullet item in this.map.bullets)
            {

                if (LineIntersectsRect(new System.Drawing.Point(item.Current_Location.X, item.Current_Location.Y),
                    new System.Drawing.Point(item.next_location.X, item.next_location.Y), new Rectangle((int)this.Robot_rect.X, (int)this.Robot_rect.Y, (int)this.Robot_rect.Width, (int)this.Robot_rect.Height)))
                {
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

        
         public static bool LineIntersectsRect(System.Drawing.Point p1, System.Drawing.Point p2, Rectangle r) 
    {
        return LineIntersectsLine(p1, p2, new System.Drawing.Point(r.X, r.Y), new System.Drawing.Point(r.X + r.Width, r.Y)) ||
               LineIntersectsLine(p1, p2, new System.Drawing.Point(r.X + r.Width, r.Y), new System.Drawing.Point(r.X + r.Width, r.Y + r.Height)) ||
               LineIntersectsLine(p1, p2, new System.Drawing.Point(r.X + r.Width, r.Y + r.Height), new System.Drawing.Point(r.X, r.Y + r.Height)) ||
               LineIntersectsLine(p1, p2, new System.Drawing.Point(r.X, r.Y + r.Height), new System.Drawing.Point(r.X, r.Y)) ||
               (r.Contains(p1) && r.Contains(p2));
    }

    private static bool LineIntersectsLine(System.Drawing.Point l1p1, System.Drawing.Point l1p2, System.Drawing.Point l2p1, System.Drawing.Point l2p2)
    {
        float q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
        float d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);

        if( d == 0 )
        {
            return false;
        }

        float r = q / d;

        q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
        float s = q / d;

        if( r < 0 || r > 1 || s < 0 || s > 1 )
        {
            return false;
        }

        return true;
    }


    }
}

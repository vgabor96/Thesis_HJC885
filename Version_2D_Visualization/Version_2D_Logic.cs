using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using version_2D;

namespace Version_2D_Visualization
{
    class Version_2D_Logic
    {
        public Map map;
        public bool endgame;
        public Rect Robot_rect;
        public ObservableCollection<Rect> bullet_rects;

        public Version_2D_Logic()
        {
            endgame = false;

            //JAVÍTSAD MAJD!!!!!!!EZ CSAK TESZTNEK

            List<Bullet> bs = new List<Bullet>();
            for (int i = 0; i < 6; i++)
            {
                Bullet b = new Bullet();
                b.GenerateRandomBullet();
                bs.Add(b);
            }

            //JAVÍTSAD MAJD!!!!!!!EZ CSAK TESZTNEK

            map = new Map(null,bs);
            bullet_rects = new ObservableCollection<Rect>();

            foreach (Bullet item in map.bullets)
            {
                this.bullet_rects.Add(new Rect(item.Current_Location.X, item.Current_Location.Y, item.size, item.size));
            }


            this.Robot_rect = new Rect(map.robot.Current_Location.X, map.robot.Current_Location.Y, 20+map.robot.Range, 20+map.robot.Range);
        
        }

    }
}

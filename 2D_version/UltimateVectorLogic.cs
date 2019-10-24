using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UniversalHelpers.Classes2D;
using version_2D;

namespace _2D_version
{
  public  class UltimateVectorLogic
    {
        Vector2 bullet_vector;
        Robot robot;
        Bullet bullet;

        public UltimateVectorLogic()
        {
            this.bullet_vector = new Vector2();
            this.robot = new Robot();
            this.bullet = new Bullet();

        }

        public bool IsRobotHit(Robot robot, Bullet bullet)
        {
            for (int i = 0; i < bullet.lines.Length; i++)
            {
                if (LineIntersectsRect(new Point((int)bullet.lines[i].X1, (int)bullet.lines[i].Y1), new Point((int)bullet.lines[i].X2, (int)bullet.lines[i].Y2), robot.robotbody))
                {
                    return true;
                }
            }
            return false;


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

            if (d == 0)
            {
                return false;
            }

            float r = q / d;

            q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
            float s = q / d;

            if (r < 0 || r > 1 || s < 0 || s > 1)
            {
                return false;
            }

            return true;
        }

    }
}

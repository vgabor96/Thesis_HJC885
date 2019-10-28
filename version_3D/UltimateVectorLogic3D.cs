using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace version_3D
{
    public class UltimateVectorLogic3D
    {

        Robot3D robot;

        public UltimateVectorLogic3D()
        {
            this.robot = new Robot3D();

        }

        public bool IsRobotHit(Robot3D robot, Bullet bullet)
        {
            for (int i = 0; i < bullet.next_location_lines.Length; i++)
            {
                if (LineIntersects_AllRects(new Point((int)bullet.next_location_lines[i].X1, (int)bullet.next_location_lines[i].Y1), new Point((int)bullet.next_location_lines[i].X2, (int)bullet.next_location_lines[i].Y2), robot.robotbody))
                {
                    return true;
                }
            }
            return false;


        }

        public bool IsRobotHit_Console(Robot3D robot, Bullet bullet)
        {
            for (int i = 0; i < bullet.destination_lines.Length; i++)
            {
                if (LineIntersects_AllRects(new Point((int)bullet.destination_lines[i].X1, (int)bullet.destination_lines[i].Y1), new Point((int)bullet.destination_lines[i].X2, (int)bullet.destination_lines[i].Y2), robot.robotbody))
                {
                    return true;
                }
            }
            return false;


        }

        public static bool LineIntersects_AllRects(System.Windows.Point p1, System.Windows.Point p2, List<Rect> r)
        {
            foreach (Rect item in r)
            {
                if (LineIntersectsRect(p1, p2, item))
                {
                    return true;
                }

            }
            return false;
        }

        public static bool LineIntersectsRect(System.Windows.Point p1, System.Windows.Point p2, Rect r)
        {
            int robot_x = (int)r.X;
            int robot_y = (int)r.Y;
            int robot_width = (int)r.Width;
            int robot_height = (int)r.Height;
            return LineIntersectsLine(p1, p2, new Point(robot_x, robot_y), new Point(robot_x + robot_width, robot_y)) ||
                   LineIntersectsLine(p1, p2, new Point(robot_x + robot_width, robot_y), new Point(robot_x + robot_width, robot_y + robot_height)) ||
                   LineIntersectsLine(p1, p2, new Point(robot_x + robot_width, robot_y + robot_height), new Point(robot_x, robot_y + robot_height)) ||
                   LineIntersectsLine(p1, p2, new Point(robot_x, robot_y + robot_height), new Point(robot_x, robot_y)) ||
                   (r.Contains(p1) && r.Contains(p2));
        }

        private static bool LineIntersectsLine(Point l1p1, Point l1p2, Point l2p1, Point l2p2)
        {
            float q = (float)((l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y));
            float d = (float)((l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X));

            if (d == 0)
            {
                return false;
            }

            float r = q / d;

            q = (float)((l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y));
            float s = q / d;

            if (r < 0 || r > 1 || s < 0 || s > 1)
            {
                return false;
            }

            return true;
        }

    }
}

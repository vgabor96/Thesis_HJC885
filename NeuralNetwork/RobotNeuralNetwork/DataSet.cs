﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNeuralNetwork
{
    public class DataSet
    {
        int width, height;



        // 2 * 2D Point;
        public const int InputSize = 2 * 2;

        public List<float> Input { get; set; } = new List<float>();

        // Robot Movements => Moving robotbody  LATER moving head moving moving legs
        public const int OutputSize = 1 * 3;

        public List<float> Output { get; set; } = new List<float>();

        public int Count { get; set; }

        //500,300,514,200,1,0,1 
        public DataSet(string filename)
        {

            LoadData(filename);
        }

        void LoadData(string filename)
        {
            string[] datarow = File.ReadAllLines(filename);
            foreach (string item in datarow)
            {
                string[] data = item.Split(',');
                int point1x = Convert.ToInt32(data[0]);
                int point1y = Convert.ToInt32(data[1]);
                int point2x = Convert.ToInt32(data[2]);
                int point2y = Convert.ToInt32(data[3]);
                int movex = Convert.ToInt32(data[4]);
                int movey = Convert.ToInt32(data[5]);
                int movez = Convert.ToInt32(data[6]);

                Input.Add((float)point1x);
                Input.Add((float)point1y);
                Input.Add((float)point2x);
                Input.Add((float)point2y);

                Output.Add((float)movex);
                Output.Add((float)movey);
                Output.Add((float)movez);

            }
        }
    }
}

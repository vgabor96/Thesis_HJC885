using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using UnityEngine.WSA;
using System.IO;
using System.Security.Cryptography;
using UnityEngine.UI;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using System.Linq;

public class PictureToVector : MonoBehaviour
{
    // Start is called before the first frame update
    Mat pic1;
    Mat pic2;
    const int N = 1080; //768
    const int M = 1920; //1024
    float v_threshold = 1;
    string folderpath = "C:/Users/loahc/Documents/GitHub/Thesis_HJC885/Unity/Thesis_HJC885/Assets/screenshots/temp/";
    List<string> filenames;
    bool process;

    public byte[] img1bytes;
    public byte[] img2bytes;

    public Texture2D text1;
    public Texture2D text2;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {



    }


    public Vector3 CalculateBulletDest()
    {
        process = true;



        Mat img = OpenCvSharp.Unity.TextureToMat(text1);

        Mat img2 = OpenCvSharp.Unity.TextureToMat(text2);

        int height = img.Rows;
        int width = img.Cols;
        //convert image from CV::MAT to float*.
        Mat dst = new Mat();
        //img.ConvertTo(dst,MatType.CV_32F);
        //float* data = dst.ptr<float>();

        //Mat dst2;
        //img2.convertTo(dst2, CV_32F);
        //float* data2 = dst2.ptr<float>();


        OpenCvSharp.Cv2.Subtract(img, img2, dst);
        //Cv2.ImShow("Subpic", dst);

        Mat dst2 = new Mat();
        OpenCvSharp.Cv2.CvtColor(dst, dst2, ColorConversionCodes.RGB2GRAY);
        //Cv2.ImShow("Gray", dst2);

        Mat dst3 = new Mat();
        OpenCvSharp.Cv2.Threshold(dst2, dst3, v_threshold, 255, ThresholdTypes.Binary);
        //Cv2.ImShow("Black_and_white", dst3);

        Mat dst4 = new Mat();
        OpenCvSharp.Cv2.GaussianBlur(dst3, dst4, new Size(9, 9), 2, 2);
        //Cv2.ImShow("GaussianBlur", dst4);

        Vector3 result = new Vector3();

        // CircleSegment[] circles = Cv2.HoughCircles(dst4, HoughMethods.Gradient, 1, 2, 30, 10, 3, 20);
        CircleSegment[] circles = Cv2.HoughCircles(dst4, HoughMethods.Gradient, 1, 2, 20, 10, 3, 30);
        int param1 = 20;
        int param2 = 10;
        while (circles.Length<2)
        {
            if (!(param1<2) && !(param2 <2))
            {
                param1--;
                param2--;
                circles = Cv2.HoughCircles(dst4, HoughMethods.Gradient, 1, 2, param1, param2, 3, 30);
            }
        
        }
     


        double maxdist = 5;
        bool notgood = false;
       
        List<CircleSegment> circlesres = new List<CircleSegment>();
        if (circles.Length >= 2)
        {
            CircleSegment[] farawaycircles = Mostfarawaycircles(circles);
            //circlesres.Add(circles[0]);
            //circlesres.Add(circles[1]);
            circlesres.Add(farawaycircles[0]);
            circlesres.Add(farawaycircles[1]);


            for (int i = 2; i < circles.Length; i++)
            {
                bool isdistinct = false;
                for (int j = 0; j < circlesres.Count; j++)
                {

                    double dist = Vector2.Distance(new Vector2(circles[i].Center.X, circles[i].Center.Y), new Vector2(circlesres[j].Center.X, circlesres[j].Center.Y));
                    if (dist < maxdist)
                    {
                        if (circles[i].Radius > circlesres[j].Radius)
                        {
                            circlesres[j] = circles[i];
                        }
                        else
                        {
                            isdistinct = false;
                            notgood = true;
                        }
                      
                    }
                    if (dist > maxdist)
                    {
                            isdistinct = true;   
                    }
                }
                if (notgood)
                {
                    notgood = false;
                    continue;
                }
                //if (isdistinct && !circlesres.Contains(circles[i]))
                //{
                //    circlesres.Add(circles[i]);
                //}
                   
                          
            }

           
             

            
            if (circlesres.Count == 2)
            {

                Cv2.Circle(dst4, circlesres[0].Center, (int)circlesres[0].Radius, Scalar.Red, 2);
                Cv2.Circle(dst4, circlesres[1].Center, (int)circlesres[1].Radius, Scalar.Red, 2);

                Cv2.Line(dst4, circlesres[0].Center, circles[1].Center, Scalar.Red, 3);
                Vector3 first = new Vector3(circlesres[0].Center.X, circlesres[0].Center.Y, 0);
                Debug.Log("FIRST: "+first);


                Vector3 second = new Vector3(circlesres[1].Center.X, circlesres[1].Center.Y, 0);
                Debug.Log("SECOND:"+ second);
                result = second - first;
                //Cv2.NamedWindow("Circles");
                //Cv2.ResizeWindow("Circles", 40, 40);
                //Cv2.ImShow("Circles", dst4);
            }


        }

      

        Debug.Log("Result "+result);

        return result;

    }

    private CircleSegment[] Mostfarawaycircles(CircleSegment[] inputcircles) 
    {
        CircleSegment[] circles = new CircleSegment[2];
        double dist = 0;
        CircleSegment circ1 = new CircleSegment();
        CircleSegment circ2 = new CircleSegment();
        double temp = 0;
        for (int i = 0; i < inputcircles.Length; i++)
        {
            for (int j = 0; j < inputcircles.Length; j++)
            {
                temp = Point2f.Distance(inputcircles[i].Center, inputcircles[j].Center);
                if ( temp> dist)
                {
                    dist = temp;
                    circ1 = inputcircles[i];
                    circ2 = inputcircles[j];
                }
            }
        }
        circles[0] = circ1;
        circles[1] = circ2;

        return circles;
    } 
}

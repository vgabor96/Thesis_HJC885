using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using UnityEngine.WSA;
using System.IO;
using System.Security.Cryptography;
using UnityEngine.UI;

public class PictureToVector : MonoBehaviour
{
    // Start is called before the first frame update
    Mat pic1;
    Mat pic2;
    const int N = 1080; //768
    const int M = 1920; //1024
    float v_threshold = 10;
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

        Cv2.NamedWindow("valami");
        Cv2.ImShow("valami", img);
        
        Cv2.NamedWindow("valami2");
        Cv2.ImShow("masodik", img2);
        int height = img.Rows;
        int width = img.Cols;
        //convert image from CV::MAT to float*.
        Mat dst = new Mat();
        //img.ConvertTo(dst,MatType.CV_32F);
        //float* data = dst.ptr<float>();

        //Mat dst2;
        //img2.convertTo(dst2, CV_32F);
        //float* data2 = dst2.ptr<float>();


        OpenCvSharp.Cv2.Subtract(img2, img, dst);
        Cv2.ImShow("kivonas", dst);

   

        return new Vector3(0,0,0);

    }

}

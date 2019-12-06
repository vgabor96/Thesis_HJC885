﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;
using System.Threading;

public class HiResScreenShots : MonoBehaviour
{
    public int resWidth = 1024;
    public int resHeight = 768;
    public Camera camera;
    private bool takeHiResShot = false;
    public float timer = float.MaxValue;
    private bool cantakeshot2 = false;
    public float secondcapturedelay = .2f;
    private Robot robot;


    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}_01.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
    public static string ScreenShotName2(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}_02.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }
    private void Start()
    {
       robot = GameObject.Find("Robot_Body").GetComponent<Robot>();
    }

    void LateUpdate()
    {
        takeHiResShot |= Input.GetKeyDown("k");
        if (takeHiResShot)
        {
            TakeHiResShot1();
            Debug.LogError("TakeHiResShot11111111111111");
            cantakeshot2 = true;
            takeHiResShot = false;
            timer = Time.time + secondcapturedelay;

        }
        if (cantakeshot2 && Time.time>timer )
        {
            Debug.LogError("TakeHiResShot2222222");
            TakeHiResShot2();
                cantakeshot2 = false;
            timer = float.MaxValue;
            // BREAKSTHE GAME TEST
            Time.timeScale = 0;

           // robot.DoMovement = true;



        }
       
      
    }

    private void TakeHiResShot1()
    {

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename;
     
            filename = ScreenShotName(resWidth, resHeight);
     
       
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));

    }
    private void TakeHiResShot2()
    {
        new WaitForSeconds(5);
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename;
       
            filename = ScreenShotName2(resWidth, resHeight);
        

        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));

    }

   




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;
using System.Threading;

public class HiResScreenShots : MonoBehaviour
{
    public int resWidth = 1024;
    public int resHeight = 768;
    public bool Istakingpictures = true;
    public Camera camera;
    private bool takeHiResShot = false;
    public float timer = float.MaxValue;
    private bool cantakeshot2 = false;
    public float capturedelay = .5f;
    public float secondcapturedelay = 5f;
    private bool start;
    public float alltime;
    private GameObject robot;
    private Bullet_Movement_Script bullet;
    private GameObject bulletgenerator;
    


    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/temp/screen_{1}x{2}_{3}_01.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
    public static string ScreenShotName2(int width, int height)
    {
        return string.Format("{0}/screenshots/temp/screen_{1}x{2}_{3}_02.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public static string ScreenShotName(int width, int height, Bullet_Movement_Script bullet)
    {
        return string.Format("{0}/screenshots/temp/{3}_{1}_01_{2}.png",
                             Application.dataPath,
                             //width, height,
                             bullet.destination,
                             bullet.isreallyrobothitted,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
    public static string ScreenShotName2(int width, int height, Bullet_Movement_Script bullet)
    {
        return string.Format("{0}/screenshots/temp/{3}_{1}_02_{2}.png",
                             Application.dataPath,
                             //width, height,
                             bullet.destination,
                             bullet.isreallyrobothitted,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakeHiResShot()
    {
        takeHiResShot = true;
        start = true;
    }

    public void TakeHiResShot(Bullet_Movement_Script bullet)
    {
        this.bullet = bullet;
        takeHiResShot = true;
        start = true;
    }
    private void Start()
    {
        robot = GameObject.Find("Robot_Body");
        bulletgenerator = GameObject.Find("BulletGenerator");
        Debug.Log("Robot pos" + robot.transform.position);
        Debug.Log("bulletgen pos" + bulletgenerator.transform.position);
        alltime = Vector3.Distance(robot.transform.position,bulletgenerator.transform.position)/bulletgenerator.GetComponent<Bullet_Shooter_Script>().bulletspeed;

        Debug.Log("ALLTIME" + alltime);

        capturedelay = 0;//alltime / 3;
        secondcapturedelay = alltime / 2;


    }

    void LateUpdate()
    {
        if (Istakingpictures)
        {
            //takeHiResShot |= Input.GetKeyDown("k");
            if (takeHiResShot)
            {
                if (Time.time > timer)
                {
                    Debug.Log("TakeHiResShot11111111111111");
                    TakeHiResShot1();

                    cantakeshot2 = true;
                    takeHiResShot = false;
                    timer = Time.time + secondcapturedelay;
                }
                else if (start)
                {
                    timer = Time.time + capturedelay;
                    start = false;
                }


            }
            if (cantakeshot2 && Time.time > timer)
            {
                Debug.Log("TakeHiResShot2222222");
                TakeHiResShot2();
                cantakeshot2 = false;
                timer = float.MaxValue;
                // BREAKSTHE GAME TEST
                //Time.timeScale = 0;

                // robot.DoMovement = true;
                //Time.timeScale = 0.5f;
                //GameObject.Find("Robot_Body").GetComponent<Robot>().RandomMovement();
                //Time.timeScale = 1;

            }
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
     
            filename = ScreenShotName(resWidth, resHeight,bullet);
     
       
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));

    }
    private void TakeHiResShot2()
    {
        //new WaitForSeconds(5);
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
       
            filename = ScreenShotName2(resWidth, resHeight,bullet);
        

        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));

    }

   




}

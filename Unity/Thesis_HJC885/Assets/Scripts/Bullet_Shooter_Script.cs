using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet_Shooter_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public float delay = 1f;

    public float recoil = 1f;
    private int currentBullets = 0;
    public int numberOfBullets = 8;
    private Vector3 gen_robot_vector;
    public float bulletspeed =0f;
    private float ResetDistance = 1000f;

    private int Learnedbulletcountact = 0;
    private int Learnedbulletcountsmax = 0;

    private int RandomTextbulletcountact = 0;
    private int RandomTextbulletcountmax = 0;

    private List<Vector3> learnedbulletvectors;

    private List<Vector3> randomtextbulletvectors;

    public Vector3 Fixedshootvector= new Vector3(0,0,0);

    public Bullet_Movement_Script Bullet;

    List<Vector3> usedvectors;

    public GameObject robotobject;
    Robot robot;
    public float actualbulletsize = 1;


    public enum WhereToShootEnum
{
 head = 1,
 body = 2,
 legs = 3,
 random = 4
}
    public enum ShootTypeEnum
    { 
        FixedBullet,
        Random,
        FromRandomTextBullets,
        LearnedBullets
       
    }
    public WhereToShootEnum Wheretoshootenum = WhereToShootEnum.random;
    public ShootTypeEnum ShootTypeenum = ShootTypeEnum.Random;
 
    void Start()
    {
        Learnedbulletcountact = 0;
        learnedbulletvectors = new List<Vector3>();
        randomtextbulletvectors = new List<Vector3>();
        usedvectors = new List<Vector3>();
        //Move the object to the same position as the parent:
        robot = robotobject.transform.Find("Robot_Body").GetComponent<Robot>();


        if (this.ShootTypeenum == ShootTypeEnum.FromRandomTextBullets)
        {
            foreach (Vector3 item in HandleTextFile.ReadBullets())
            {
                randomtextbulletvectors.Add(item);
            }
            RandomTextbulletcountmax = randomtextbulletvectors.Count;
        }

        else if (this.ShootTypeenum == ShootTypeEnum.LearnedBullets)
        {
            foreach (Vector3 item in HandleTextFile.ReadSolutions().Keys)
            {
                learnedbulletvectors.Add(item);
            }
            Learnedbulletcountsmax = learnedbulletvectors.Count;
        }
       // robot = gameObject.GetComponent<Robot>();

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        this.ResetDistance = Vector3.Distance(this.robotobject.transform.position, this.transform.position)+5f;//+10f;
        this.gen_robot_vector = robotobject.transform.position - this.transform.position;
        Debug.DrawLine(transform.localPosition, robotobject.transform.localPosition);

        InstianteBullet();
      

    }

    // Update is called once per frame
    void Update()
    {
       
            if (this.Bullet != null && Vector3.Distance(this.Bullet.startingPos, this.Bullet.mPrevPos) >= ResetDistance)
            {
                ReGenerate(this.Bullet);
            Debug.Log("StartPos: "+this.Bullet.transform.position);
            }

            //if (item.isfired)
            //{
            //    GameObject.Find("Robot_Body").GetComponent<Robot>().DoMovement = true;
            //}
    }

    private void InstianteBullet()
    {
      

        if (currentBullets < numberOfBullets)
        {
         
            this.Bullet= Instantiate(Bullet, transform.position,this.transform.rotation);
            //setting the bullet's length to fix mSpeed
            this.Bullet.destination = Fixedshootvector.normalized* bulletspeed;
            //this.Bullet.destination.x = (float)Math.Round(this.Bullet.destination.x, 1);
            //this.Bullet.destination.y = (float)Math.Round(this.Bullet.destination.y, 1);
            //this.Bullet.destination.z = (float)Math.Round(this.Bullet.destination.z, 1);
            this.Bullet.GetComponent<SphereCollider>().radius *= this.Bullet.transform.localScale.x;
            actualbulletsize = this.Bullet.GetComponent<SphereCollider>().radius;
            //SetDestination(this.Bullet);
          
          

            this.Bullet.transform.position = this.transform.position;
            var body = GameObject.Find("Robot_Body").transform;

            //Setting Bullet Size, Rotation, MSpeed,ResetDistance,Destination
           // this.Bullet.transform.localScale = body.GetChild(0).GetComponent<BoxCollider>().size*actualbulletsize;
            this.Bullet.transform.rotation = this.transform.rotation;
          
            ReGenerate(this.Bullet);
            //this.Bullet.mSpeed = this.mSpeed;
            this.Bullet.ResetDistance = this.ResetDistance;

           
            currentBullets++;
        }


    }
 
    private void ReGenerate(Bullet_Movement_Script bullet)
    {
        GameObject.Find("Robot_Body").GetComponent<Robot>().DoReset = true;
        // GameObject.Find("Robot_Body").GetComponent<Robot>().Reset();
        //this.Bullet.gameObject.transform.GetChild(0).transform.position = new Vector3(bullet.resetx, this.Bullet.gameObject.transform.GetChild(0).transform.position.y, this.Bullet.gameObject.transform.GetChild(0).transform.position.z);
        bullet.israydone = false;
        bullet.this_ID++;
        //bullet.isfired = true;
        if (!bullet.ishittedrobot)
        {
            Debug.Log($"Bullet ID: {bullet.this_ID} Vector:{bullet.destination} Length: {bullet.destination.magnitude} Hit => NONE");
        }
        bullet.ishittedrobot = false;
        //bullet.isrobothitted = false;
        bullet.transform.position = this.transform.position;
        bullet.ishit = true;

     
        SetDestination(this.Bullet);
        

        //GameObject.Find("Robot_Body").GetComponent<Robot>().actbulletthits = bullet.hits;
     
       
        GameObject.Find("RobotCamera").GetComponent<HiResScreenShots>().TakeHiResShot(bullet);


    }

    private void SetDestination(Bullet_Movement_Script bullet)
    {


        switch (this.ShootTypeenum)
        {
            case ShootTypeEnum.FixedBullet:
                break;
            case ShootTypeEnum.Random:
                Vector3 vec = (DestinationRandomize() - transform.position).normalized;
                Vector3 randvec = new Vector3((float)Math.Round(vec.x, 3), (float)Math.Round(vec.y, 3), (float)Math.Round(vec.z, 3));
                bullet.destination = randvec * bulletspeed;
                //bullet.destination = DestinationRandomize().normalized * bulletspeed;
                break;
            case ShootTypeEnum.FromRandomTextBullets:
                if (RandomTextbulletcountact >= RandomTextbulletcountmax)
                {
                    //Application.Quit();
                    UnityEditor.EditorApplication.isPlaying = false;
                }
                bullet.destination = randomtextbulletvectors[RandomTextbulletcountact];
                RandomTextbulletcountact++;
                break;
            case ShootTypeEnum.LearnedBullets:
                if (Learnedbulletcountact >= Learnedbulletcountsmax)
                {
                    Learnedbulletcountact = 0;

                }
                bullet.destination = learnedbulletvectors[Learnedbulletcountact];
                Learnedbulletcountact++;
                break;
            default:
                break;
        }
        //if (this.ShootTypeenum == ShootTypeEnum.Random)
        //{
        //    Vector3 vec = (DestinationRandomize() - transform.position).normalized * bulletspeed;
        //    Vector3 randvec = new Vector3((float)Math.Round(vec.x, 1), (float)Math.Round(vec.y, 1), (float)Math.Round(vec.z, 1));
        //    bullet.destination = randvec;
        //    //bullet.destination = DestinationRandomize().normalized * bulletspeed;
        //}
        //else if (this.ShootTypeenum == ShootTypeEnum.LearnedBullets)
        //{
        //    if (Learnedbulletcountact >= Learnedbulletcountsmax)
        //    {
        //        Learnedbulletcountact = 0;

        //    }
        //    bullet.destination = learnedbulletvectors[Learnedbulletcountact] * bulletspeed;
        //    Learnedbulletcountact++;

        //}
      
     
    }


    public void Generate1000_RandomToTextRandom()
    {
        for (int i = 500; i < 700; i++)
        {

            for (int j = -50; j < 50; j++)
            {
                for (int l = -50; l < 50; l++)
                {
                    Vector3 vec = new Vector3((float)i/100, (float)j/100, (float)l/100);
                    HandleTextFile.WriteBullet(vec);
                }
            }
          
           
            //Vector3 final = randvec * bulletspeed * 5;

          
        }
      

    }



    private Vector3 DestinationRandomize()
    {
       
        switch (this.Wheretoshootenum)
        {
            case WhereToShootEnum.head:
                return Random_Shootaround(GameObject.Find("Head").GetComponent<BoxCollider>());
                
            case WhereToShootEnum.body:
                return Random_Shootaround(GameObject.Find("Body").GetComponent<BoxCollider>());
        
            case WhereToShootEnum.legs:
                return Random_Shootaround(GameObject.Find("Legs").GetComponent<BoxCollider>());
            default:
                return Random_Shootaround(GameObject.Find("Robot_Body").GetComponent<BoxCollider>());
        }


    }
    private Vector3 Random_Shootaround(BoxCollider bodypart)
    {
        Vector3 vector;
        float x;
        float y;
        float z;

        do
        {
            x = bodypart.transform.position.x + Random.Range(-bodypart.size.x * recoil, bodypart.size.x * recoil);
            y = bodypart.transform.position.y + Random.Range(-bodypart.size.y * recoil, bodypart.size.y * recoil);
            z = bodypart.transform.position.z + Random.Range(-bodypart.size.z * recoil, bodypart.size.z * recoil);

            x = (float)Math.Round(x, 1);
            y = (float)Math.Round(y, 1);
            z = (float)Math.Round(z, 1);

            vector = new Vector3(x, y, z);
       
        } while (usedvectors.Contains(vector));

        usedvectors.Add(vector);
        return vector;
    }

    public void RayCastBullet()
    {
        this.Bullet.RAYCAST();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Shooter_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public float delay = 1f;

    public float recoil = 1f;
    private int currentBullets = 0;
    public int numberOfBullets = 8;
    private Vector3 gen_robot_vector;
    public float mSpeed = 10f;
    private float ResetDistance = 100f;

    public Bullet_Movement_Script Bullet;
   

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
        Random
       
    }
    public WhereToShootEnum Wheretoshootenum = WhereToShootEnum.random;
    public ShootTypeEnum ShootTypeenum = ShootTypeEnum.Random;





    
    void Start()
    {

        //Move the object to the same position as the parent:
        //robot = robotobject.GetComponent<Robot>();
        robot = robotobject.transform.Find("Robot_Body").GetComponent<Robot>();
        //robot.DoMovement = true;
        
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        this.ResetDistance = Vector3.Distance(this.robotobject.transform.position, this.transform.position);//+10f;
        this.gen_robot_vector = robotobject.transform.position - this.transform.position;
        Debug.DrawLine(transform.localPosition, robotobject.transform.localPosition);

        GenerateBullets();
      

    }

    // Update is called once per frame
    void Update()
    {
       
            if (this.Bullet != null && Vector3.Distance(this.Bullet.startingPos, this.Bullet.mPrevPos) >= ResetDistance)
            {
                ReGenerate(this.Bullet, false);

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
            SetDestination(this.Bullet);

            this.Bullet.transform.position = this.transform.position;
            var body = GameObject.Find("Robot_Body").transform;

            //Setting Bullet Size, Rotation, MSpeed,ResetDistance,Destination
            this.Bullet.transform.localScale = body.GetChild(0).GetComponent<BoxCollider>().size*actualbulletsize;
            this.Bullet.transform.rotation = this.transform.rotation;
          
            ReGenerate(this.Bullet, false);
            this.Bullet.mSpeed = mSpeed;
            this.Bullet.ResetDistance = ResetDistance;

            currentBullets++;
        }


    }

    private void InstantiateBulletAllatOnce()
    {
        if (currentBullets < numberOfBullets)
        {

            this.Bullets.Add(Instantiate(Bullet, transform.position, this.transform.rotation));
            SetDestination(Bullets[currentBullets]);

            Bullets[currentBullets].transform.position = this.transform.position;
            var body = GameObject.Find("Robot_Body").transform;

            //Setting Bullet Size, Rotation, MSpeed,ResetDistance,Destination
            Bullets[currentBullets].transform.localScale = body.GetChild(0).GetComponent<BoxCollider>().size *actualbulletsize;
            Bullets[currentBullets].transform.rotation = this.transform.rotation;

            ReGenerate(Bullets[currentBullets], true);
            Bullets[currentBullets].mSpeed = mSpeed;
            Bullets[currentBullets].ResetDistance = ResetDistance;

            currentBullets++;
        }
    }

    private void InstantiateBulletOnlyOncewithDelay()
    {

        //GameObject.Find("Robot_Body").GetComponent<Robot>().DoMovement = true;



        if (currentBullets < numberOfBullets)
        {
            var body = GameObject.Find("Robot_Body").transform;


            this.Bullets.Add(Instantiate(Bullet, transform.position, this.transform.rotation));
            SetDestination(Bullets[currentBullets]);

            Bullets[currentBullets].transform.position = this.transform.position;
            //robotobject.transform;

            //Setting Bullet Size, Rotation, MSpeed,ResetDistance,Destination
            Bullets[currentBullets].transform.localScale = body.GetChild(0).GetComponent<BoxCollider>().size * actualbulletsize;
            Bullets[currentBullets].transform.rotation = this.transform.rotation;

           

            currentBullets++;
        }

      
        ReGenerate(Bullets[0], false);
        Bullets[0].mSpeed = mSpeed;
        Bullets[0].ResetDistance = ResetDistance;

      
  
    }

    private void GenerateBullets()
    { 
        switch (ShootTypeenum)
        {
            case ShootTypeEnum.Continous:
                SpawnBulletsContinous();
                break;
            case ShootTypeEnum.AllAtOnce:
                SpawnBulletsAllAtOnce();
                break;
            case ShootTypeEnum.OnlyOnewithDelay:
                numberOfBullets = 1;
                SpawnOnlyOneBulletWithDelay();
                break;
            default:
                break;
        }
       
               
       
    }

    private void SpawnBulletsAllAtOnce()
    {
        InvokeRepeating(nameof(InstantiateBulletAllatOnce), 0, 0.0001f);
      
    }

    private void SpawnOnlyOneBulletWithDelay()
    {
       
        InvokeRepeating(nameof(InstantiateBulletOnlyOncewithDelay), 0, delay);
    }

    private void SpawnBulletsContinous()
    {
        InvokeRepeating(nameof(InstianteBullet), 0, delay);
    }

    private void ReGenerate(Bullet_Movement_Script bullet, bool waitforall)
    {
    
            //Debug.Log($"BUllet ID: {bullet.this_ID} Vector:{bullet.destination} Hit => NONE");
        
        GameObject.Find("Robot_Body").GetComponent<Robot>().Reset();
        bullet.isfired = true;
        bullet.isreallyrobothitted = false;

        //Debug.Log(bullet.this_ID.ToString()+robot + " MOOOOVE");

        if (waitforall)
        {
            if (this.Bullets.FindAll(x => x.ishit == false).Count == 0) 
             {
                bullet.transform.position = this.transform.position;
                bullet.ishit = true;
                SetDestination(bullet);
              
             
            }
        }
        else
        {
            ReGenerate(bullet);
        
           
        }
        GameObject.Find("RobotCamera").GetComponent<HiResScreenShots>().TakeHiResShot(bullet);

    }

    private void ReGenerate(Bullet_Movement_Script bullet)
    {
        bullet.transform.position = this.transform.position;
                bullet.ishit = true;
                SetDestination(bullet);

    }

    private void SetDestination(Bullet_Movement_Script bullet)
    {
        bullet.destination = (DestinationRandomize() - transform.position).normalized;
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
        
        float x = bodypart.transform.position.x + Random.Range(-bodypart.size.x*recoil, bodypart.size.x*recoil);
        float y = bodypart.transform.position.y + Random.Range(-bodypart.size.y * recoil, bodypart.size.y * recoil);
        float z = bodypart.transform.position.z + Random.Range(-bodypart.size.z * recoil, bodypart.size.z * recoil);

        return new Vector3(x, y, z);
    }

}

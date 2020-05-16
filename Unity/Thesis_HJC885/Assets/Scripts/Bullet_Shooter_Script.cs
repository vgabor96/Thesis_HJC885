﻿using System.Collections;
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
    public float bulletspeed =0f;
    private float ResetDistance = 1000f;

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
        Random
       
    }
    public WhereToShootEnum Wheretoshootenum = WhereToShootEnum.random;
    public ShootTypeEnum ShootTypeenum = ShootTypeEnum.Random;
 
    void Start()
    {
        usedvectors = new List<Vector3>();
        //Move the object to the same position as the parent:
        robot = robotobject.transform.Find("Robot_Body").GetComponent<Robot>();
      
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        this.ResetDistance = Vector3.Distance(this.robotobject.transform.position, this.transform.position);//+10f;
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
            if (this.ShootTypeenum == ShootTypeEnum.Random)
            {
                SetDestination(this.Bullet);
            }
          

            this.Bullet.transform.position = this.transform.position;
            var body = GameObject.Find("Robot_Body").transform;

            //Setting Bullet Size, Rotation, MSpeed,ResetDistance,Destination
            this.Bullet.transform.localScale = body.GetChild(0).GetComponent<BoxCollider>().size*actualbulletsize;
            this.Bullet.transform.rotation = this.transform.rotation;
          
            ReGenerate(this.Bullet);
            //this.Bullet.mSpeed = this.mSpeed;
            this.Bullet.ResetDistance = this.ResetDistance;


            currentBullets++;
        }


    }
 
    private void ReGenerate(Bullet_Movement_Script bullet)
    {
        GameObject.Find("Robot_Body").GetComponent<Robot>().Reset();
        bullet.this_ID++;
        //bullet.isfired = true;
        bullet.isrobothitted = false;
        bullet.transform.position = this.transform.position;
        bullet.ishit = true;

        if (ShootTypeenum == ShootTypeEnum.Random)
        {
            SetDestination(this.Bullet);
        }
        //GameObject.Find("Robot_Body").GetComponent<Robot>().actbulletthits = bullet.hits;
        GameObject.Find("RobotCamera").GetComponent<HiResScreenShots>().TakeHiResShot(bullet);

    }

    private void SetDestination(Bullet_Movement_Script bullet)
    {
       
        bullet.destination = (DestinationRandomize() - transform.position).normalized*bulletspeed;
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

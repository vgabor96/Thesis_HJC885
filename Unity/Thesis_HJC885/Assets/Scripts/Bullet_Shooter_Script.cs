using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Shooter_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public float delay = 1f;
    private int currentBullets = 0;
    public int numberOfBullets = 8;
    public float shootradius = 5f;
    private Vector3 gen_robot_vector;
    public float mSpeed = 10f;
    public float ResetDistance = 100f;

    public Bullet_Movement_Script Bullet;
    public GameObject robot;
    //public float maxdistancereset;
    //public float mSpeed;

    private Vector3 randomaround;

    public enum WhereToShootEnum
{
 head = 1,
 body = 2,
 legs = 3,
 random = 4
}
    public WhereToShootEnum Wheretoshootenum = WhereToShootEnum.random;


    private Bullet_Movement_Script[] Bullets;


    
    void Start()
    {

        //Move the object to the same position as the parent:

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        this.gen_robot_vector = robot.transform.position - this.transform.position;
        //Align();
        this.Bullets = new Bullet_Movement_Script[numberOfBullets];
        //for (int i = 0; i < numberOfBullets; i++)
        //{
        //    this.Bullets.Add(Bullet.GetComponent<Bullet_Movement_Script>());
        //    Debug.Log(this.Bullets[i].this_ID);
        //}
        //Align();
        
        GenerateBullets();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, robot.transform.position, Color.green);
        
            foreach (Bullet_Movement_Script item in this.Bullets)
            {
                if (item != null && Vector3.Distance(item.startingPos, item.mPrevPos) >= ResetDistance)
                {
                    ReGenerate(item);

                }
            }
        
       
    }

    private void InstianteBullet()
    {
        if (currentBullets < numberOfBullets)
        {
            //Quaternion rot = Quaternion.Euler(DestinationRandomize() - this.transform.position);
         
            this.Bullets[currentBullets] = Instantiate(Bullet, transform.position,this.transform.rotation);
            //Align(this.Bullets[currentBullets]);
            SetDestination(Bullets[currentBullets]);
     
            Bullets[currentBullets].transform.position = this.transform.position;
            var body = GameObject.Find("Robot_Body").transform;

            //Setting Bullet Size, Rotation, MSpeed,ResetDistance,Destination
            Bullets[currentBullets].transform.localScale = body.GetChild(0).GetComponent<BoxCollider>().size *0.1f;
            Bullets[currentBullets].transform.rotation = this.transform.rotation;
          
            ReGenerate(Bullets[currentBullets]);
            Bullets[currentBullets].mSpeed = mSpeed;
            Bullets[currentBullets].ResetDistance = ResetDistance;
            //Bullets[currentBullets].destination = gen_robot_vector;



            currentBullets++;
        }

      


    }

    private void GenerateBullets()
    {
            InvokeRepeating(nameof(InstianteBullet), 0, delay);
               
       
    }

    private void ReGenerate(Bullet_Movement_Script bullet)
    {
        bullet.transform.position = this.transform.position;
        //Debug.Log(bullet.this_ID+"ishit TRUE");
        bullet.ishit = true;
        //bullet.destination = this.gen_robot_vector;
        SetDestination(bullet);
        //destination = RandomDestinationGenerator(min, max);
        //this.RandomDestinationGenerator();
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
                return Random_Shootaround(GameObject.Find("Body").GetComponent<BoxCollider>());
        }


    }
    private Vector3 Random_Shootaround(Collider bodypart)
    {
        
        float x = bodypart.transform.position.x + Random.Range(-bodypart.transform.localScale.x, bodypart.transform.localScale.x);
        float y = bodypart.transform.position.y + Random.Range(-bodypart.transform.localScale.y, bodypart.transform.localScale.y);
        float z = bodypart.transform.position.z + Random.Range(-bodypart.transform.localScale.z, bodypart.transform.localScale.z);

        return new Vector3(x, y, z);
    }

    private void Align(Bullet_Movement_Script bullet)
    {
        Vector3 direction = this.gen_robot_vector;
        // Change child.forward to child.up if you want the up vectors to "look at" the other child object
        Quaternion rotation = Quaternion.FromToRotation(bullet.transform.up, direction);
        bullet.transform.rotation = rotation * bullet.transform.rotation;
    }

    private void Align()
    {
        Vector3 direction = this.gen_robot_vector;
        // Change child.forward to child.up if you want the up vectors to "look at" the other child object
        Quaternion rotation = Quaternion.FromToRotation(this.transform.up, direction);
        this.transform.rotation = rotation * this.transform.rotation;
    }
}

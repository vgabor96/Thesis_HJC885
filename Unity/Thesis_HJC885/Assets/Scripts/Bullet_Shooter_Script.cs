using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Shooter_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public float delay =1f;
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



    private Bullet_Movement_Script[] Bullets;


    
    void Start()
    {

        //Move the object to the same position as the parent:
        
        //transform.localPosition = new Vector3(0, 0, 0);
        this.gen_robot_vector = robot.transform.position - this.transform.position;
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
           
            this.Bullets[currentBullets] = Instantiate(Bullet, transform.position, transform.rotation);
           
           
            var body = GameObject.Find("Robot_Body").transform;

            //Setting Bullet Size, Rotation, MSpeed,ResetDistance,Destination
            Bullets[currentBullets].transform.localScale = body.GetChild(0).GetComponent<BoxCollider>().size *0.5f;
            Bullets[currentBullets].transform.rotation = this.transform.rotation;
            Bullets[currentBullets].mSpeed = mSpeed;
            Bullets[currentBullets].ResetDistance = ResetDistance;
            Bullets[currentBullets].destination = gen_robot_vector;
            


            currentBullets++;
        }

      


    }

    private void GenerateBullets()
    {
            InvokeRepeating(nameof(InstianteBullet), 0, delay);
               
       
    }
    private Vector3 RandomSpherePoints()
    {
        return Random.insideUnitSphere * shootradius/gen_robot_vector.magnitude;
    }
    //private Vector3 RandomSpherePoints()
    //{

    //    float x = UnityEngine.Random.Range(-shootradius, shootradius);
    //    float y = UnityEngine.Random.Range(-shootradius, shootradius);
    //    float z = UnityEngine.Random.Range(-shootradius, shootradius);

    //    return new Vector3(x/**mSpeed*/, y/** mSpeed*/, z/**mSpeed*/);
    //}

    private Quaternion RandomRotation()
    {
        Vector3 spherepoints = RandomSpherePoints();
        float x = Mathf.Rad2Deg * Mathf.Atan(spherepoints.x /gen_robot_vector.magnitude) + transform.rotation.eulerAngles.x;
        float y = Mathf.Rad2Deg * Mathf.Atan(spherepoints.y / gen_robot_vector.magnitude) + transform.rotation.eulerAngles.y;
        float z = Mathf.Rad2Deg * Mathf.Atan(spherepoints.z / gen_robot_vector.magnitude) + transform.rotation.eulerAngles.z;
        Vector3 angles = new Vector3(x, y, z);
        return Quaternion.Euler(angles);
    }

    private void ReGenerate(Bullet_Movement_Script bullet)
    {
        bullet.transform.position = bullet.startingPos;
        //Debug.Log(bullet.this_ID+"ishit TRUE");
        bullet.ishit = true;
        bullet.destination = this.gen_robot_vector;
        //destination = RandomDestinationGenerator(min, max);
        //this.RandomDestinationGenerator();
    }

    private void Align()
    {
        Vector3 direction = robot.transform.position - transform.position;
        // Change child.forward to child.up if you want the up vectors to "look at" the other child object
        Quaternion rotation = Quaternion.FromToRotation(transform.position, direction);
        transform.rotation = rotation * transform.rotation;
    }
}

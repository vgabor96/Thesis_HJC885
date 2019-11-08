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
    public float mSpeed;
    public float Resetdistance;

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
        
        GenerateBullets();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, robot.transform.position, Color.green);
    }

    private void InstianteBullet()
    {
        if (currentBullets < numberOfBullets)
        {
           
            this.Bullets[currentBullets] = Instantiate(Bullet, transform.position, transform.rotation/*Quaternion.Euler(new Vector3(45,0,0))*/);
            Bullets[currentBullets].mSpeed = mSpeed;
            Bullets[currentBullets].ResetDistance = Resetdistance;
          
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
}

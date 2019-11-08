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


    private List<Bullet_Movement_Script> Bullets;


    
    void Start()
    {

        //Move the object to the same position as the parent:

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        this.gen_robot_vector = robot.transform.position - this.transform.position;
        this.Bullets = new List<Bullet_Movement_Script>();

        
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
                    ReGenerate(item,false);

                }
            }        
       
    }

    private void InstianteBullet()
    {
        if (currentBullets < numberOfBullets)
        {
         
            this.Bullets.Add(Instantiate(Bullet, transform.position,this.transform.rotation));
            SetDestination(Bullets[currentBullets]);
     
            Bullets[currentBullets].transform.position = this.transform.position;
            var body = GameObject.Find("Robot_Body").transform;

            //Setting Bullet Size, Rotation, MSpeed,ResetDistance,Destination
            Bullets[currentBullets].transform.localScale = body.GetChild(0).GetComponent<BoxCollider>().size *0.1f;
            Bullets[currentBullets].transform.rotation = this.transform.rotation;
          
            ReGenerate(Bullets[currentBullets],false);
            Bullets[currentBullets].mSpeed = mSpeed;
            Bullets[currentBullets].ResetDistance = ResetDistance;

            currentBullets++;
        }     

    }

    private void GenerateBullets()
    {
            InvokeRepeating(nameof(InstianteBullet), 0, delay);
               
       
    }

    private void ReGenerate(Bullet_Movement_Script bullet, bool waitforall)
    {
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
        
        float x = bodypart.transform.position.x + Random.Range(-bodypart.size.x, bodypart.size.x);
        float y = bodypart.transform.position.y + Random.Range(-bodypart.size.y, bodypart.size.y);
        float z = bodypart.transform.position.z + Random.Range(-bodypart.size.z, bodypart.size.z);

        return new Vector3(x, y, z);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;


public class Bullet_Movement_Script : MonoBehaviour
{
    private static int Bullet_ID = 0;
    public int this_ID;
    private Vector3 startingPos;
    private Vector3 destination;
    public float ResetDistance = 1000.0f;
    public float mSpeed = 100.0f;
    private bool ishit = true;
    private Ray ray;


    //public ParticleSystem explosion;

    //public Vector3 min;
    //public Vector3 max;
  

    Vector3 mPrevPos;
    // Start is called before the first frame update
    void Start()
    {
       

        mPrevPos = transform.position;
        startingPos = mPrevPos;
        //transform.localPosition = new Vector3(0, 0, 0);
        //destination = RandomDestinationGenerator(min,max);
        this.destination = transform.position;
        //this.RandomDestinationGenerator();
        this.this_ID = Bullet_ID++;
        //setting the bullet's hitbox!!
        GetComponent<SphereCollider>().radius *= transform.localScale.x ;
        //TODO min max according to robot distance!!
        //this.destination = RandoMDestinationGeneratorbasedonRobotdistance();
       
    }

    // Update is called once per frame
    void Update()
    {

        mPrevPos = transform.position;

        //transform.Translate(mSpeed * Time.deltaTime,0.0f, 0.0f); 
        transform.Translate(destination*Time.deltaTime*mSpeed);
        ray = new Ray(mPrevPos, (transform.position - mPrevPos).normalized);
        RaycastHit[] hits = Physics.SphereCastAll(ray, GetComponent<SphereCollider>().radius, (transform.position - mPrevPos).magnitude);
        if (ishit)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.name =="Head")
                {
                  
                    Debug.Log(hits[i].collider.gameObject.name);
                    //Debug.Log(GetComponent<SphereCollider>().radius);
                    Debug.Log(this_ID);
                    Debug.Log($"{mPrevPos} {(transform.position - mPrevPos).normalized} {GetComponent<SphereCollider>().radius} {(transform.position - mPrevPos).magnitude}");
                    Debug.Break();
                    //GameObject.Find("BulletShooter_Camera").SendMessage("DoShake");
                   // explosion.Play();
                    CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, .1f);
                    ishit = false;
                }
               
            }
            
        }

        Debug.DrawRay(startingPos, ray.direction * ((transform.position - mPrevPos).magnitude) * 1000, Color.red);

        //Debug.DrawLine(startingPos, robot.transform.position, Color.green);

        if (Vector3.Distance(startingPos, mPrevPos) >= ResetDistance)
        {
            ReGenerate();
           
        }
    }

    private void ReGenerate()
    {
        transform.position = startingPos;
        ishit = true;
        //destination = RandomDestinationGenerator(min, max);
        //this.RandomDestinationGenerator();
    }

    //private void RandomDestinationGenerator()
    //{
    //    Vector3 movement = Random.insideUnitSphere * 5;/*robot.transform.position - startingPos;*/
    //    Vector3 newPos = transform.position + movement;

    //    // Calculate the distance of the new position from the center point. Keep the direction
    //    // the same but clamp the length to the specified radius.
    //    Vector3 offset = newPos - robot.transform.position;
    //    this.destination = robot.transform.position + Vector3.ClampMagnitude(offset, shootradius);
    //}

    //private Vector3 RandomDestinationVectorGenerator()
    //{

    //    float x = UnityEngine.Random.Range(-shootradius, shootradius);
    //    float y = UnityEngine.Random.Range(-shootradius, shootradius);
    //    float z = UnityEngine.Random.Range(-shootradius, shootradius);

    //    return new Vector3(x/**mSpeed*/, y/** mSpeed*/, z/**mSpeed*/);
    //}

    //private Vector3 RandoMDestinationGeneratorbasedonRobotdistance()
    //{
    //    Vector3 temp = RandomDestinationGenerator();
    //    float tempdist = Vector3.Distance(this.startingPos, robot.transform.position);
    //    Debug.Log("tempVector:" + temp);
    //    Debug.Log("tempdistance" + tempdist);


    //    temp.x = Mathf.Tan(temp.x / tempdist);
    //    temp.y = Mathf.Tan(temp.y / tempdist);
    //    temp.z = Mathf.Tan(temp.z / tempdist);
    //    Debug.Log("new Vector" + temp);
    //    return temp;
    //}

    //public void Modify_Destination()
    //{
    //    Vector3 random = RandoMDestinationGeneratorbasedonRobotdistance();

    //    destination = this.robot_startpos_Vector;

    //    if (Random.Range(0,2) == 0)
    //    {
    //        destination += Quaternion.Euler(random.x, random.y, random.z).eulerAngles;
    //    }
    //    else
    //    {
    //        destination -= Quaternion.Euler(random.x, random.y, random.z).eulerAngles;

    //    }

    //}

    private Vector3 FixDestinationGenerator()
    {
        float x = 0;
        float y = 1;
        float z = 1;
        return new Vector3(x * mSpeed, y * mSpeed, z * mSpeed);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.gameObject.name);
    //}
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(transform.position, transform.GetComponent<SphereCollider>().radius);
    }


}

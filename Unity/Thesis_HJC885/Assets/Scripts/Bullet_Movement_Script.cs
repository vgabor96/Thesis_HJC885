using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;


public class Bullet_Movement_Script : MonoBehaviour
{
    private static int Bullet_ID = 0;
    public int this_ID;
    public Vector3 startingPos;
    public Vector3 destination;
    public float ResetDistance = 1000.0f;
    public float mSpeed = 100.0f;
    public bool ishit;
    private Ray ray;
    public bool isfired;


    //public ParticleSystem explosion;

    //public Vector3 min;
    //public Vector3 max;
  

    public Vector3 mPrevPos;
    // Start is called before the first frame update
    void Start()
    {

        isfired = true;
        mPrevPos = transform.position;
        startingPos = mPrevPos;
        //transform.localPosition = new Vector3(0, 0, 0);
        //destination = RandomDestinationGenerator(min,max);
        //this.destination = transform.position;
        //this.RandomDestinationGenerator();
        this.this_ID = Bullet_ID++;
        this.ishit = true;
        //setting the bullet's hitbox!!
        GetComponent<SphereCollider>().radius *= transform.localScale.x ;
        //TODO min max according to robot distance!!
        //this.destination = RandoMDestinationGeneratorbasedonRobotdistance();
       
    }

    // Update is called once per frame
    void Update()
    {
        isfired = false;
        mPrevPos = transform.position;
        string hitname="";
        //transform.Translate(mSpeed * Time.deltaTime,0.0f, 0.0f); 
        transform.Translate(destination*Time.deltaTime*mSpeed);
        ray = new Ray(mPrevPos, (transform.position - mPrevPos).normalized);
        RaycastHit[] hits = Physics.SphereCastAll(ray, GetComponent<SphereCollider>().radius, (transform.position - mPrevPos).magnitude);
        if (ishit)
        {
            //Debug.Log($"{ this_ID} HITS:{hits.Length}");
            for (int i = 0; i < hits.Length; i++)
            {
                hitname = hits[i].collider.gameObject.name;
                if (IsRobothittedandLog(hitname))
                {

                    Debug.Log($"BUllet ID: {this_ID} Vector:{ray} Hit => {hitname}");
                    //Debug.Log(hits[i].collider.gameObject.name);
                    //Debug.Log(GetComponent<SphereCollider>().radius);
                    //Debug.Log(this_ID);
                    //Debug.Log($"{mPrevPos} {(transform.position - mPrevPos).normalized} {GetComponent<SphereCollider>().radius} {(transform.position - mPrevPos).magnitude}");
                    //Debug.Break();
                    //GameObject.Find("BulletShooter_Camera").SendMessage("DoShake");
                    // explosion.Play();
                    CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, .1f);
                    ishit = false;
                }

            }
            
        }
        //Debug.Log($"ID: {this_ID } Startpos: {startingPos}  RayDirection:{ray.direction} MPrepos:{mPrevPos}  transformPosition: {transform.position}");
        Debug.DrawRay(startingPos, ray.direction * ((transform.position - mPrevPos).magnitude) * 1000, Color.red);
       
        //Debug.DrawLine(startingPos, robot.transform.position, Color.green);


    }

    private bool IsRobothittedandLog(string name)
    {
        bool ishitted = false;
        if (name != "Robot_Body")
        {
           
            foreach (BoxCollider item in GameObject.Find("Robot_Body").GetComponentsInChildren<BoxCollider>())
            {
                if (name == item.name)
                {
                    ishitted = true;


                }
            }
        }
       

        return ishitted;

    }
    private Vector3 FixDestinationGenerator()
    {
        float x = 0;
        float y = 1;
        float z = 1;
        return new Vector3(x * mSpeed, y * mSpeed, z * mSpeed);
    }
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(transform.position, transform.GetComponent<SphereCollider>().radius);
    }


}

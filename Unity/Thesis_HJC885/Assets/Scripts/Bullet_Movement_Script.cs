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
    public float mSpeed = 1f;
    public bool ishit;
    private Ray ray;
    public Ray raystart;
    public bool isfired;
    public bool isrobothitted;
    public bool ishittedrobot;
    public bool isreallyrobothitted;
    public RaycastHit[] hits;
    public bool israydone = false;
    private GameObject robot;
    double dist = double.MaxValue;
    double dist2 = 0;
    Vector3 vectortofollowcamera;
    public float resetx = -15f;
    public float resety = 15f;
    float actx = 0f;
    float acty =0f;
    Vector3 destination2 = new Vector3();
    GameObject bulletRay;
    //public ParticleSystem explosion;

    //public Vector3 min;
    //public Vector3 max;


    public Vector3 mPrevPos;
    // Start is called before the first frame update
    void Start()
    {
        ishittedrobot = false;
        isfired = true;
        mPrevPos = transform.position;
        startingPos = mPrevPos;
     
        //transform.localPosition = new Vector3(0, 0, 0);
        //destination = RandomDestinationGenerator(min,max);
        //this.destination = transform.position;
        //this.RandomDestinationGenerator();
        this.this_ID = 0;
        this.ishit = true;
        //setting the bullet's hitbox!!
        //this.GetComponent<SphereCollider>().radius *= transform.localScale.x ;

       
        robot = GameObject.Find("Robot_Body");

        bulletRay = GameObject.Find("BulletRay");
   


     
        //TODO min max according to robot distance!!
        //this.destination = RandoMDestinationGeneratorbasedonRobotdistance();

    }

    // Update is called once per frame
    void Update()
    {
        // UpdateBulletBasic();
        UpdateBulletForTest();

    }

    private void UpdateBulletForTest()
    {
        mPrevPos = transform.position;

      

        if (destination2 != this.destination)
        {
            this.destination2 = this.destination;
            actx = 0f;
            acty = 0f;
            this.gameObject.transform.GetChild(0).transform.localPosition = new Vector3(resetx, resety, this.gameObject.transform.GetChild(0).transform.localPosition.z);
        }
       
        //transform.Translate(mSpeed * Time.deltaTime,0.0f, 0.0f); 
        transform.Translate(destination * Time.deltaTime * mSpeed);

        dist = double.MaxValue;
        foreach (Collider item in robot.GetComponentsInChildren<Collider>())
        {
            dist2 = Vector3.Distance(mPrevPos, item.transform.position);
            if (dist2 < dist)
            {
                dist = dist2;
                vectortofollowcamera = item.ClosestPointOnBounds(transform.position);
            }
        }

        //Vector3 dest;
        if (dist < 20 && this.transform.position.x < robot.transform.position.x)
        {
            //this.gameObject.transform.GetChild(0).transform.position = new Vector3(-10f, this.gameObject.transform.GetChild(0).transform.position.y, this.gameObject.transform.GetChild(0).transform.position.z);
            //this.gameObject.transform.GetChild(0).transform.LookAt(vectortofollowcamera, Vector3.up);
            if (this.gameObject.transform.GetChild(0).transform.localPosition.x + actx < -5f)
            {
                this.gameObject.transform.GetChild(0).transform.localPosition = new Vector3(this.gameObject.transform.GetChild(0).transform.localPosition.x + actx, this.gameObject.transform.GetChild(0).transform.localPosition.y, this.gameObject.transform.GetChild(0).transform.localPosition.z);
                actx += 0.0004f;
            }

        }
        if (this.gameObject.transform.GetChild(0).transform.localPosition.y - acty > 1)
        {
            this.gameObject.transform.GetChild(0).transform.localPosition = new Vector3(this.gameObject.transform.GetChild(0).transform.localPosition.x, this.gameObject.transform.GetChild(0).transform.localPosition.y-acty, this.gameObject.transform.GetChild(0).transform.localPosition.z);
            this.gameObject.transform.GetChild(0).transform.LookAt(this.transform, Vector3.up);
            if (this.gameObject.transform.GetChild(0).transform.localPosition.y - acty > 9)
            {
                acty += 0.0005f;
            }
            else
            {
                acty += 0.000000025f;
              
            }
          
           
        }
        else
        {


            this.gameObject.transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(this.destination.x, this.destination.y + 90, this.destination.z));
        }
        if (dist<6 && dist >4)
        {
            Time.timeScale = 0.5f;
          
         
           
        }
        else if (dist < 4 && dist>2)
        {
            if (!israydone)
            {
                GameObject.Find("Robot_Body").GetComponent<Robot>().bullettododge = this;

                GameObject.Find("Robot_Body").GetComponent<Robot>().DoMovement = true;
                //GameObject.Find("Robot_Body").GetComponent<Robot>().MovementToDodge(this);
                israydone = true;
            }
        }
        else if (dist <= 2)
        {
            Time.timeScale = 2f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        ray = new Ray(mPrevPos, (transform.position - mPrevPos).normalized);
        //if (isfired)
        //{
            raystart = new Ray(mPrevPos,(transform.position - mPrevPos)*int.MaxValue);
        //}



        //isfired = false;
        //Raycasthits();

        //var robotcolliders = robot.GetComponentsInChildren<Collider>();
        //if (!ishittedrobot)
        //{
        //    for (var index = 0; index < robotcolliders.Length; index++)
        //    {
        //        var colliderItem = robotcolliders[index];
        //        if (!(colliderItem.name == "Robot_Body"))
        //        {
        //            //if (this.GetComponent<SphereCollider>().bounds.Intersects(colliderItem.bounds))
        //            //{
        //               if (this.GetComponent<SphereCollider>().isTrigger)
        //                {

        //                    float dist1 = Vector3.Distance(this.GetComponent<SphereCollider>().transform.position, colliderItem.ClosestPointOnBounds(this.GetComponent<SphereCollider>().transform.position));
        //                if (dist1*3 < this.GetComponent<SphereCollider>().radius/6)
        //                {



        //                Debug.Log($"Bullet ID: {this_ID} Vector:{destination} Length: {destination.magnitude} Hit => {colliderItem.gameObject.name}");
        //                ishittedrobot = true;


        //                }
        //            }
        //        }

        //    }
        //}
        //Debug.Log($"ID: {this_ID } Startpos: {startingPos}  RayDirection:{ray.direction} MPrepos:{mPrevPos}  transformPosition: {transform.position}");
        bulletRay.transform.LookAt((transform.position - mPrevPos) * int.MaxValue);
        bulletRay.transform.Rotate(90, 0, 0);
        bulletRay.transform.localScale += new Vector3(0, 200, 0);
        Debug.DrawRay(startingPos, ray.direction * ((transform.position - mPrevPos).magnitude) * int.MaxValue, Color.red);
        Debug.DrawRay(startingPos, raystart.direction * ((transform.position - mPrevPos).magnitude) * int.MaxValue, Color.green);

        //Debug.DrawLine(startingPos, robot.transform.position, Color.green);
    }

    public void Raycasthits()
    {
        string hitname = "";
        hits = Physics.SphereCastAll(raystart, GetComponent<SphereCollider>().radius, (transform.position - mPrevPos).magnitude * int.MaxValue);
        if (ishit)
        {
            //Debug.Log($"{ this_ID} HITS:{hits.Length}");
            int i = 0;
            for (i = 0; i < hits.Length; i++)
            {
                hitname = hits[i].collider.gameObject.name;
                //Debug.Log(hitname);
                //if (IsRobothitandLog(hitname))
                //{


                //    //Debug.Log($"Bullet ID: {this_ID} Vector:{destination * mSpeed} Hit => {hitname}");
                //    Debug.Log($"Bullet ID: {this_ID} Vector:{destination} Length: {destination.magnitude} Hit => {hitname}");

                //    Debug.DrawRay(startingPos, raystart.direction * ((transform.position - mPrevPos).magnitude) * int.MaxValue, Color.green);
                //    //Debug.Log($"Bullet ID: {this_ID} Vector:{ray} Hit => {hitname}");
                //    //CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, .1f);

                //    isrobothitted = true;
                //    isreallyrobothitted = true;
                //}

            }



            if (!isrobothitted)
            {
                Debug.Log($"Bullet ID: {this_ID} Vector:{destination} Length: {destination.magnitude} Hit =>NONE");

                isrobothitted = true;
                isreallyrobothitted = false;


            }

            ishit = false;



        }
    }

    private void UpdateBulletBasic()
    {
    
        mPrevPos = transform.position;
        string hitname = "";
        //transform.Translate(mSpeed * Time.deltaTime,0.0f, 0.0f); 
        transform.Translate(destination * Time.deltaTime * mSpeed);

        ray = new Ray(mPrevPos, (transform.position - mPrevPos).normalized);
        RaycastHit[] hits = Physics.SphereCastAll(ray, GetComponent<SphereCollider>().radius, (transform.position - mPrevPos).magnitude);
        if (ishit)
        {
            //Debug.Log($"{ this_ID} HITS:{hits.Length}");
            for (int i = 0; i < hits.Length; i++)
            {
                hitname = hits[i].collider.gameObject.name;
                if (IsRobothitandLog(hitname))
                {

                    Debug.Log($"Bullet ID: {this_ID} Vector:{ray} Hit => {hitname}");
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
        Debug.DrawRay(startingPos, ray.direction * ((transform.position - mPrevPos).magnitude) * int.MaxValue, Color.red);
        Debug.DrawRay(startingPos, ray.direction * ((transform.position - mPrevPos).magnitude) * int.MaxValue, Color.green);

        //Debug.DrawLine(startingPos, robot.transform.position, Color.green);
    }

    public void RAYCAST()
    {
        RaycastHit[] hitsa = Physics.SphereCastAll(raystart, GetComponent<SphereCollider>().radius, (transform.position - mPrevPos).magnitude * int.MaxValue);
        //Debug.Log(hits);
        GameObject.Find("Robot_Body").GetComponent<Robot>().actbulletthits = hitsa;
    }

    private bool IsRobothitandLog(string name)
    {
        
        bool ishit = false;
        if (name != "Robot_Body")
        {
      
            foreach (BoxCollider item in GameObject.Find("Robot_Body").GetComponentsInChildren<BoxCollider>())
            {
               
                if (name == item.name)
                {
                    ishit = true;
                }
            }
        }
       

        return ishit;

    }
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(transform.position, transform.GetComponent<SphereCollider>().radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Robot_Body")
        {
            Debug.Log($"Bullet ID: {this_ID} Vector:{destination * mSpeed} Hit => {other.gameObject.name}");
        }
              
            
           
    }

    //private void OnParticleCollision(GameObject other)
    //{
    //    Debug.Log(other);
    //}


}

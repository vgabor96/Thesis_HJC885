﻿using System.Collections;
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
    private Ray raystart;
    public bool isfired;
    public bool isrobothitted;
    public bool isreallyrobothitted;


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
        this.this_ID = 0;
        this.ishit = true;
        //setting the bullet's hitbox!!
        GetComponent<SphereCollider>().radius *= transform.localScale.x ;

     
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
        string hitname = "";
      
        //transform.Translate(mSpeed * Time.deltaTime,0.0f, 0.0f); 
        transform.Translate(destination * Time.deltaTime * mSpeed);

        ray = new Ray(mPrevPos, (transform.position - mPrevPos).normalized);
        //if (isfired)
        //{
            raystart = new Ray(mPrevPos,(transform.position - mPrevPos)*int.MaxValue);
        //}
        //isfired = false;
        RaycastHit[] hits = Physics.SphereCastAll(raystart, GetComponent<SphereCollider>().radius, (transform.position - mPrevPos).magnitude * int.MaxValue);
        if (ishit)
        {     
            //Debug.Log($"{ this_ID} HITS:{hits.Length}");
            int i = 0;
            for (i = 0; i < hits.Length; i++)
            {
                hitname = hits[i].collider.gameObject.name;
                if (IsRobothitandLog(hitname))
                {
                  

                    //Debug.Log($"Bullet ID: {this_ID} Vector:{destination * mSpeed} Hit => {hitname}");
                    Debug.Log($"Bullet ID: {this_ID} Vector:{destination * mSpeed} Length: {destination.magnitude} Hit => {hitname}");
                
                    Debug.DrawRay(startingPos, raystart.direction * ((transform.position - mPrevPos).magnitude) * int.MaxValue, Color.green);
                    //Debug.Log($"Bullet ID: {this_ID} Vector:{ray} Hit => {hitname}");
                    //CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, .1f);
                    
                    isrobothitted = true;
                    isreallyrobothitted = true;
                }

            }
         


            if (!isrobothitted)
            {
                Debug.Log($"Bullet ID: {this_ID} Vector:{destination * mSpeed} Hit =>NONE");
             
                isrobothitted = true;
                isreallyrobothitted = false;
              

            }

            ishit = false;

          

        }

        //Debug.Log($"ID: {this_ID } Startpos: {startingPos}  RayDirection:{ray.direction} MPrepos:{mPrevPos}  transformPosition: {transform.position}");
        //Debug.DrawRay(startingPos, ray.direction * ((transform.position - mPrevPos).magnitude) * int.MaxValue, Color.red);
        Debug.DrawRay(startingPos, raystart.direction * ((transform.position - mPrevPos).magnitude) * int.MaxValue, Color.green);

        //Debug.DrawLine(startingPos, robot.transform.position, Color.green);
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


}

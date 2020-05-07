﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Robot : MonoBehaviour
{
    // Start is called before the first frame update
    public float radius;
    private Vector3 startpos;
    private List<Vector3> childrenstartpos;
    

    public bool DoMovement { get; set; }
    public bool DoReset { get; set; }

    public bool DoResetAfter { get; set; }


    // private GameObject body = GameObject.Find("Robot_Body");

    void Start()
    {
        childrenstartpos = new List<Vector3>();
        startpos = transform.localPosition;
        foreach (Transform item in this.transform)
        {
            childrenstartpos.Add(item.localPosition);
            //Debug.Log(item.localRotation);
        }

        //InvokeRepeating(nameof(RandomMovement), 0, 0.5f);
       
    }

    // Update is called once per frame
    void Update()
    {
        DoMovement |= Input.GetKeyDown(KeyCode.W);

        DoReset |= Input.GetKeyDown(KeyCode.R);
        if (DoReset)
        {
            Debug.Log("RESET");
            Reset();
            DoReset = false;

        }
        if (DoMovement)
        {
            Debug.Log("mooooooove");
            RandomMovement();
            DoMovement = false;
            //Time.timeScale = 1;
        }


    }


    private Vector3 RandomMovement_Vector3()
    {
        // float next_x = Random.Range(-radius, radius);
        //float next_z = Random.Range(-radius, radius);
        float next_x = Random.Range(0,2) == 0 ? radius/5 : -radius/5;
        float next_z = Random.Range(0, 2) == 0 ? radius / 5 : -radius / 5;
        Vector3 newpos = new Vector3(next_x, 0,next_z);
        float distance = Vector3.Distance(this.startpos,this.transform.localPosition + newpos/* * Time.deltaTime*/);
       
        if (distance <radius)
        {
            Debug.Log("TAV: " + distance + "Radius: " + radius);
            //Debug.LogError(startpos+" "+newpos+" "+distance);
            return newpos;
        }

        return new Vector3(0, 0, 0);
    }

    private void RandomMovement_Repeating()
    {
       
        transform.Translate(RandomMovement_Vector3() * Time.deltaTime);
    }

    public void RandomMovement()
    {
        //transform.Translate(new Vector3(0, 0, 4));//RandomMovement_Vector3() /* * Time.deltaTime*/);
        MoveFullBody(RandomMovement_Vector3());
       //MoveHead(RandomMovement_Vector3());
     //   RotateHead(RandomMovement_Vector3());
      //  MoveBody(RandomMovement_Vector3());
     //   RotateBody(RandomMovement_Vector3());
       //   MoveLeg(RandomMovement_Vector3());
        //  RotateLeg(RandomMovement_Vector3());


    }

    public void MoveFullBody(Vector3 vector)
    {
        if (AcceptedMoveFullBodyVector(vector))
        {
            transform.Translate(vector);
        }
    
    }

    private bool AcceptedMoveFullBodyVector(Vector3 newvector)
    {
        return Vector3.Distance(this.startpos, transform.localPosition + newvector) < radius;
        
    }

    public void MoveHead(Vector3 vector)
    {
        this.gameObject.transform.Find("Head").gameObject.transform.Translate(vector);
        //this.gameObject.transform.Find("Head").GetComponent<Rigidbody>().MovePosition(vector);
    }
    public void RotateHead(Vector3 vector)
    {
        this.gameObject.transform.Find("Head").gameObject.transform.Rotate(vector);
    }
    public void MoveBody(Vector3 vector)
    {
        this.gameObject.transform.Find("Body").gameObject.transform.Translate(vector);
    }
    public void RotateBody(Vector3 vector)
    {
        this.gameObject.transform.Find("Body").gameObject.transform.Rotate(vector);
    }
    public void MoveLeg(Vector3 vector)
    {
        this.gameObject.transform.Find("Legs").gameObject.transform.Translate(vector);
    }
    public void RotateLeg(Vector3 vector)
    {
        this.gameObject.transform.Find("Legs").gameObject.transform.Rotate(vector);
    }
    public void Reset()
    {
        //Debug.LogError("Robot position reseted");
        this.transform.localPosition = startpos;
        this.transform.rotation = new Quaternion(0, 0, 0, 0);
        int i = 0;
        foreach (Transform item in this.transform)
        {

            item.transform.localPosition = childrenstartpos[i];
            item.transform.rotation = new Quaternion(0, 0, 0, 0);
            i++;
        }
        //this.transform.position = startpos;
    }
}


﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static Utils;

public class Robot : MonoBehaviour
{
    // Start is called before the first frame update
    public float radius;
    private Vector3 startpos;
    private float headradius;
    private Dictionary<string,Vector3> childrenstartpos;
    private Dictionary<string,Transform> childrenobjects;
    private GameObject body;
    public RaycastHit[] actbulletthits;
    public GeneticAlgorithm_ForMovement GA;
    public Bullet_Movement_Script bullettododge;
    private float movementcostmultiplier=100;

    private List<GameObject> ghostbodyparts = new List<GameObject>();
    //private Dictionary<string,BoxCollider> ghostbodyparts = new Dictionary<string, BoxCollider>();



    public bool DoMovement { get; set; }
    public bool DoReset { get; set; }

    public bool DoResetAfter { get; set; }

    public bool IsLearning;

    //How much energy the movements cost
    public float MovementEnergyUsed { get; set; }


    // private GameObject body = GameObject.Find("Robot_Body");

    void Start()
    {
        MovementEnergyUsed = 0;
        childrenstartpos = new Dictionary<string, Vector3>();
        childrenobjects = new Dictionary<string, Transform>();


       // ghostbodyparts = new Dictionary<string, BoxCollider>();

        startpos = transform.localPosition;
        foreach (Transform item in this.transform)
        {
            //childrenstartpos.Add(item.localPosition);
            childrenstartpos.Add(item.name, item.transform.localPosition);
            childrenobjects.Add(item.name, item);
            
           
            //Debug.Log(item.localRotation);
        }

     //   GetGhostBodyparts();
        //Half of width
        headradius = childrenobjects["Head"].transform.localScale.x / 2;




        //Debug.Log(headradius);

        //InvokeRepeating(nameof(RandomMovement), 0, 0.5f);

    }

    //private void GetGhostBodyparts()
    //{
    //    BoxCollider head = gameObject.AddComponent<BoxCollider>();
    //    head.size = childrenobjects["Head"].GetComponent<BoxCollider>().size;
    //    head.center = childrenobjects["Head"].GetComponent<BoxCollider>().center;
    //    head.transform.localPosition = childrenobjects["Head"].GetComponent<BoxCollider>().transform.localPosition;
    //    head.transform.rotation = childrenobjects["Head"].GetComponent<BoxCollider>().transform.rotation;
    //    head.isTrigger = true;
    //    head.enabled = true;
    //    head.name = "ghosthead";

    //    BoxCollider body = gameObject.AddComponent<BoxCollider>();

    //    body.size = childrenobjects["Body"].GetComponent<BoxCollider>().size;
    //    body.center = childrenobjects["Body"].GetComponent<BoxCollider>().center;
    //    body.transform.localPosition = childrenobjects["Body"].GetComponent<BoxCollider>().transform.localPosition;
    //    body.transform.rotation = childrenobjects["Body"].GetComponent<BoxCollider>().transform.rotation;
    //    body.isTrigger = true;
    //    body.enabled = true;
    //    body.name = "ghostbody";

    //    BoxCollider leg = gameObject.AddComponent<BoxCollider>();

    //    leg.size = childrenobjects["Legs"].GetComponent<BoxCollider>().size;
    //    leg.center = childrenobjects["Legs"].GetComponent<BoxCollider>().center;
    //    leg.transform.localPosition = childrenobjects["Legs"].GetComponent<BoxCollider>().transform.localPosition;
    //    leg.transform.rotation = childrenobjects["Legs"].GetComponent<BoxCollider>().transform.rotation;
    //    leg.isTrigger = true;
    //    leg.enabled = true;
    //    leg.name = "ghostleggs";
    //    this.ghostbodyparts.Add("Head", head);
    //    this.ghostbodyparts.Add("Body", body);
    //    this.ghostbodyparts.Add("Legs", leg);
    //}

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
            //RandomMovement();
            MovementToDodge(GameObject.Find("BulletGenerator").GetComponent<Bullet_Shooter_Script>().Bullet.destination);
            DoMovement = false;
            //Time.timeScale = 1;
        }


    }


    private Vector3 RandomMovement_Vector3(Transform bodypart)
    {
        float next_x = 0;
        float next_z = 0;
        Vector3 newpos;
        if (bodypart == this.transform)
        {
            next_x = UnityEngine.Random.Range(0, 2) == 0 ? radius / 5 : -radius / 5;
            next_z = UnityEngine.Random.Range(0, 2) == 0 ? radius / 5 : -radius / 5;
        }
        else if (bodypart == childrenobjects["Head"])
        {
            next_x = UnityEngine.Random.Range(0, 2) == 0 ? headradius / 5 : -headradius / 5;
            next_z = UnityEngine.Random.Range(0, 2) == 0 ? headradius / 5 : -headradius / 5;       
        }
        else if (bodypart == childrenobjects["Body"])
        {
            next_x = UnityEngine.Random.Range(0, 2) == 0 ? headradius / 5 : -headradius / 5;
            next_z = UnityEngine.Random.Range(0, 2) == 0 ? headradius / 5 : -headradius / 5;
        }
        else if (bodypart == childrenobjects["Legs"])
        {
            next_x = UnityEngine.Random.Range(0, 2) == 0 ? headradius / 5 : -headradius / 5;
            next_z = UnityEngine.Random.Range(0, 2) == 0 ? headradius / 5 : -headradius / 5;
        }


        newpos = new Vector3(next_x, 0, next_z);
        return newpos;
    }

    //private void RandomMovement_Repeating()
    //{
       
    //    transform.Translate(RandomMovement_Vector3() * Time.deltaTime);
    //}

    public void RandomMovement()
    {

        //transform.Translate(new Vector3(0, 0, 4));//RandomMovement_Vector3() /* * Time.deltaTime*/);
        //MoveFullBody(new Vector3(0,0,0.4f));
        //MoveHead(new Vector3(0, 0, 0.4f));
        //MoveHead(new Vector3(0,0.2f,0));
        //   RotateHead(RandomMovement_Vector3());
        //  MoveBody(RandomMovement_Vector3());
        //   RotateBody(RandomMovement_Vector3());
        //   MoveLeg(RandomMovement_Vector3());
        //  RotateLeg(RandomMovement_Vector3());
        //List<Vector3> onemovement = new List<Vector3>() { new Vector3(1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1) };

        //objective(onemovement);

        //Reset();

        //List<Vector3> onemovement2 = new List<Vector3>() { new Vector3(0, 1, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) };

        //objective(onemovement2);

       
      


        // Debug.Log("Energy Used: " + MovementEnergyUsed);
    }

    public void MovementToDodge(Vector3 bullet)
    {
        if (IsLearning)
        {
            DoOneMovement(GA.StartFindingMovement(bullet,objective));
        }
    }

    public void MoveFullBody(Vector3 vector)
    {
        //if (AcceptedMoveFullBodyVector(vector))
        //{
        //transform.Translate(vector);
        MoveHead(vector);
        MoveBody(vector);
        MoveLeg(vector);

        //}
    
    }

    private bool AcceptedMoveFullBodyVector(Vector3 newvector)
    {
        return Vector3.Distance(this.startpos, transform.localPosition + newvector) < radius;
        
    }

    public void DoOneMovement(List<Vector3> movementcomponents)
    {
        MoveHead(movementcomponents[0]);
        RotateHead(movementcomponents[1]);
        MoveBody(movementcomponents[2]);
        RotateBody(movementcomponents[3]);
        MoveLeg(movementcomponents[4]);
        RotateLeg(movementcomponents[5]);
    }

    public void MoveHead(Vector3 vector)
    {
        //if (AcceptedMoveHeadvector(vector))
        //{
            childrenobjects["Head"].transform.Translate(vector);
            //ghostbodyparts["Head"].transform.Translate(vector);
            MovementEnergyUsed += vector.magnitude* movementcostmultiplier;
          
        //}
     
        //this.gameObject.transform.Find("Head").GetComponent<Rigidbody>().MovePosition(vector);
    }

    private bool AcceptedMoveHeadvector(Vector3 vector3)
    {
        return Vector3.Distance(this.childrenstartpos["Head"], childrenobjects["Head"].transform.localPosition + vector3) < headradius;
    }
    public void RotateHead(Vector3 vector)
    {
        this.gameObject.transform.Find("Head").gameObject.transform.Rotate(vector);
        //ghostbodyparts["Head"].transform.Rotate(vector);
        MovementEnergyUsed += vector.magnitude;
    }
    public void MoveBody(Vector3 vector)
    {
        this.gameObject.transform.Find("Body").gameObject.transform.Translate(vector);
        //ghostbodyparts["Body"].transform.Translate(vector);
        MovementEnergyUsed += vector.magnitude * movementcostmultiplier;
    }
    public void RotateBody(Vector3 vector)
    {
        this.gameObject.transform.Find("Body").gameObject.transform.Rotate(vector);
        //ghostbodyparts["Head"].transform.Rotate(vector);
        MovementEnergyUsed += vector.magnitude;
    }
    public void MoveLeg(Vector3 vector)
    {
        this.gameObject.transform.Find("Legs").gameObject.transform.Translate(vector);
        //ghostbodyparts["Legs"].transform.Translate(vector);
        MovementEnergyUsed += vector.magnitude * movementcostmultiplier;
    }
    public void RotateLeg(Vector3 vector)
    {
        this.gameObject.transform.Find("Legs").gameObject.transform.Rotate(vector);
        //ghostbodyparts["Legs"].transform.Rotate(vector);
        MovementEnergyUsed += vector.magnitude;
    }

    public double objective(List<Vector3> solution)
    {
        //Time.timeScale = 0;   
        RaycastHit[] hits1 = Physics.SphereCastAll(bullettododge.raystart, bullettododge.GetComponent<SphereCollider>().radius, (bullettododge.transform.position - bullettododge.mPrevPos).magnitude * int.MaxValue);
        DoOneMovement(solution);
       
       // GameObject.Find("BulletGenerator").GetComponent<Bullet_Shooter_Script>().RayCastBullet();
      
        double sum = 0;

        //foreach (RaycastHit item in actbulletthits)
        //{

        //    if (childrenobjects.FirstOrDefault(x=>x.Key == item.collider.gameObject.name).Value != null)
        //    {
        //        double dist1 = Vector3.Distance(childrenobjects[item.collider.gameObject.name].GetComponent<BoxCollider>().ClosestPoint(item.point), item.point);
        //        double dist2 = GameObject.Find("BulletGenerator").GetComponent<Bullet_Shooter_Script>().actualbulletsize;
        //        if (!(dist1> dist2))
        //        {
        //            sum += (1/Vector3.Distance(childrenobjects[item.collider.gameObject.name].position, item.point)) *1000000;
        //        }


        //    }
        //}
        //MeshFilter mf = GetComponent<MeshFilter>();
        //if (mf && mf.sharedMesh)
        //{
        //    Bounds bounds = mf.sharedMesh.bounds;
        //    BoxCollider collider = mf.gameObject.AddComponent<BoxCollider>();
        //    collider.center = bounds.center;
        //    collider.size = bounds.size;
        //}



        //GameObject ghosthead = Instantiate(childrenobjects["Head"].gameObject);

        //ghosthead.GetComponent<BoxCollider>().name = "ghosthead";
        //GameObject ghostbody = Instantiate(childrenobjects["Body"].gameObject);

        //ghostbody.GetComponent<BoxCollider>().name = "ghostbody";
        //GameObject ghostlegs = Instantiate(childrenobjects["Legs"].gameObject);

        //ghostlegs.GetComponent<BoxCollider>().name = "ghostlegs";

        //BoxCollider box = gameObject.AddComponent<BoxCollider>();
        //box.size = childrenobjects["Head"].GetComponent<BoxCollider>().size;
        //box.center = childrenobjects["Head"].GetComponent<BoxCollider>().center;
        //box.transform.position = childrenobjects["Head"].GetComponent<BoxCollider>().transform.position;
        //box.transform.rotation = childrenobjects["Head"].GetComponent<BoxCollider>().transform.rotation;
        //box.isTrigger = true;
        //box.enabled = true;
        //box.name = "ghosthead";

        //BoxCollider box1 = gameObject.AddComponent<BoxCollider>();
        //box1.size = childrenobjects["Body"].GetComponent<BoxCollider>().size;
        //box1.center = childrenobjects["Body"].GetComponent<BoxCollider>().center;
        //box1.transform.position = childrenobjects["Body"].GetComponent<BoxCollider>().transform.position;
        //box1.transform.rotation = childrenobjects["Body"].GetComponent<BoxCollider>().transform.rotation;
        //box1.isTrigger = true;
        //box1.enabled = true;
        //box1.name = "ghostbody";

        //BoxCollider box2 = gameObject.AddComponent<BoxCollider>();
        //box2.size = childrenobjects["Legs"].GetComponent<BoxCollider>().size;
        //box2.center = childrenobjects["Legs"].GetComponent<BoxCollider>().center;
        //box2.transform.position = childrenobjects["Legs"].GetComponent<BoxCollider>().transform.position;
        //box2.transform.rotation = childrenobjects["Legs"].GetComponent<BoxCollider>().transform.rotation;
        //box2.isTrigger = true;
        //box2.enabled = true;
        //box2.name = "ghostleg";
        //Utils.GetCopyOf(box, childrenobjects["Head"].GetComponent<BoxCollider>());


        RaycastHit[] hits = Physics.SphereCastAll(bullettododge.raystart, bullettododge.GetComponent<SphereCollider>().radius, (bullettododge.transform.position - bullettododge.mPrevPos).magnitude * int.MaxValue);

        bool isa = hits.Equals(hits1);


        //foreach (RaycastHit item in hits)
        //{

        //    if (childrenobjects.FirstOrDefault(x => x.Key == item.collider.gameObject.name).Value != null)
        //    {
        //        foreach (Transform bodypart in this.childrenobjects.Values)
        //        {
        //            if (bodypart.GetComponent<BoxCollider>() != null)
        //            {

        //                double dist1 = Vector3.Distance(bodypart.GetComponent<BoxCollider>().ClosestPoint(item.point), item.point);
        //                double dist2 = GameObject.Find("BulletGenerator").GetComponent<Bullet_Shooter_Script>().actualbulletsize*2;
        //                if (!(dist1 > dist2))
        //                {
        //                    // sum += (1 / Vector3.Distance(childrenobjects[item.collider.gameObject.name].position, item.point)) * 1000000;
        //                    sum += 1000000;
        //                }
        //                //double dist1 = Vector3.Distance(childrenobjects[item2.collider.gameObject.name].GetComponent<BoxCollider>().ClosestPoint(item.point), item.point);
        //                //double dist2 = GameObject.Find("BulletGenerator").GetComponent<Bullet_Shooter_Script>().actualbulletsize * 2;
        //                //if (!(dist1 > dist2))
        //                //{
        //                //    // sum += (1 / Vector3.Distance(childrenobjects[item.collider.gameObject.name].position, item.point)) * 1000000;
        //                //    sum += 1000000;
        //                //}
        //            }

        //        }



        //    }
        //    //if (item.collider.name == "ghosthead")
        //    //{
        //    //    sum += 1000000;
        //    //}
        //    //if (item.collider.name == "ghostbody")
        //    //{
        //    //    sum += 1000000;
        //    //}
        //    //if (item.collider.name == "ghostleg")
        //    //{
        //    //    sum += 1000000;
        //    //}
        //}


       
        foreach (Transform bodypart in childrenobjects.Values)
        {
            if (bodypart.GetComponent<BoxCollider>() != null)
            {
                float dist1 = DistanceToRay(bullettododge.raystart, bodypart.GetComponent<BoxCollider>().transform.position);
                if (dist1 < (GameObject.Find("BulletGenerator").GetComponent<Bullet_Shooter_Script>().actualbulletsize*0.75 + (bodypart.GetComponent<BoxCollider>().size.x*0.75)))
                {
                    sum += (1 / dist1) * 100000;
                }
            }

          
        }

        //Destroy(ghosthead);
        //Destroy(ghostbody);
        //Destroy(ghostlegs);



        double temp = MovementEnergyUsed;

        Reset();
        Debug.Log("Sum: "+ sum +" Energy: "+temp+" => Fitness: "+ (sum+temp));
        //Time.timeScale = 1;
        return sum + temp;
    }

    public static float DistanceToRay(Ray ray, Vector3 point)
    {
        return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
    }
    public void Reset()
    {
        //Debug.LogError("Robot position reseted");
        this.transform.localPosition = startpos;
        this.transform.rotation = new Quaternion(0, 0, 0, 0);
        int i = 0;
        foreach (Transform item in this.transform)
        {

            item.transform.localPosition = childrenstartpos[item.name];
            item.transform.rotation = new Quaternion(0, 0, 0, 0);
            i++;
        }



        //foreach (string item in ghostbodyparts.Keys)
        //{
        //    ghostbodyparts[item].size = childrenobjects[item].GetComponent<BoxCollider>().size;
        //    ghostbodyparts[item].center = childrenobjects[item].GetComponent<BoxCollider>().center;
        //    ghostbodyparts[item].transform.localPosition = childrenobjects[item].GetComponent<BoxCollider>().transform.localPosition;
        //    ghostbodyparts[item].transform.rotation = childrenobjects[item].GetComponent<BoxCollider>().transform.rotation;

        //}
        MovementEnergyUsed = 0;
        //this.transform.position = startpos;
    }


}


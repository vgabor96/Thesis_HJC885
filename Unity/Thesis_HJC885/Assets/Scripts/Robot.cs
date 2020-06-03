using System;
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
    private float movementcostmultiplier=120;
    double sum = 0;

    public double actmovfitness = 0;

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

         sum = 0;
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
            //Debug.Log("RESET");
            Reset();
            DoReset = false;

        }
        if (DoMovement)
        {
            //Debug.Log("mooooooove");
            //RandomMovement();
            DoOneMovement(MovementToDodge(GameObject.Find("BulletGenerator").GetComponent<Bullet_Shooter_Script>().Bullet));
            //bullettododge.Raycasthits();
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
        //MoveFullBody(new Vector3(0,0,0.5f));
        //MoveHead(new Vector3(0, 0, 0.5f));

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

    public List<Vector3> MovementToDodge(Bullet_Movement_Script bullet)
    {
        this.bullettododge = bullet;
        if (IsLearning)
        {


            //DoOneMovement(GA.StartFindingMovement(bullet.destination, objective));
           return GA.StartFindingMovement(bullet.destination, objective);
        }
        return new List<Vector3>();
    }

    public void MoveFullBody(Vector3 vector)
    {
        //if (AcceptedMoveFullBodyVector(vector))
        //{
        //transform.Translate(vector);
        MoveHead(vector);
        MoveBody(vector);
        MoveLeg(vector);
        MovementEnergyUsed += 50;
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
        MoveFullBody(movementcomponents[6]);
    }

    public void MoveHead(Vector3 vector)
    {
        //if (AcceptedMoveHeadvector(vector))
        //{
            childrenobjects["Head"].transform.Translate(vector);
        //childrenobjects["Head"].gameObject.GetComponent<Rigidbody>().MovePosition(childrenobjects["Head"].gameObject.GetComponent<Rigidbody>().position + vector);
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
        //childrenobjects["Head"].gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler( childrenobjects["Head"].gameObject.GetComponent<Rigidbody>().rotation.eulerAngles + vector));
        //ghostbodyparts["Head"].transform.Rotate(vector);
        MovementEnergyUsed += vector.magnitude;
    }
    public void MoveBody(Vector3 vector)
    {
        this.gameObject.transform.Find("Body").gameObject.transform.Translate(vector);
        //childrenobjects["Body"].gameObject.GetComponent<Rigidbody>().MovePosition(childrenobjects["Body"].gameObject.GetComponent<Rigidbody>().position + vector);
        //ghostbodyparts["Body"].transform.Translate(vector);
        MovementEnergyUsed += vector.magnitude * movementcostmultiplier;
    }
    public void RotateBody(Vector3 vector)
    {
        this.gameObject.transform.Find("Body").gameObject.transform.Rotate(vector);
        //childrenobjects["Body"].gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(childrenobjects["Body"].gameObject.GetComponent<Rigidbody>().rotation.eulerAngles + vector));
        //ghostbodyparts["Head"].transform.Rotate(vector);
        MovementEnergyUsed += vector.magnitude;
    }
    public void MoveLeg(Vector3 vector)
    {
        this.gameObject.transform.Find("Legs").gameObject.transform.Translate(vector);
        //childrenobjects["Legs"].gameObject.GetComponent<Rigidbody>().MovePosition(childrenobjects["Legs"].gameObject.GetComponent<Rigidbody>().position + vector);
        //ghostbodyparts["Legs"].transform.Translate(vector);
        MovementEnergyUsed += vector.magnitude * movementcostmultiplier;
    }
    public void RotateLeg(Vector3 vector)
    {
       this.gameObject.transform.Find("Legs").gameObject.transform.Rotate(vector);
        //childrenobjects["Legs"].gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(childrenobjects["Legs"].gameObject.GetComponent<Rigidbody>().rotation.eulerAngles + vector));
        //ghostbodyparts["Legs"].transform.Rotate(vector);
        MovementEnergyUsed += vector.magnitude;
    }

    public double objective(List<Vector3> solution)
    {
        //Time.timeScale = 0;   
        //RaycastHit[] hits1 = Physics.SphereCastAll(bullettododge.raystart, bullettododge.GetComponent<SphereCollider>().radius, (bullettododge.transform.position - bullettododge.mPrevPos).magnitude * int.MaxValue);

        DoOneMovement(solution);

        // GameObject.Find("BulletGenerator").GetComponent<Bullet_Shooter_Script>().RayCastBullet();

        Collider a = GameObject.Find("RayCollider").GetComponent<CapsuleCollider>();
        //a.transform.Translate(new Vector3(0,0,0.00001f));


        //RaycastHit[] hits1 = Physics.SphereCastAll(bullettododge.raystart, bullettododge.GetComponent<SphereCollider>().radius, (bullettododge.transform.position - bullettododge.mPrevPos).magnitude * int.MaxValue);

        foreach (Transform bodypart in childrenobjects.Values)
        {

            if (bodypart.GetComponent<BoxCollider>() != null)
            {


                //penalty for intersect
                Collider[] collisions = Physics.OverlapSphere(bodypart.position, bodypart.GetComponent<BoxCollider>().size.x*0.5f);//.50f);
                foreach (Collider C in collisions)
                {

                    if (C.transform.name != bodypart.name)
                    {
                        sum += 1000;
                    }
                    //if (C.transform.gameObject.name == "RayCollider")
                    //{
                    //    sum += 500111111;
                    //}


                }

                // penalty for moving to far
                Collider[] collisions2 = Physics.OverlapSphere(bodypart.position, bodypart.GetComponent<BoxCollider>().size.x);//.50f);

                if (collisions2.Length < 1)
                {
                    sum += 1000;
                }


                //double dist = Vector3.Distance(GameObject.Find("RayCollider").GetComponent<CapsuleCollider>().ClosestPoint(bodypart.transform.position), bodypart.transform.position);

                //if (dist < ((GameObject.Find("BulletGenerator").GetComponent<Bullet_Shooter_Script>().actualbulletsize) + bodypart.GetComponent<BoxCollider>().size.x / 2))
                //{
                //    sum += (1 / DistanceToRay(bullettododge.raystart, bodypart.GetComponent<BoxCollider>().transform.position)) * 100000;

                //}



                Vector3 vec = a.ClosestPoint(bodypart.GetComponent<BoxCollider>().transform.position);

                float dist1x = Math.Abs(vec.x - bodypart.GetComponent<BoxCollider>().transform.position.x);
                double tempsx = (bodypart.GetComponent<BoxCollider>().size.x * 0.5);
                double tempsumx = 0;
                if (dist1x <= tempsx)
                {
                    tempsumx = (1 / dist1x) * 3000;
                }

                float dist1y = Math.Abs(vec.y - bodypart.GetComponent<BoxCollider>().transform.position.y);
                double tempsy = (bodypart.GetComponent<BoxCollider>().size.y * 0.5);
                double tempsumy = 0;
                if (dist1y <= tempsy)
                {
                    tempsumy = (1 / dist1y) * 3000;
                }

                float dist1z = Math.Abs(vec.z- bodypart.GetComponent<BoxCollider>().transform.position.z);
                double tempsz = (bodypart.GetComponent<BoxCollider>().size.z * 0.5);
                double tempsumz = 0;
                if (dist1z <= tempsz)
                {
                    tempsumz = (1 / dist1z) * 3000;
                }

                if ((tempsumx> 0) && (tempsumy > 0) && (tempsumz>0))
                {
                    sum += tempsumx + tempsumy + tempsumz;
                }
               
                //GameObject.Find("RayCollider").GetComponent<SphereCollider>().

                //foreach (RaycastHit item in hits1)
                //{
                //    if (item.collider.name == bodypart.GetComponent<BoxCollider>().name)
                //    {
                //        float dist1 = DistanceToRay(bullettododge.raystart, bodypart.GetComponent<BoxCollider>().transform.position);
                //        if (dist1 < )
                //        {

                //        }
                //        sum += (1 / dist1) * 100000;
                //    }
                //}


            }

          
        }

       

      
        //Destroy(ghosthead);
        //Destroy(ghostbody);
        //Destroy(ghostlegs);


        //a.transform.Translate(new Vector3(0, 0, -0.00001f));
        double temp = MovementEnergyUsed;

        double sum1 = sum;
        Reset();
        //Debug.Log("Sum: "+ sum +" Energy: "+temp+" => Fitness: "+ (sum+temp));
        //Time.timeScale = 1;
        return sum1 + temp;
    }

    public static float DistanceToRay(Ray ray, Vector3 point)
    {
        return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
    }

    //private double DistanceFromBodypart(BoxCollider)
    //{

    //}
    public void Reset()
    {
        //Debug.LogError("Robot position reseted");
        this.transform.localPosition = startpos;

        //this.GetComponent<Rigidbody>().MovePosition(startpos);
        //this.GetComponent<Rigidbody>().MoveRotation(new Quaternion(0, 0, 0, 0));
        this.transform.rotation = new Quaternion(0, 0, 0, 0);
        int i = 0;
        foreach (Transform item in this.transform)
        {
            //if (item.GetComponent<Rigidbody>() !=null)
            //{
                item.transform.localPosition = childrenstartpos[item.name];
                item.transform.rotation = new Quaternion(0, 0, 0, 0);

                //item.GetComponent<Rigidbody>().MovePosition(childrenstartpos[item.name]);
                //item.GetComponent<Rigidbody>().MoveRotation(new Quaternion(0, 0, 0, 0).normalized);
                i++;
            //}
           
        }

        sum = 0;
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

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.name == "Bullet(Clone)")
        //{
        //    Bullet_Movement_Script b = other.gameObject.GetComponent<Bullet_Movement_Script>();
        //    Debug.Log($"Bullet ID: {b.this_ID} Vector:{b.destination * b.mSpeed} Hit => {other.gameObject.name}");
        //}

        if (other.gameObject.name == "RayCollider")
        {
            //Bullet_Movement_Script b = other.gameObject.GetComponent<Bullet_Movement_Script>();
            sum += 1000000000;
        }

    }


    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.name == "RayCollider")
        {
            //Bullet_Movement_Script b = other.gameObject.GetComponent<Bullet_Movement_Script>();
            sum = 0;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
}


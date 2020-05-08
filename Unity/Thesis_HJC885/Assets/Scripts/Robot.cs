using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Robot : MonoBehaviour
{
    // Start is called before the first frame update
    public float radius;
    private Vector3 startpos;
    private float headradius;
    private Dictionary<string,Vector3> childrenstartpos;
    private Dictionary<string,Transform> childrenobjects;
    private GameObject body;

    

    public bool DoMovement { get; set; }
    public bool DoReset { get; set; }

    public bool DoResetAfter { get; set; }

    //How much energy the movements cost
    public float MovementEnergyUsed { get; set; }


    // private GameObject body = GameObject.Find("Robot_Body");

    void Start()
    {
        MovementEnergyUsed = 0;
        childrenstartpos = new Dictionary<string, Vector3>();
        childrenobjects = new Dictionary<string, Transform>();
        
        startpos = transform.localPosition;
        foreach (Transform item in this.transform)
        {
            //childrenstartpos.Add(item.localPosition);
            childrenstartpos.Add(item.name, item.transform.localPosition);
            childrenobjects.Add(item.name, item);

            //Debug.Log(item.localRotation);
        }
        //Half of width
        headradius = childrenobjects["Head"].transform.localScale.x / 2;
        Debug.Log(headradius);
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


    private Vector3 RandomMovement_Vector3(Transform bodypart)
    {
        float next_x = 0;
        float next_z = 0;
        Vector3 newpos;
        if (bodypart == this.transform)
        {
            next_x = Random.Range(0, 2) == 0 ? radius / 5 : -radius / 5;
            next_z = Random.Range(0, 2) == 0 ? radius / 5 : -radius / 5;
        }
        else if (bodypart == childrenobjects["Head"])
        {
            next_x = Random.Range(0, 2) == 0 ? headradius / 5 : -headradius / 5;
            next_z = Random.Range(0, 2) == 0 ? headradius / 5 : -headradius / 5;       
        }
        else if (bodypart == childrenobjects["Body"])
        {
            next_x = Random.Range(0, 2) == 0 ? headradius / 5 : -headradius / 5;
            next_z = Random.Range(0, 2) == 0 ? headradius / 5 : -headradius / 5;
        }
        else if (bodypart == childrenobjects["Legs"])
        {
            next_x = Random.Range(0, 2) == 0 ? headradius / 5 : -headradius / 5;
            next_z = Random.Range(0, 2) == 0 ? headradius / 5 : -headradius / 5;
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
        //MoveFullBody(RandomMovement_Vector3());
       MoveHead(new Vector3(0,0.2f,0));
        //   RotateHead(RandomMovement_Vector3());
        //  MoveBody(RandomMovement_Vector3());
        //   RotateBody(RandomMovement_Vector3());
        //   MoveLeg(RandomMovement_Vector3());
        //  RotateLeg(RandomMovement_Vector3());

        Debug.Log("Energy Used: " + MovementEnergyUsed);
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
        //if (AcceptedMoveHeadvector(vector))
        //{
            childrenobjects["Head"].transform.Translate(vector);
            MovementEnergyUsed += vector.magnitude;
          
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
        MovementEnergyUsed += vector.magnitude;
    }
    public void MoveBody(Vector3 vector)
    {
        this.gameObject.transform.Find("Body").gameObject.transform.Translate(vector);
        MovementEnergyUsed += vector.magnitude;
    }
    public void RotateBody(Vector3 vector)
    {
        this.gameObject.transform.Find("Body").gameObject.transform.Rotate(vector);
        MovementEnergyUsed += vector.magnitude;
    }
    public void MoveLeg(Vector3 vector)
    {
        this.gameObject.transform.Find("Legs").gameObject.transform.Translate(vector);
        MovementEnergyUsed += vector.magnitude;
    }
    public void RotateLeg(Vector3 vector)
    {
        this.gameObject.transform.Find("Legs").gameObject.transform.Rotate(vector);
        MovementEnergyUsed += vector.magnitude;
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
        MovementEnergyUsed = 0;
        //this.transform.position = startpos;
    }
}


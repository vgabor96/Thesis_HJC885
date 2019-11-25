using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_MovementScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float radius;
    private Vector3 startpos;

    
    // private GameObject body = GameObject.Find("Robot_Body");

    void Start()
    {

        startpos = transform.position;
       
    }

    // Update is called once per frame
    void Update()
    {
        RandomMovement();
    }


    private void RandomMovement()
    {
        float next_x = Random.Range(-radius, radius);
        float next_z = Random.Range(-radius, radius);
        Vector3 newpos = new Vector3(next_x, 0,next_z);
        
        if (Vector3.Distance(this.startpos,this.transform.localPosition+newpos) <radius)
        {
           
            transform.Translate(newpos* Time.deltaTime);
        }
       
    }
}


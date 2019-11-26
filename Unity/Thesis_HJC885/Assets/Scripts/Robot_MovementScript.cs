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

        startpos = transform.localPosition;
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(RandomMovement()* Time.deltaTime);
    }


    private Vector3 RandomMovement()
    {
        // float next_x = Random.Range(-radius, radius);
        //float next_z = Random.Range(-radius, radius);
        float next_x = Random.Range(0,2) == 0 ? radius/2 : -radius/2;
        float next_z = Random.Range(0, 2) == 0 ? radius / 2 : -radius / 2;
        Vector3 newpos = new Vector3(next_x, 0,next_z);
        float distance = Vector3.Distance(this.startpos,this.transform.localPosition + newpos * Time.deltaTime);
        if (distance <radius)
        {
            //Debug.LogError(startpos+" "+newpos+" "+distance);
            return newpos;
        }
        return new Vector3(0, 0, 0);
    }
}


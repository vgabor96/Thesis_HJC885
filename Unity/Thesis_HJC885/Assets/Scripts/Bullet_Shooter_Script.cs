using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Shooter_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public int numberOfBullets = 10;
    public GameObject Bullet;
    
    void Start()
    {
        //Move the object to the same position as the parent:
        transform.localPosition = new Vector3(0, 90, 0);

        for (int i = 0; i < numberOfBullets; i++)
        {
            Instantiate(Bullet, new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z), transform.rotation);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

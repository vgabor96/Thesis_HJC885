using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Shooter_Script : MonoBehaviour
{
    // Start is called before the first frame update
    private int currentBullets = 0;
    public int numberOfBullets = 10;
    public GameObject Bullet;
    
    void Start()
    {
        //Move the object to the same position as the parent:
        transform.localPosition = new Vector3(0, 0, 0);

        GenerateBullets();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InstianteBullet()
    {
        if (currentBullets <= numberOfBullets)
        {
            Instantiate(Bullet, transform.position, transform.rotation);
            currentBullets++;
        }
       
    }

    private void GenerateBullets()
    {
            InvokeRepeating(nameof(InstianteBullet), 2, 0.5f);
               
       
    }
}

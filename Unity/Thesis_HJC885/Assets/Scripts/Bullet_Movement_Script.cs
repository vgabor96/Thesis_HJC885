using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Bullet_Movement_Script : MonoBehaviour
{
    private Vector3 startingPos;
    private Vector3 destination;
    public float ResetDistance = 1000.0f;
    public float mSpeed = 100.0f;
    private bool ishit = true;

    //public ParticleSystem explosion;

    public Vector3 min = new Vector3(-5,-1,-1);
    public Vector3 max = new Vector3(5, 1, 1);

    Vector3 mPrevPos;
    // Start is called before the first frame update
    void Start()
    {

        mPrevPos = transform.position;
        startingPos = mPrevPos;
        //transform.localPosition = new Vector3(0, 0, 0);
        destination = RandomDestinationGenerator(min,max);

    }

    // Update is called once per frame
    void Update()
    {

        mPrevPos = transform.position;

        //transform.Translate(mSpeed * Time.deltaTime,0.0f, 0.0f); 
        transform.Translate(destination*Time.deltaTime*mSpeed);
        RaycastHit[] hits = Physics.SphereCastAll(new Ray(mPrevPos, (transform.position - mPrevPos).normalized), GetComponent<SphereCollider>().radius, (transform.position - mPrevPos).magnitude);
        if (ishit)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.name =="Robot_Body")
                {
                  
                    Debug.Log(hits[i].collider.gameObject.name);
                    //GameObject.Find("BulletShooter_Camera").SendMessage("DoShake");
                   // explosion.Play();
                    CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, .1f);
                    ishit = false;
                }
               
            }
            
        }

        // Debug.DrawLine(transform.position, mPrevPos);

        if (Vector3.Distance(startingPos, mPrevPos) >= ResetDistance)
        {
            ReGenerate();
        }
    }

    private void ReGenerate()
    {
        transform.position = startingPos;
        ishit = true;
        destination = RandomDestinationGenerator(min,max);
    }

    private Vector3 RandomDestinationGenerator(Vector3 min, Vector3 max)
    {

        float x = UnityEngine.Random.Range(min.x, max.x);
        float y = UnityEngine.Random.Range(min.y, max.y);
        float z = UnityEngine.Random.Range(min.z, max.z);
        return new Vector3(x*mSpeed, y* mSpeed,z*mSpeed);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.gameObject.name);
    //}
}

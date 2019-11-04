using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Movement_Script : MonoBehaviour
{
    public Vector3 startingPos;
    public float ResetDistance = 1000.0f;
    public float mSpeed = 100.0f;
    private bool ishit = true;

    Vector3 mPrevPos;
    // Start is called before the first frame update
    void Start()
    {

        mPrevPos = transform.position;
        startingPos = mPrevPos;

    }

    // Update is called once per frame
    void Update()
    {

        mPrevPos = transform.position;
        transform.Translate(0.0f, 0.0f, mSpeed * Time.deltaTime);
        RaycastHit[] hits = Physics.SphereCastAll(new Ray(mPrevPos, (transform.position - mPrevPos).normalized), GetComponent<SphereCollider>().radius, (transform.position - mPrevPos).magnitude);
        if (ishit)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                Debug.Log(hits[i].collider.gameObject.name);
            }
            //ishit = false;
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
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.gameObject.name);
    //}
}

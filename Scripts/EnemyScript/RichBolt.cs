using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichBolt : MonoBehaviour
{
    public Vector3 target;
    Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.velocity  = transform.forward *10f;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Obstacle"))
        {
            Destroy(gameObject, 0.1f);
        }
    }
}

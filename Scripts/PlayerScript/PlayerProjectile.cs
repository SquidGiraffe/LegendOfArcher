using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    Rigidbody rigid;
    public GameObject target;
    public Vector3 targetPos;
    public float damage;

     void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

     void Move()
    {
        rigid.AddForce(transform.forward *2f, ForceMode.Impulse);

    }

    void Update()
    {
        Move();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Obstacle") || other.transform.CompareTag("Monster"))
        {
            rigid.velocity = Vector3.zero;
            Destroy(gameObject, 0.2f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Obstacle") || collision.transform.CompareTag("Monster"))
        {
            rigid.velocity = Vector3.zero;
            Destroy(gameObject, 0.2f);
        }
    }
}

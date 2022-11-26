using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    public Transform target;
    Vector3 moveVec;
    Vector3 tempVec;
    float z;
    public Vector3 offset;

     void Awake()
    {
        offset  = target.position;
    }
    void LateUpdate()
    {
        MoveToPlayer();
    }
    public void MoveToPlayer()
    {
        tempVec = target.position - offset;
        moveVec = new Vector3(tempVec.x, tempVec.y, tempVec.z);
        transform.position += moveVec;
        offset = target.position;

    }
}

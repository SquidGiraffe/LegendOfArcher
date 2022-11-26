using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerLine : MonoBehaviour
{
    TrailRenderer trail;
    public Vector3 EndPosition;

    void Start()    
    {
        trail = GetComponent<TrailRenderer>();
        Destroy(gameObject, 4f);
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, EndPosition, Time.deltaTime * 3f);
    }
}

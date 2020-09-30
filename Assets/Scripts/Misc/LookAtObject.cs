using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    [SerializeField] Transform targetObject;


    private void Start()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        transform.LookAt(targetObject);
    }
}

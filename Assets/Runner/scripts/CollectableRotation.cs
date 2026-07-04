using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CollectableRotation : MonoBehaviour
{
    public float rotateSpeed = 90f;

    void Update()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.World);
    }
}
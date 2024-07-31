using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RotateOnPoint : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.RotateAround(this.transform.position, Vector3.right, speed * Time.deltaTime);
    }
}

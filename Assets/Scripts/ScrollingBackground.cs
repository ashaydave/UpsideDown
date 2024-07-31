using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScrollingBackground : MonoBehaviour
{
    public float speed = 2f;
    private Vector3 startPosition;
    private float backgroundWidth;

    void Start()
    {
        startPosition = transform.position;
        backgroundWidth = GetComponent<Renderer>().bounds.size.x;
    }

    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * speed, backgroundWidth);
        transform.position = startPosition + Vector3.left * newPosition;

        if (transform.position.x < startPosition.x - backgroundWidth)
        {
            transform.position = new Vector3(startPosition.x, transform.position.y, transform.position.z);
        }
    }

    public void StopScrolling()
    {
        this.enabled = false;
    }
}

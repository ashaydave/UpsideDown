using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground3D : MonoBehaviour
{
    public PoolManager poolManager;
    public float speed = 5f;
    public float resetPosition = -50f;

    private float objectWidth; // Width of a single object
    private Vector3 initialPosition; // Initial position of the PoolManager

    public bool isScrolling = false;

    void Start()
    {
        // Calculate the width of a single object from the pool manager
        objectWidth = poolManager.objectWidth;
        initialPosition = poolManager.transform.position; // Store the initial position of the PoolManager
    }

    void Update()
    {
        if (!isScrolling) return;
        // Move all active objects
        foreach (GameObject obj in poolManager.pool)
        {
            if (obj.activeInHierarchy)
            {
                obj.transform.Translate(Vector3.left * speed * Time.deltaTime);

                // Check if the object goes off-screen
                if (obj.transform.position.x < resetPosition)
                {
                    // Calculate the new position in front of the moving direction
                    float newXPosition = initialPosition.x + objectWidth * poolManager.poolSize - Mathf.Abs(resetPosition - obj.transform.position.x);
                    poolManager.RecycleObject(obj, newXPosition);
                }
            }
        }
    }

    public void StartScrolling()
    {
        isScrolling = true;
    }

    public void StopScrolling()
    {
        isScrolling = false;
        PoolManager.Instance.StopPoolManager();
    }
}

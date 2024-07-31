using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    public GameObject prefab;
    public int poolSize = 5;
    public float objectWidth = 10.24f; // Width of prefab

    public List<GameObject> pool;
    private Vector3 startPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        pool = new List<GameObject>();
        startPosition = transform.position; // Use the position of the PoolManager as the starting point

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, startPosition + Vector3.right * i * objectWidth, Quaternion.identity);
            pool.Add(obj);
        }

        prefab.SetActive(false);
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // If all objects are active, instantiate a new one
        GameObject newObj = Instantiate(prefab);
        pool.Add(newObj);
        return newObj;
    }

    public void RecycleObject(GameObject obj, float newXPosition)
    {
        obj.transform.position = new Vector3(newXPosition, obj.transform.position.y, obj.transform.position.z);
        obj.SetActive(true);
    }

    public void StopPoolManager()
    {
        // Disable all objects in the pool
        foreach (GameObject obj in pool)
        {
           pool.Clear();
        }

        this.enabled = false;
    }

    
}

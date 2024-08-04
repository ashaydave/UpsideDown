using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] bikePrefab;
    [SerializeField] private float bikeSpawnRateMin = 2f;
    [SerializeField] private float bikeSpawnRateMax = 5f;

    private bool isSpawning = true;

    IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            Vector3 spawnHere = new Vector3(Random.Range(85, 100), -2.2f, -107.28f);
            Instantiate(bikePrefab[Random.Range(0, bikePrefab.Length)], spawnHere, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(bikeSpawnRateMin, bikeSpawnRateMax));
        }
    }

    public void StartSpawning()
    {
        isSpawning = true;
        StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }
}

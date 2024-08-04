using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] robotPrefab;
    [SerializeField] private float robotSpawnRateMin = 2f;
    [SerializeField] private float robotSpawnRateMax = 8f;

    private bool isSpawning = true;

    IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            Vector3 spawnHere = new Vector3(Random.Range(85, 100), 0.22f, -107.28f);
            Instantiate(robotPrefab[Random.Range(0, robotPrefab.Length)], spawnHere, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(robotSpawnRateMin, robotSpawnRateMax));
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

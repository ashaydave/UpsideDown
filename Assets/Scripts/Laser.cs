using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float timer = 5f;
    private float bulletTime;

    public GameObject enemyLaser;
    public Transform firePoint;

    void ShootAtPlayer()
    {
        bulletTime -= Time.deltaTime;

        if (bulletTime > 0) return;

        bulletTime = timer;

        GameObject laserObject = Instantiate(enemyLaser, firePoint.transform.position, firePoint.transform.rotation) as GameObject;
        Rigidbody laserRb = laserObject.GetComponent<Rigidbody>();
        laserRb.AddForce(firePoint.transform.right * 100, ForceMode.Impulse);
        Destroy(laserObject, 5f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public GameObject laserPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float nextFireTime = 0f;
    private AudioSource audioSource;

    private PlayerController player;

    [SerializeField] private float edgeLength = -12f; // Position where enemy should be destroyed

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = Object.FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        MoveTowardsPlayer();
        if (Time.time > nextFireTime)
        {
            // FireBullet();
            nextFireTime = Time.time + fireRate;
        }

        if (transform.position.x < edgeLength)
        {
            Destroy(gameObject);
        }
    }

    void MoveTowardsPlayer()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }

    void FireBullet()
    {
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        audioSource.Play();
        // Add force to laser to move it
        Rigidbody laserRb = laser.GetComponent<Rigidbody>();
        laserRb.AddForce(firePoint.forward * speed, ForceMode.Impulse);
        audioSource.Play();
    }

    public void StopMoving()
    {
        speed = 0;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{

    private float speed = 15;
    public bool dash;
    private PlayerController playerControllerScript;
    private float edgeLength = -12;
    public int scoreCount;
    private int currentCount;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.gameOver == false)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
            {
                if (Input.GetKey(KeyCode.LeftShift) && !playerControllerScript.gameOver)
                {
                    speed = 30;
                    dash = true;
                    scoreCount = +2;
                }
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    speed = 15;
                    dash = false;
                    
                }
            }
        }
        if (transform.position.x < edgeLength && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
        if (transform.position.x < edgeLength)
        {
            scoreCount++;
        }
            Debug.Log("Score: " + scoreCount);
        }
}

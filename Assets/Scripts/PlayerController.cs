using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DimensionSwitchTimer dimensionSwitchTimer;

    public GameObject robotEndScreen;
    public GameObject bikeEndScreen;

    private Rigidbody playerRb;
    public Transform playerTransform;

    public float jumpForce = 10;
    public float gravityModifier;

    private bool isOnGround = true;
    private bool isInAir;
    public bool gameOver;

    public Animator playerAnim;

    private Coroutine autoSwitchCoroutine;
    [SerializeField] private float autoSwitchTime = 5f;

    private bool isDimensionSwitched = false; // Start on Upside always, if true then it's on Downside
    public bool isInputEnabled = false;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -9.81f * gravityModifier, 0);
        //autoSwitchCoroutine = StartCoroutine(AutoSwitchDimension());
    }

    void Update()
    {
        if (!isInputEnabled) return;

        if (Input.GetKeyDown(KeyCode.W) && !gameOver)
        {
            if (!isDimensionSwitched && isOnGround)
            {
                Jump(Vector3.up);
            }
            else if (isDimensionSwitched && isOnGround)
            {
                SwitchDimension();
                // AudioManager.PlaySound2D(AudioManager.SoundClips.DimensionSwitchUp, 1f, 1f, 0f);
            }
        }

        if (Input.GetKeyDown(KeyCode.S) && !gameOver)
        {
            if (isDimensionSwitched && isOnGround)
            {
                Jump(Vector3.down);
            }
            else if (!isDimensionSwitched && isOnGround)
            {
                SwitchDimension();
                // AudioManager.PlaySound2D(AudioManager.SoundClips.DimensionSwitchDown, 1f, 1f, 0f);
            }
        }
        //if (Input.GetKeyDown(KeyCode.LeftShift) && !gameOver)
        //{
        //    playerAnim.speed = 3;
        //}
        //else if (Input.GetKeyUp(KeyCode.LeftShift))
        //{
        //    playerAnim.speed = 1.5f;
        //}
    }

    public void SwitchDimension()
    {
        if (gameOver) return;

        isDimensionSwitched = !isDimensionSwitched;
        InvertGravity();
        FlipPlayer();
        dimensionSwitchTimer.ResetTimer();
        if(autoSwitchCoroutine != null)
        {
            StopCoroutine(autoSwitchCoroutine);
        }
        //autoSwitchCoroutine = StartCoroutine(AutoSwitchDimension());
        gameManager.SwitchDimension();
    }

    private IEnumerator AutoSwitchDimension()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(autoSwitchTime);
            while (!isOnGround && !gameOver)
            {
                yield return null;
            }
            SwitchDimension();
        }
    }

    private void Jump(Vector3 direction)
    {
        // AudioManager.PlaySound3D(AudioManager.SoundClips.Jump, playerTransform, 1f, 1f, 1f, 0f, false, false);
        playerRb.AddForce(direction * jumpForce, ForceMode.Impulse);
        isOnGround = false;
        playerAnim.SetBool("isJumping", true);
        //dirtParticle.Stop();    
    }

    private void InvertGravity()
    {
        Physics.gravity = isDimensionSwitched ? new Vector3(0, 9.81f * gravityModifier, 0) : new Vector3(0, -9.81f * gravityModifier, 0);
        playerRb.velocity = Vector3.zero;
    }

    private void FlipPlayer()
    {
        playerTransform.Rotate(0f, 0f, 180f);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            playerAnim.SetBool("isJumping", false);
        }

        else if (collision.gameObject.CompareTag("Robot"))
        {
            Debug.Log("You smashed into a Robot");
            gameOver = true;
            if(!dimensionSwitchTimer.timerEndScreen.activeSelf)
            {
                robotEndScreen.SetActive(true);
                bikeEndScreen.SetActive(false);
            }
            gameManager.EndGame();
        }

        else if (collision.gameObject.CompareTag("Bike"))
        {
            Debug.Log("You got blasted by a Bike!");
            gameOver = true;
            if (!dimensionSwitchTimer.timerEndScreen.activeSelf)
            {
                bikeEndScreen.SetActive(true);
                robotEndScreen.SetActive(false);
            }
            gameManager.EndGame();           
        }
    }
}

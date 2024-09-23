using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private DimensionSwitchTimer dimensionSwitchTimer;
    [SerializeField] private ScoreTimer scoreTimer;

    [SerializeField] private PoolManager poolManager;

    public GameObject timerEndScreen;

    public Camera mainCamera;
    public float smoothTime = 10f;
    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;

    public Light directionalLight;

    public string upsideLightHexColor = "#FBAAFD";
    public string downsideLightHexColor = "#FDAABB";

    [SerializeField] private ScrollingBackground3D scrollingBackground3D;
    public ScrollingBackground[] scrollingBackgrounds;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private BikeSpawner bikeSpawner;

    private bool scoreboardActive = false;
    [SerializeField] private Scoreboard scoreboard;

    // void Start()
    // {
    //     // Initialize targetPosition to the camera's current position
    //     targetPosition = mainCamera.transform.position;
    // }

    void Update()
    {
        // Smoothly move the camera to the target position
        //mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, smoothTime);
        if (!scoreboardActive && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed");
            QuitGame();
        }
    }

    public void SwitchDimension()
    {
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, -mainCamera.transform.position.y, mainCamera.transform.position.z);
        directionalLight.transform.rotation = new Quaternion(-directionalLight.transform.rotation.x, directionalLight.transform.rotation.y, directionalLight.transform.rotation.z, directionalLight.transform.rotation.w);

        // Change the directional light color based on the current dimension
        string currentHexColor = mainCamera.transform.position.y < 0 ? downsideLightHexColor : upsideLightHexColor;
        if (ColorUtility.TryParseHtmlString(currentHexColor, out Color newColor))
        {
            directionalLight.color = newColor;
        }
    }

    public void BeginGame()
    {
        playerController.enabled = true;
        scoreTimer.StartTimer();
        dimensionSwitchTimer.StartTimer();
        poolManager.enabled = true;
        playerController.playerAnim.SetBool("gameStarted", true);
        playerController.isInputEnabled = true;

        foreach (ScrollingBackground background in scrollingBackgrounds)
        {
            background.enabled = true;
        }

        if (scrollingBackground3D != null)
        {
            scrollingBackground3D.StartScrolling();
        }

        if (spawnManager != null)
        {
            spawnManager.StartSpawning();
        }

        if (bikeSpawner != null)
        {
            bikeSpawner.StartSpawning();
        }
    }

    public void EndGame()
    {
        // AudioManager.PlaySound3D(AudioManager.SoundClips.Death, playerController.playerTransform, 1f, 1f, 1f, 0f);
        playerController.playerAnim.SetBool("Death", true);
        scoreTimer.StopTimer();
        dimensionSwitchTimer.StopTimer();
        foreach (ScrollingBackground background in scrollingBackgrounds)
        {
            background.enabled = false;
        }

        if (scrollingBackground3D != null)
        {
            scrollingBackground3D.StopScrolling();
        }

        if (spawnManager && bikeSpawner != null)
        {
            spawnManager.StopSpawning();
            bikeSpawner.StopSpawning();
        }
    }

    
    public void ScoreboardActive(bool scoreboard)
    {
        scoreboardActive = scoreboard;
    }

    public void SubmitScore()
    {
        StartCoroutine(scoreboard.SubmitScoreRoutine(scoreTimer.finalScoreInMS));
    }

    // Press R to restart
    public void RestartGame()
    {
        scoreboardActive = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        scoreTimer.ResetTimer();
        dimensionSwitchTimer.ResetTimer();

        if (playerController.bikeEndScreen != null && isActiveAndEnabled)
        {
            playerController.bikeEndScreen.SetActive(false);
        }

        if (playerController.robotEndScreen != null && isActiveAndEnabled)
        {
            playerController.bikeEndScreen.SetActive(false);
        }

        if (dimensionSwitchTimer.timerEndScreen != null && isActiveAndEnabled)
        {
            dimensionSwitchTimer.timerEndScreen.SetActive(false);
        }
    }

    public void QuitGame()
    {
        #if UNITY_STANDALONE
                Application.Quit();

        #elif UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;

        #elif UNITY_WEBGL
                Application.OpenURL("https://ashay-dave.me");
        #endif
    }
}

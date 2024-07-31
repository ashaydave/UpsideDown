using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image loadingImage;

    public void LoadScene(int sceneID)
    {
        StartCoroutine(LoadSceneAsync(sceneID));
    }

    IEnumerator LoadSceneAsync(int sceneID)
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);
        operation.allowSceneActivation = false;

        StartCoroutine(LoadData());

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //loadingImage.fillAmount = progress;

            if (operation.progress >= 0.9f && !operation.allowSceneActivation)
            {
                // Activate the scene
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        loadingScreen.SetActive(false);
    }

    IEnumerator LoadData()
    {
        // 
        yield return new WaitForSeconds(2.0f);
    }

}

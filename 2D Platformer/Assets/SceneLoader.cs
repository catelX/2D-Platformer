using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneLoader : MonoBehaviour
{
    public Action onLoadCallBack;
    public static SceneLoader instance;

    public bool isNewScene = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadNextScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        int sceneLoadIndex;

        isNewScene = true;
        if (scene.buildIndex == 3)
        {
            sceneLoadIndex = 1;
        }
        else sceneLoadIndex = scene.buildIndex + 1;

        onLoadCallBack = () =>
        {
            if (sceneLoadIndex == 1)
            {
                UIManager.instance.LoadMainMenuPanel();
            }
            StartCoroutine(LoadSceneAsync(sceneLoadIndex));
        };

        LoadScene(0);
    }

    public IEnumerator LoadSceneAsync(int sceneIndex)
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    public void LoadCallBack()
    {
        if (onLoadCallBack != null)
        {
            onLoadCallBack();
            onLoadCallBack = null;
        }
    }

    public int GetCurrentSceneIndex()
    {
        Scene scene = SceneManager.GetActiveScene();
        return scene.buildIndex;
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Loading")]
    public GameObject LoadingScreen;
    public GameObject MenuUI;

    [Header("SFX")]
    public AudioSource audioSource;
    public AudioClip ButtonClickSfx;

    public void Play()
    {
        audioSource.clip = ButtonClickSfx;
        audioSource.Play();
        MenuUI.SetActive(false);
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadScene("Levels"));
    }

    public void Leaderboard()
    {

    }

    public void profile()
    {

    }

    public void share()
    {

    }

    public void exitGame()
    {

    }

    public IEnumerator LoadScene(string SceneName)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(SceneName);
        scene.allowSceneActivation = false;
        while (!scene.isDone)
        {
            float progress = Mathf.Clamp01(scene.progress / 0.9f);
            if (progress >= 1f)
            {
                yield return new WaitForSeconds(3f);
                scene.allowSceneActivation = true;
            }
            yield return null;
        }

    }
}


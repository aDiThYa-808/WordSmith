using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject MenuUI;
    public GameObject LeaderBoardUI;
    public GameObject ProfileUI;


    [Header("Loading")]
    public GameObject LoadingScreen;
    

    [Header("SFX")]
    public AudioSource audioSource;
    public AudioClip ButtonClickSfx;
    public AudioClip ButtonClickSfx2;

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
        audioSource.clip = ButtonClickSfx2;
        audioSource.Play();
        MenuUI.SetActive(false);
        LeaderBoardUI.SetActive(true);
    }
    public void ExitLeaderBoard()
    {
        audioSource.clip = ButtonClickSfx2;
        audioSource.Play();
        LeaderBoardUI.SetActive(false);
        MenuUI.SetActive(true);
    }

    public void profile()
    {
        audioSource.clip = ButtonClickSfx2;
        audioSource.Play();
        MenuUI.SetActive(false);
        ProfileUI.SetActive(true);
    }
    public void exitProfile()
    {
        audioSource.clip = ButtonClickSfx2;
        audioSource.Play();
        ProfileUI.SetActive(false);
        MenuUI.SetActive(true);
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject GameOverText;
    [SerializeField] private GameObject MainUI;

    [Header("Player")]
    [SerializeField] private GameObject Player;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSrc;
    public AudioClip GameOverSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameOverText.SetActive(true);
            Player.SetActive(false);
            MainUI.SetActive(false);

            audioSrc.clip = GameOverSFX;
            audioSrc.Play();

            StartCoroutine(LoadScene("Levels"));
        }
    }

    public IEnumerator LoadScene(string SceneName)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(SceneName);
        scene.allowSceneActivation = false;

        while (!scene.isDone)
        {
            if (scene.progress >= 0.9f)
            {
                yield return new WaitForSeconds(2f); // Short delay before activation
                scene.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance

    [Header("Loading")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject LevelsUI;

    [Header("SFX")]
    [SerializeField]private AudioSource MenuAudSrc;
    [SerializeField] private AudioSource BGMAudSrc;
    public AudioClip BtnClickSfx;

    

    public void Level1()
    {
        loadscene("Level-1");
    }

    public void Level2()
    {
        loadscene("Level-2");
    }



    void loadscene(string scenename)
    {
        MenuAudSrc.clip = BtnClickSfx;
        BGMAudSrc.Stop();
        MenuAudSrc.Play();
        LevelsUI.SetActive(false);
        StartCoroutine(loadSceneCoroutine(scenename));
    }

   IEnumerator loadSceneCoroutine(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        loadingScreen.SetActive(true);

        while(scene.progress < 0.9f)
        {
            yield return null;       
        }

        yield return new WaitForSeconds(2f);

        scene.allowSceneActivation = true;
    }
}

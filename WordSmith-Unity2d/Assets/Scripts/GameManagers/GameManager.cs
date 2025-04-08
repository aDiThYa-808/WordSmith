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
    public AudioClip BtnClickSfx1;



    public void Level1()
    {
        MenuAudSrc.clip = BtnClickSfx;
        BGMAudSrc.Stop();
        MenuAudSrc.Play();
        loadscene("Level-1");
    }

    public void Level2()
    {
        MenuAudSrc.clip = BtnClickSfx;
        BGMAudSrc.Stop();
        MenuAudSrc.Play();
        loadscene("Level-2");
    }

    public void exit()
    {
        MenuAudSrc.clip = BtnClickSfx1;
        BGMAudSrc.Stop();
        MenuAudSrc.Play();
        loadscene("Home");
    }

    void loadscene(string scenename)
    {
        
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

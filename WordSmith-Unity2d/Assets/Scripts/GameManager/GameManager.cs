using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance

    [Header("Loading")]
    [SerializeField] private GameObject loadingScreen;



    public void Level1()
    {
        loadscene("level-1");
    }



    void loadscene(string scenename)
    {
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

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LevelUnlockManager : MonoBehaviour
{
    public GameObject[] levelButtons; // Assign buttons in inspector
    private string baseUrl = "http://localhost:5000/api/profile/unlocked/";

    [System.Serializable]
    public class UnlockedData
    {
        public List<int> unlockedLevels;
    }

    void Start()
    {
        string username = AuthManager.Instance.GetUsername();
        StartCoroutine(GetUnlockedLevels(username));
    }

    IEnumerator GetUnlockedLevels(string username)
    {
        string url = baseUrl + username;

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            UnlockedData data = JsonConvert.DeserializeObject<UnlockedData>(json);

            foreach (int level in data.unlockedLevels)
            {
                int index = level - 1;
                if (index >= 0 && index < levelButtons.Length)
                {
                    levelButtons[index].SetActive(true);
                }
            }

            Debug.Log("✅ Activated levels: " + string.Join(", ", data.unlockedLevels));
        }
        else
        {
            Debug.LogError("❌ Failed to fetch unlocked levels: " + request.error);
        }
    }
}

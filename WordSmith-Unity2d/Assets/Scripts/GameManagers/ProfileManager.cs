using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;
using System.Collections;

public class ProfileManager : MonoBehaviour
{
    [Header("UI Text Fields")]
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI levelsCompletedText;
    public TextMeshProUGUI starsText;
    public TextMeshProUGUI avgTimeText;

    private string baseUrl = "https://word-smith-c8ny.onrender.com/api/profile/";

    [System.Serializable]
    public class ProfileData
    {
        public string username;
        public int levelsCompleted;
        public int totalStars;
        public string averageTime;
    }

    void Start()
    {
        string username = AuthManager.Instance.GetUsername();
        StartCoroutine(FetchProfile(username));
    }

    IEnumerator FetchProfile(string username)
    {
        string url = baseUrl + username;

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            ProfileData profile = JsonConvert.DeserializeObject<ProfileData>(json);

            usernameText.text = profile.username;
            levelsCompletedText.text = profile.levelsCompleted.ToString();
            starsText.text = profile.totalStars.ToString();
            avgTimeText.text = profile.averageTime;

            Debug.Log("✅ Profile data loaded!");
        }
        else
        {
            Debug.LogError("❌ Failed to load profile data: " + request.error);
        }
    }
}

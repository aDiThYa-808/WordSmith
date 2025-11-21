using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private TMP_Text[] usernameTexts;
    [SerializeField] private TMP_Text[] timeTexts;

    private string apiUrl = "https://word-smith-c8ny.onrender.com/api/leaderboard/";

    void Start()
    {
        StartCoroutine(FetchLeaderboard());
    }

    IEnumerator FetchLeaderboard()
    {
        string fullUrl = apiUrl + levelNumber;
        UnityWebRequest request = UnityWebRequest.Get(fullUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            List<LeaderboardEntry> leaderboard = JsonConvert.DeserializeObject<List<LeaderboardEntry>>(json);

            for (int i = 0; i < 3; i++)
            {
                if (i < leaderboard.Count)
                {
                    usernameTexts[i].text = leaderboard[i].username;
                    timeTexts[i].text = leaderboard[i].timeTaken;
                }
                else
                {
                    usernameTexts[i].text = "--";
                    timeTexts[i].text = "--";
                }
            }
        }
        else
        {
            Debug.LogError("❌ Failed to fetch leaderboard: " + request.error);
        }
    }

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string username;
        public string timeTaken;
    }
}

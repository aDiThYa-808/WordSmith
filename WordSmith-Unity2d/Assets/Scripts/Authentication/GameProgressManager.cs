using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;

public class GameProgressManager : MonoBehaviour
{
    [Header("Player Progress")]
    public string username;
    public int stars;
    public string completionTime;
    public bool levelComplete; // Renamed for consistency

    private string apiUrl = "http://localhost:5000/api/progress/save"; // Update for deployment

    private void Start()
    {
        // Load username from PlayerPrefs
        string storedUsername = PlayerPrefs.GetString("username", "Not Found");

        Debug.Log("🔹 Loaded username from PlayerPrefs in Level-1: " + storedUsername);

        // If username is missing, redirect to Login scene
        if (string.IsNullOrEmpty(storedUsername) || storedUsername == "Not Found")
        {
            Debug.LogError("❌ No username found in PlayerPrefs!");
           // SceneManager.LoadScene("Login");
        }
    }

    public void SaveProgress()
    {
        StartCoroutine(SendProgressToServer());
    }

    IEnumerator SendProgressToServer()
    {
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("❌ Cannot save progress: No username found.");
            yield break; // Prevent the request if user is not logged in
        }

        var progressData = new
        {
            username,
            stars,
            timeTaken = completionTime, // Matches backend expectations
            levelComplete // Ensuring naming consistency
        };

        string jsonData = JsonConvert.SerializeObject(progressData);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ Progress saved successfully: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("❌ Failed to save progress: " + request.error);
            }
        }
    }
}

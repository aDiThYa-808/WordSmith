using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;

public class GameProgressManager : MonoBehaviour
{
    [Header("Player Progress")]
    public string username;
    public int levelNumber;
    public int stars;
    public string completionTime;
    public bool levelComplete; // Renamed for consistency

    private string apiUrl = "http://localhost:5000/api/progress/save"; // Update for deployment

    private void Start()
    {
        StartCoroutine(WaitForAuthManager());
    }

    IEnumerator WaitForAuthManager()
    {
        yield return new WaitUntil(() => AuthManager.Instance != null);

        username = AuthManager.Instance.GetUsername();

        if (string.IsNullOrEmpty(username))
        {
            Debug.Log("⚠️ Username is empty, defaulting to Guest.");
            username = "Guest";
        }

        Debug.Log("✅ Username retrieved: " + username);
    }

    public void SaveProgress()
    {
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("❌ Cannot save progress: No username found.");
            return; // Exit if no username
        }
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
            levelNumber,
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

using System.Collections;
using UnityEngine;
using TMPro;

public class EndLevel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject EndLevelPanel;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private string MagicWord;

    [Header("Level")]
    [SerializeField] private int levelNumber;
    [SerializeField] private GameObject player;
    [SerializeField] private GamePlayManager gamePlayManager;

    public bool levelComplete { get; private set; }
    public string completionTime;
    public int stars;

    private void Awake()
    {
        levelComplete = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EndLevelPanel.SetActive(true);
            player.SetActive(false);
        }
    }

    public void ExitButton()
    {
        EndLevelPanel.SetActive(false);
        player.SetActive(true);
    }

    public void DoneButton()
    {
        if (inputField.text.Trim().ToLower() == MagicWord.ToLower())
        {
            EndLevelPanel.SetActive(false);
            levelComplete = true;
            completionTime = gamePlayManager.StopAndGetFinalTime();
            stars = StarsObtained(completionTime);

            Debug.Log($"✅ Correct! Level Complete. Time: {completionTime}, Stars: {stars}");

            LevelCleared();
        }
        else
        {
            Debug.Log("❌ Wrong word! Try again.");
        }

        inputField.text = "";
    }

    private void LevelCleared()
    {
        // Fetch username directly from AuthManager
        string username = "Guest"; // Default to 'Guest'
        if (AuthManager.Instance != null)
        {
            username = AuthManager.Instance.GetUsername();
        }

        // Ensure GameProgressManager is available
        GameProgressManager progressManager = FindObjectOfType<GameProgressManager>();

        if (progressManager != null)
        {
            progressManager.username = AuthManager.Instance?.GetUsername() ?? "Guest";
            progressManager.levelNumber = levelNumber;
            progressManager.stars = stars;
            progressManager.completionTime = completionTime;
            progressManager.levelComplete = true;
            progressManager.SaveProgress();
        }
        else
        {
            Debug.LogError("❌ GameProgressManager not found in the scene!");
        }
    }

    private int StarsObtained(string timeString)
    {
        string[] parts = timeString.Split(':');
        if (parts.Length != 2 || !int.TryParse(parts[0], out int minutes) || !int.TryParse(parts[1], out int seconds))
        {
            Debug.LogError("❌ Invalid time format. Expected mm:ss");
            return 1; // Fallback to 1 star on error
        }

        int totalSeconds = minutes * 60 + seconds;

        if (totalSeconds <= 60)
            return 3;
        else if (totalSeconds <= 90)
            return 2;
        else
            return 1;
    }

}

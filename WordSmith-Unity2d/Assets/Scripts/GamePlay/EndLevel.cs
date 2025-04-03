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
    [SerializeField] private GameObject player;
    [SerializeField] private GamePlayManager gamePlayManager;

    public bool levelComplete { get; private set; }
    public string completionTime;

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
            int stars = StarsObtained(completionTime);

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
        string username = PlayerPrefs.GetString("username", "Guest");
        int stars = StarsObtained(completionTime);

        GameProgressManager progressManager = FindObjectOfType<GameProgressManager>();

        if (progressManager != null)
        {
            progressManager.username = username;
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
        return int.TryParse(timeString.Split(':')[1], out int seconds) && seconds <= 60 ? 3 : seconds <= 90 ? 2 : 1;
    }
}


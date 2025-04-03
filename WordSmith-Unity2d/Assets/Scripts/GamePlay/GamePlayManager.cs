using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayManager : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private List<GameObject> enemies;

    [Header("Letters")]
    [SerializeField] private List<GameObject> letters;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textMessage;
    [SerializeField] private TextMeshProUGUI timer;

    private float elapsedTime = 0f;
    private bool timerRunning = false;

    private void Start()
    {
        elapsedTime = 0f;
        timerRunning = true;
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while (timerRunning)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timer.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Format as MM:SS
            yield return null;
        }
    }



    public void EnemyDefeated(GameObject enemy)
    {
        int index = enemies.IndexOf(enemy);
        if (index != -1 && index < letters.Count)
        {
            StartCoroutine(ShowMessage("You obtained the letter " + letters[index].name + "!"));
            letters[index].SetActive(true); // Enable the corresponding letter
            enemies[index] = null; // Remove reference to defeated enemy (optional)
        }
    }

    IEnumerator ShowMessage(string message)
    {
        textMessage.text = message;
        yield return new WaitForSeconds(2f);
        textMessage.text = "";
    }

    public string StopAndGetFinalTime()
    {
        timerRunning = false;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}

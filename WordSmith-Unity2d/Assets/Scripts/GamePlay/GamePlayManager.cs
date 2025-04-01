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
}

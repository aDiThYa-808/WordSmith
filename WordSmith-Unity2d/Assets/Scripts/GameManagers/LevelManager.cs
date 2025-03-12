using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("Level")]
    public GameObject LevelInfoPanel;
    public GameObject Game;
    public GameObject GameLostPanel;

    [Header("Timer")]
    public TextMeshProUGUI TimerText;
    public float TimeLimit;

    [Header("SFX")]
    [SerializeField] private AudioSource MenuAudioSrc;
    public AudioClip BtnClickSfx;
    //public AudioClip GameLostSfx;


    private void Update()
    {
        timer();


       
        
        
    }


    public void StartLevel()
    {
        MenuAudioSrc.clip = BtnClickSfx;
        MenuAudioSrc.Play();
        LevelInfoPanel.SetActive(false);
        Game.SetActive(true);
    }

    public void timer()
    {
        if (Game.activeSelf)
        {
            if (TimeLimit > 0)
            {
                TimeLimit -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(TimeLimit / 60);
                int seconds = Mathf.FloorToInt(TimeLimit % 60);
                TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                GameLost();
            }
        }
        
    }

    public void GameLost()
    {
        Game.SetActive(false);
        //MenuAudioSrc.clip = GameLostSfx;
        //MenuAudioSrc.Play();
        GameLostPanel.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health stats")]
    [SerializeField] private float playerHealth;
    public float currentHealth { get; private set; }

    [Header("Audio Sources")]
    public AudioSource CollectableAudioSrc;
    public AudioSource PlayerVoiceSrc;
    [SerializeField] private AudioSource MusicAudioSrc;
    [SerializeField] private GameObject BGMsrc;

    [Header("Audio Clips")]
    public AudioClip PlayerHurtSfx;
    public AudioClip PlayerDeathSfx;
    public AudioClip HealthAddSound;
    public AudioClip GameOverSFX;


    [Header("IFrames")]
    [SerializeField] private float IFrameDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRen;

    [Header("UI")]
    [SerializeField] private GameObject GameOverText;
    [SerializeField] private GameObject MainUI;

    private Animator anim;
    private bool dead;

    private void Awake()
    {
        currentHealth = playerHealth;
        anim = GetComponent<Animator>();
        spriteRen = GetComponent<SpriteRenderer>();
    }

    public void DamagePlayer(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, playerHealth);

        if(currentHealth > 0)
        {
            //hurt animation
            anim.SetTrigger("hurt");

            //hurt sfx
            PlayerVoiceSrc.clip = PlayerHurtSfx;
            PlayerVoiceSrc.Play();

            //enable iframes
            StartCoroutine(EnableIFrames());
        }
        else
        {
            if (!dead)
            {
                //animate death
                anim.SetTrigger("die");

                //death sfx
                PlayerVoiceSrc.clip = PlayerDeathSfx;
                PlayerVoiceSrc.Play();

                //disable player movements after death
                GetComponent<PlayerMovement>().enabled = false;
                dead = true;

                BGMsrc.SetActive(false);

                GameOver();
                
            }
            
        }
    }

    public void addHealth(float _value)
    {
        //subract damage from health
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, playerHealth);

        //Health add sfx
        CollectableAudioSrc.clip = HealthAddSound;
        CollectableAudioSrc.Play();
    }

    private IEnumerator EnableIFrames()
    {
        //disable collisions
        Physics2D.IgnoreLayerCollision(7, 8, true);

        //turn sprite color to red during IFrameDuration and go back to white
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRen.color = new Color(1,0,0,0.5f);
            yield return new WaitForSeconds(IFrameDuration / (numberOfFlashes * 2));
            spriteRen.color = Color.white;
            yield return new WaitForSeconds(IFrameDuration / (numberOfFlashes * 2));
        }

        //enable collisions
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }


    public void GameOver()
    {
        StartCoroutine(EndGame());
    }


    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1f);
        GameOverText.SetActive(true);
        MainUI.SetActive(false);

        MusicAudioSrc.clip = GameOverSFX;
        MusicAudioSrc.Play();

        StartCoroutine(LoadScene("Levels"));
    }

    public IEnumerator LoadScene(string SceneName)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(SceneName);
        scene.allowSceneActivation = false;

        while (!scene.isDone)
        {
            if (scene.progress >= 0.9f)
            {
                yield return new WaitForSeconds(3f);
                scene.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}

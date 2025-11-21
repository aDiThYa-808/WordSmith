using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health stats")]
    [SerializeField] private float enemyHealth;
    public float currentEnemyHealth { get; private set; }

    [Header("Audio Sources")]
    public AudioSource EnemyVoiceSrc;

    [Header("Audio Clips")]
    public AudioClip EnemyHurtSfx;
    public AudioClip EnemyDeathSfx;


    [Header("IFrames")]
    [SerializeField] private float IFrameDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRen;

    [Header("Level Manager")]
    [SerializeField] private GameObject gamePlayManager;

    private Animator anim;
    public bool dead { get; private set; }


    private void Awake()
    {
        currentEnemyHealth = enemyHealth;
        anim = GetComponent<Animator>();
        spriteRen = GetComponent<SpriteRenderer>();
    }


    public void DamageEnemy(float _damage)
    {
        currentEnemyHealth = Mathf.Clamp(currentEnemyHealth - _damage, 0, enemyHealth);

        if (currentEnemyHealth > 0)
        {
            //hurt animation
            anim.SetTrigger("hurt");

            //hurt sfx
            EnemyVoiceSrc.clip = EnemyHurtSfx;
            EnemyVoiceSrc.Play();

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
                EnemyVoiceSrc.clip = EnemyDeathSfx;
                EnemyVoiceSrc.Play();

                //disable enemy movements after death
                GetComponentInParent<EnemyPatrol>().enabled = false;
                dead = true;

                gamePlayManager.GetComponent<GamePlayManager>().EnemyDefeated(gameObject);
               
            }

        }
    }

    private IEnumerator EnableIFrames()
    {
        //disable collisions
        Physics2D.IgnoreLayerCollision(7, 8, true);

        //turn sprite color to red during IFrameDuration and go back to white
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRen.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(IFrameDuration / (numberOfFlashes * 2));
            spriteRen.color = Color.white;
            yield return new WaitForSeconds(IFrameDuration / (numberOfFlashes * 2));
        }

        //enable collisions
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }

    /*private IEnumerator destroyEnemyGameObject()
    {
        yield return new WaitForSeconds(2f);
        Destroy(enemyPatrol);
    }*/
}

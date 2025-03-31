using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player attack range")]
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    public LayerMask enemyLayer;

    [Header("Power stats")]
    [SerializeField] private float damage;
    [SerializeField] private float AttackCoolDownDuration;
    private float coolDownTime;

    [Header("Audio sources and clips")]
    [SerializeField] private AudioSource WeaponAudioSource;
    public AudioClip SwordSlash;
    public AudioClip SwordSwoosh;


    private BoxCollider2D boxCol;
    private EnemyHealth enemyHealth;
    private Animator anim;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        coolDownTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (coolDownTime >= AttackCoolDownDuration)
            {
                coolDownTime = 0;
                anim.SetTrigger("attack");

                if (!enemyInRange())
                {
                    WeaponAudioSource.clip = SwordSwoosh;
                    WeaponAudioSource.Play();
                }
            }
        }
        
    }

    bool enemyInRange()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCol.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCol.bounds.size.x * range, boxCol.bounds.size.y, boxCol.bounds.size.z),
            0,
            Vector2.left,
            0,
            enemyLayer
            );

        if (raycastHit.collider != null)
        {
            enemyHealth = raycastHit.transform.GetComponent<EnemyHealth>();
        }

        return raycastHit.collider != null;
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCol.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCol.bounds.size.x * range, boxCol.bounds.size.y, boxCol.bounds.size.z)
            );
    }*/

    void DamageEnemy()
    {
        if (enemyInRange())
        {
            enemyHealth.DamageEnemy(damage);
            WeaponAudioSource.clip = SwordSlash;
            WeaponAudioSource.Play();
        }        
    }
}

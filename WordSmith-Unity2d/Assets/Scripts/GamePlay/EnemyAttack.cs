using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCoolDown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private int damage;
    public LayerMask playerLayer;
    private BoxCollider2D boxCol;
    private float coolDownTimer = Mathf.Infinity;

    private Animator anim;
    private PlayerHealth playerHealth;
    private EnemyPatrol enemyPatrol;


    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        coolDownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (coolDownTimer >= attackCoolDown)
            {
                coolDownTimer = 0;
                anim.SetTrigger("attack");

            }
        }

        if(enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
        
    }


    bool PlayerInSight()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCol.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCol.bounds.size.x * range, boxCol.bounds.size.y, boxCol.bounds.size.z), 
            0 , 
            Vector2.left, 
            0, 
            playerLayer
            );

        if(raycastHit.collider != null)
        {
            playerHealth = raycastHit.transform.GetComponent<PlayerHealth>();
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


    public void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.DamagePlayer(damage);
        }
    }
}

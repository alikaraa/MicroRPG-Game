using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [Header("Stats")]
    public int curHp;
    public int maxHp;
    public float moveSpeed;
    public int xpToGive;

    [Header("Target")]
    public float chaseRange;
    public float attackRange;
    private Player player;
    [Header("Attack")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;

    //components
    private Rigidbody2D rig;

    void Awake(){
        //get the player target 
        player = FindObjectOfType<Player>();

        //get the rigidbody component
        rig = GetComponent<Rigidbody2D>();
    }
    
    void Update(){
        float playerDist = Vector2.Distance(transform.position, player.transform.position);
        if(playerDist <= attackRange){
            //Attack the player
            if(Time.time - lastAttackTime >= attackRate)
                Attack();

            rig.velocity = Vector2.zero;

        }else if(playerDist <= chaseRange){
            Chase();
        }else{
            rig.velocity = Vector2.zero;
        }
    }

    void Attack(){
        lastAttackTime = Time.time;

        player.TakeDamage(damage);
    }

    void Chase(){
        Vector2 dir = (player.transform.position - transform.position).normalized;
        rig.velocity = dir * moveSpeed;
    }

    public void TakeDamage(int damageTaken){
        curHp -= damageTaken;

        if(curHp <= 0){
            Die();
        }
    }

    void Die(){
        //Give the player xp
        player.AddXp(xpToGive);
        
        Destroy(gameObject);
    }


}

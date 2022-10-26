using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float damage = 50;
   
    EnemyHealth enemyHealth;

    void Awake()
    {
        enemyHealth = FindObjectOfType<EnemyHealth>();
    }
   

    public void AttackClose()
    {
        Debug.Log("AttackClose");
        enemyHealth.currentHealth -= damage;
    }

    public void AttackCloseMid()
    {
        Debug.Log("AttackCloseMid");
        enemyHealth.currentHealth -= damage;
    }

    public void AttackFarMid()
    {
        enemyHealth.currentHealth -= damage;
        Debug.Log("AttackFarMid");
    }

    public void AttackFar()
    {
        enemyHealth.currentHealth -= damage;
        Debug.Log("AttackFar");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] MakeMonsterSO monster;
    [SerializeField] Slider healthSlider;

    public float maxHealth = 1000;
    public float currentHealth;

    private void Awake()
    {
        maxHealth = monster.life;
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
       
    }

    private void Update()
    {
        healthSlider.value = currentHealth;
    }

}

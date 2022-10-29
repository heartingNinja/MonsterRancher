using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] MakeMonsterSO monster;
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;

    public float maxHealth = 1000;
    public float currentHealth;

    public bool isRight;

    private void Awake()
    {
        maxHealth = monster.life;
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
       
    }

    private void Update()
    {
        healthSlider.value = currentHealth;

        if(isRight)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }
        else
        {
            healthText.text = maxHealth + " / " + currentHealth;
        }
        
    }

}

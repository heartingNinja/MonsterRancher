using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Slider healthSlider;

    [SerializeField] float maxHealth = 1000;
    public float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
       
    }

    private void Update()
    {
        healthSlider.value = currentHealth;
    }

}

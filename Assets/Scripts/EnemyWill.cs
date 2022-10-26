using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWill : MonoBehaviour
{
    [SerializeField] Slider willSlider;

    public float willMax = 100;
    public float willCurrent = 40;
    [SerializeField] float inscreaseWill = 1;

    private void Awake()
    {
       
    }

   
    void Update()
    {
        willCurrent +=  (inscreaseWill * Time.deltaTime);
        willSlider.value = willCurrent;

        if(willCurrent >= willMax)
        {
            willCurrent = willMax;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManger : MonoBehaviour
{
    public GameObject damageTextPrefabRight, enemyInstance;
    public GameObject damageRightEnemy;
    public GameObject damageTextPrefabLeft;
    public EnemyMonsterMovement enemyMonsterMovement;
    public EnemyMovementStartRight enemyMovementStartRight;

    public string textToDisplay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void PostDamageCodeRight()
    {
        if(enemyMonsterMovement.fightHuman == false)
        {
            GameObject DamageTextInstance = Instantiate(damageTextPrefabRight, damageRightEnemy.transform);
           
            DamageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(enemyMonsterMovement.damage.ToString());
        }
    }

    public void PostDamageCodeLeft()
    {
        if (enemyMovementStartRight.fightHuman == false)
        {
           
            GameObject DamageTextInstance = Instantiate(damageTextPrefabLeft, enemyInstance.transform);
            DamageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(enemyMovementStartRight.damage.ToString());
        }
    }

    public void PostDodgeRight()
    {
        GameObject DamageTextInstance = Instantiate(damageTextPrefabRight, damageRightEnemy.transform);
        DamageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("Dodge");
        
    }

    public void PostDodgeLeft()
    {
        GameObject DamageTextInstance = Instantiate(damageTextPrefabLeft, enemyInstance.transform);
        DamageTextInstance.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("Dodge");
    }
}

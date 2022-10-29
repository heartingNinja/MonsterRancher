using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceToEnemy : MonoBehaviour
{
    [SerializeField] bool isRight;
    [SerializeField] EnemyMonsterMovement enemyMonsterMovement;
    [SerializeField] EnemyMovementStartRight enemyMovementStartRight;

    
    [SerializeField] Button closeAttack;
    [SerializeField] Button closeMidAttack;
    [SerializeField] Button closeFarAttack;
    [SerializeField] Button farAttack;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isRight)
        {
            if(enemyMovementStartRight.sliderValueDistancetoEnemy < .25f)
            {
                closeAttack.Select();
            }
           

            if(enemyMovementStartRight.sliderValueDistancetoEnemy >= .25f && enemyMovementStartRight.sliderValueDistancetoEnemy < .50f)
            {
                closeMidAttack.Select();
            }

            if(enemyMovementStartRight.sliderValueDistancetoEnemy >= .50f && enemyMovementStartRight.sliderValueDistancetoEnemy < .75f)
            {
                closeFarAttack.Select();
            }

            if(enemyMovementStartRight.sliderValueDistancetoEnemy >= .75f)
            {
                farAttack.Select();
            }
        }
        else
        {
            if (enemyMonsterMovement.sliderValueDistancetoEnemy < .25f)
            {
                closeAttack.Select();
            }

            if (enemyMonsterMovement.sliderValueDistancetoEnemy >= .25f && enemyMonsterMovement.sliderValueDistancetoEnemy < .50f)
            {
                closeMidAttack.Select();
            }

            if (enemyMonsterMovement.sliderValueDistancetoEnemy >= .50f && enemyMonsterMovement.sliderValueDistancetoEnemy < .75f)
            {
                closeFarAttack.Select();
            }

            if (enemyMonsterMovement.sliderValueDistancetoEnemy >= .75f)
            {
                farAttack.Select();
            }
        }
    }
}

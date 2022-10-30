using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMonsterAnimation : MonoBehaviour
{
    EnemyMonsterMovement enemyMonsterMovement;
    EnemyMovementStartRight enemyMovementStartRight;

    [SerializeField] bool startRight;

    Animator enemyAnimator;

    private void Awake()
    {
        if(startRight == false)
        {
            enemyMonsterMovement = GetComponentInParent<EnemyMonsterMovement>();
        }
        else
        {
            enemyMovementStartRight = GetComponentInParent<EnemyMovementStartRight>();
        }
       
        
        enemyAnimator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // need to have no ability to change animation until attack is finished. Right now player moves while attacking
        if(startRight == false)
        {
            if (enemyMonsterMovement.wait)
            {
                enemyAnimator.SetBool("Wait", true);
            }
            else
            {
                enemyAnimator.SetBool("Wait", false);
            }

            if (enemyMonsterMovement.moveForward)
            {
                enemyAnimator.SetBool("WalkForward", true);
            }
            else
            {
                enemyAnimator.SetBool("WalkForward", false);
            }

            if (enemyMonsterMovement.moveBackward)
            {
                enemyAnimator.SetBool("WalkBack", true);
            }
            else
            {
                enemyAnimator.SetBool("WalkBack", false);
            }

            if (enemyMonsterMovement.attack && enemyMonsterMovement.closeToEnemy)
            {

                enemyAnimator.SetBool("AttackClose", true);
            }

            if (enemyMonsterMovement.attack && enemyMonsterMovement.midCloseEnemy)
            {
                enemyAnimator.SetBool("AttackMidClose", true);
                enemyMonsterMovement.attack = false;
            }

            if (enemyMonsterMovement.attack && enemyMonsterMovement.midFarEnemy)
            {
                enemyAnimator.SetBool("AttackMidFar", true);
            }

            if (enemyMonsterMovement.attack && enemyMonsterMovement.farEnemy)
            {
                enemyAnimator.SetBool("AttackFar", true);
            }
        }
        else
        {
            if (enemyMovementStartRight.wait)
            {
                enemyAnimator.SetBool("Wait", true);
            }
            else
            {
                enemyAnimator.SetBool("Wait", false);
            }

            if (enemyMovementStartRight.moveForward)
            {
                enemyAnimator.SetBool("WalkForward", true);
            }
            else
            {
                enemyAnimator.SetBool("WalkForward", false);
            }

            if (enemyMovementStartRight.moveBackward)
            {
                enemyAnimator.SetBool("WalkBack", true);
            }
            else
            {
                enemyAnimator.SetBool("WalkBack", false);
            }

            if (enemyMovementStartRight.attack && enemyMovementStartRight.closeToEnemy)
            {

                enemyAnimator.SetBool("AttackClose", true);
            }

            if (enemyMovementStartRight.attack && enemyMovementStartRight.midCloseEnemy)
            {
                enemyAnimator.SetBool("AttackMidClose", true);
                enemyMonsterMovement.attack = false;
            }

            if (enemyMovementStartRight.attack && enemyMovementStartRight.midFarEnemy)
            {
                enemyAnimator.SetBool("AttackMidFar", true);
            }

            if (enemyMovementStartRight.attack && enemyMovementStartRight.farEnemy)
            {
                enemyAnimator.SetBool("AttackFar", true);
            }
        }
        
    }

    public void SetAttackCloseFalse()
    {
        if(startRight == false)
        {
            enemyMonsterMovement.attack = false;
        }
        else
        {
            enemyMovementStartRight.attack = false;
        }
        
        enemyAnimator.SetBool("AttackClose", false);
        enemyAnimator.SetBool("AttackMidClose", false);
        enemyAnimator.SetBool("AttackMidFar", false);
        enemyAnimator.SetBool("AttackFar", false);
    }
}

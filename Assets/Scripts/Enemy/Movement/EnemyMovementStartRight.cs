using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using System;

public class EnemyMovementStartRight : MonoBehaviour
{
    [SerializeField] MakeMonsterSO thisMonsterStats;
    [SerializeField] MakeMonsterSO enemyMonsterStats;

    public int hitChanceForUI;
    [SerializeField] TextMeshProUGUI hitChanceText;

    //Transform targetDestination;
    GameObject targetGameobject;
    [SerializeField] GameObject rightEdge;
    [SerializeField] Slider distanceToPlayerSlider;
    [SerializeField] float speed = 4; // get this from scriptable object in future
    GameManger gameManger;
    

    // if player has more health from start less attacking, if enemy has less health from start increase attacking. change the attackbonus
    [SerializeField] float attackbonus = 0; // if attackbonus increases it takes down wait chance. max will be 25 attack. This gives 50% attack and 50% move. 
    Rigidbody rb;
    bool moveForward;
    bool moveBackward;
    bool wait;
    bool attack;

    int attackBonuseMultiplier = 3;
    public bool fightHuman;

    public float damage = 100;

    [SerializeField] float timeForState = 2;
    float sliderValue;
    [SerializeField] EnemyHealth enemyHealth;
    PlayerHealth playerHealth;

    float attackReset = .5f;

    public int customSeed;
    int hour;
    int minutes;
    int seconds;
    int miliseconds;

    EnemyHealth selfHealthEnemy;

    public int powerVsDefenseBonus; // = thisMonsterStats.power - enemyMonsterStats.defense; // sets increase to damage or decrease, can be 998+ to 998-
    public int dodgeBonus; // = thisMonsterStats.skill - enemyMonsterStats.speed; // sets increase to dodge or decreae, can be 998+ to 998-

    public int dodgeBase = 100; // base dodge if speed an skill, 100 should be 10%
    public int dodgeRollRandom; // =Random.Range(1, 1001); // what number will be choose to see if enemy dodges
    public bool enemyDodge;
    int dodge;

    public bool enemyMiss;

    public int skillbase = 25;
    public int skillBonus;
    public int skillRollRandom;

    public int attackRandomRoll;
    public int hitChanceBonus;

    int willCostClose;
    int willCostMidClose;
    int willCostMidFar;
    int willCostFar;

    public int willTaken;
    public bool willToAttackBool = true;

    EnemyWill enemyWill;

    private void Awake()
    {
        enemyWill = GetComponent<EnemyWill>();
        rb = GetComponent<Rigidbody>();
        // targetGameobject = FindObjectOfType<MonsterMove>().gameObject;
        selfHealthEnemy = GetComponent<EnemyHealth>();
        gameManger = FindObjectOfType<GameManger>();

        hour = System.DateTime.Now.Hour;
        minutes = System.DateTime.Now.Minute;
        seconds = System.DateTime.Now.Second;
        miliseconds = System.DateTime.Now.Millisecond;

        if (fightHuman)
        {
            targetGameobject = FindObjectOfType<MonsterMove>().gameObject;
            playerHealth = FindObjectOfType<PlayerHealth>();
        }
        else
        {
            targetGameobject = enemyHealth.gameObject;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        customSeed = (hour + minutes + seconds + miliseconds) * (hour + minutes + seconds + miliseconds) ;
        Random.InitState(customSeed );

        powerVsDefenseBonus = thisMonsterStats.power - enemyMonsterStats.defense; // sets increase to damage or decrease, can be 998+ to 998-
        dodgeBonus = enemyMonsterStats.speed - thisMonsterStats.speed;  // sets increase to dodge or decreae, can be 998+ to 998-
        skillBonus = thisMonsterStats.skill - enemyMonsterStats.skill; // sets increase or decrease to skill. Skill decides crit %

        // will need a 2nd for 2nd attacks with a bool to choose what one to use. Can use enemy data for attack and choose attack from bool
        willCostClose = thisMonsterStats.willCostClose;
        willCostMidClose = thisMonsterStats.willCostCloseMid;
        willCostMidFar = thisMonsterStats.willCostFarMid;
        willCostFar = thisMonsterStats.willCostFar;
    }

    // Update is called once per frame
    void Update()
    {
        // NextState();
        DistanceToEnemy();
        AttackBonus();
        DamageToGive();

        

        hitChanceForUI = (hitChanceBonus * (100 - (dodge/10)))/100;

        if(hitChanceForUI > 100)
        {
            hitChanceForUI = 99;
        }

        if(hitChanceForUI < 0)
        {
            hitChanceForUI = 0;
        }

        hitChanceText.text = hitChanceForUI.ToString();
    }



    private void FixedUpdate()
    {
        MoveForward();
        MoveBack();
        Wait();
        // Attack();

        NextState();
    }

    void AttackBonus() // add in will amount to increase attack bonus, later time left, less time moare attack
    {
        int increaseAttackOnEnemyHealth;
        int increaseAttackOnPlayerHealth;

        if (fightHuman)
        {
            increaseAttackOnEnemyHealth = (int)(playerHealth.maxHealth / playerHealth.currentHealth); // makes number bigger that 1 to mutiply with

            increaseAttackOnPlayerHealth = (int)(selfHealthEnemy.maxHealth / selfHealthEnemy.currentHealth);
            attackbonus = increaseAttackOnEnemyHealth * attackBonuseMultiplier + increaseAttackOnPlayerHealth * attackBonuseMultiplier * 2 - 4;
        }
        else
        {
            increaseAttackOnEnemyHealth = (int)(enemyHealth.maxHealth / enemyHealth.currentHealth); // makes number bigger that 1 to mutiply with

            increaseAttackOnPlayerHealth = (int)(selfHealthEnemy.maxHealth / selfHealthEnemy.currentHealth);
            attackbonus = (100 -(increaseAttackOnEnemyHealth * attackBonuseMultiplier + increaseAttackOnPlayerHealth * attackBonuseMultiplier * 2 - 4 + enemyWill.willCurrent))/100;
        }

        if(attackbonus < 0)
        {
            attackbonus = 0.01f;
        }

        if(attackbonus > 1)
        {
            attackbonus = 1;
        }

     //   if (attackbonus >= 20) // instead of subtracting attackbonus from other stats have them muiltplied by attackbonus. the larger the attack bonus the less attack will be added. use (100-attackbonus)/100, make max attack bonus = 99
     //   {
     //       attackbonus = 20;
     //   }
    }

    void DamageToGive()
    {
        int damageStartValue;
        dodgeRollRandom = Random.Range(1, 1001);

        int hitChance;
        
        attackRandomRoll = Random.Range(1, 101);

        dodge = dodgeBase + dodgeBonus; // increase or decrease to dodge

        if(dodge < 0)
        {
            dodge = 0;
        }

       

        // close
        if (sliderValue < .25f)
        {
            willTaken = willCostClose;

            if (willCostClose > enemyWill.willCurrent)
            {
                willToAttackBool = false;
            }
            else
            {
                willToAttackBool = true;
            }

            damageStartValue = thisMonsterStats.attackDamageClose;
            hitChance = thisMonsterStats.hitPercentClose;
            hitChanceBonus = hitChance + (skillBonus / 100);

            if (hitChanceBonus < hitChance)
            {
                hitChanceBonus = hitChance;
            }

            if (hitChanceBonus > attackRandomRoll)
            {
                enemyMiss = false;
            }
            else
            {
                enemyMiss = true;
                
            }

            if (dodgeRollRandom > dodge)
                {
                    enemyDodge = false;

                    if (powerVsDefenseBonus < 0)
                    {
                        damage = damageStartValue + (powerVsDefenseBonus / 10);

                        if (damage > 0) // can not give negitive or 0 damage
                        {
                            damage = 1;
                        }
                    }
                    else
                    {
                        damage = damageStartValue + powerVsDefenseBonus;
                    }
                }
                else
                {
                    enemyDodge = true;

                }
            
           

            



        }

        // close mid
        if (sliderValue >= .25f && sliderValue < .50f)
        {
            willTaken = willCostMidClose;

            if (willCostMidClose > enemyWill.willCurrent)
            {
                willToAttackBool = false;
            }
            else
            {
                willToAttackBool = true;
            }

            damageStartValue = thisMonsterStats.attackDamageCloseMid;
            hitChance = thisMonsterStats.hitPercentCloseMid;
            hitChanceBonus = hitChance + (skillBonus / 100);

            if (hitChanceBonus < hitChance)
            {
                hitChanceBonus = hitChance;
            }

            if (hitChanceBonus > attackRandomRoll)
            {
                enemyMiss = false;
            }
            else
            {
                enemyMiss = true;
            }

            if (dodgeRollRandom > dodge)
                {
                    enemyDodge = false;

                    if (powerVsDefenseBonus < 0)
                    {
                        damage = damageStartValue + (powerVsDefenseBonus / 10);

                        if (damage < 0)
                        {
                            damage = 1;
                        }
                    }
                    else
                    {
                        damage = damageStartValue + powerVsDefenseBonus;
                    }
                }
                else
                {
                    enemyDodge = true;

                }
            
            

           




        }

        // mid far
        if (sliderValue >= .50f && sliderValue < .75f)
        {
            willTaken = willCostMidFar;

            if (willCostMidFar > enemyWill.willCurrent)
            {
                willToAttackBool = false;
            }
            else
            {
                willToAttackBool = true;
            }

            damageStartValue = thisMonsterStats.attackDamageFarMid;           
            hitChance = thisMonsterStats.hitPercentFarMid;
            hitChanceBonus = hitChance + (skillBonus / 100);

            if (hitChanceBonus < hitChance)
            {
                hitChanceBonus = hitChance;
            }

            if (hitChanceBonus > attackRandomRoll)
            {
                enemyMiss = false;
            }
            else
            {
                enemyMiss = true;
            }

            if (dodgeRollRandom > dodge)
                {
                    enemyDodge = false;

                    if (powerVsDefenseBonus < 0)
                    {
                        damage = damageStartValue + (powerVsDefenseBonus / 10);

                        if (damage < 0)
                        {
                            damage = 1;
                        }
                    }
                    else
                    {
                        damage = damageStartValue + powerVsDefenseBonus;
                    }
                }
                else
                {
                    enemyDodge = true;

                }
            
           

               





        }

        // far
        if (sliderValue >= .75f)
        {
            willTaken = willCostFar;

            if (willCostFar > enemyWill.willCurrent)
            {
                willToAttackBool = false;
            }
            else
            {
                willToAttackBool = true;
            }

            damageStartValue = thisMonsterStats.attackDamageFar;

            hitChance = thisMonsterStats.hitPercentFar;
            hitChanceBonus = hitChance + (skillBonus / 100);

           

            if (hitChanceBonus < hitChance)
            {
                hitChanceBonus = hitChance;
            }


            if (hitChanceBonus > attackRandomRoll)
            {
                enemyMiss = false;
            }
            else
            {
                enemyMiss = true;
            }

            if (dodgeRollRandom > dodge)
                {
                    enemyDodge = false;

                    if (powerVsDefenseBonus < 0)
                    {
                        damage = damageStartValue + (powerVsDefenseBonus / 10);

                        if (damage < 0)
                        {
                            damage = 1;
                        }
                    }
                    else
                    {
                        damage = damageStartValue + powerVsDefenseBonus;
                    }
                }
                else
                {
                    damage = 0;
                    enemyDodge = true;

                }
            
           


                





        }
    }

    void TakeWill(int willTake) // put this on a manager
    {
        enemyWill.willCurrent -= willTake;
    }

    void GiveDamage(float damage) // put on a manger
    {
        skillRollRandom = Random.Range(1, 1001);

        int skill = 25 + skillBonus;

        if (skill > 500)
        {
            skill = 500;
        }

        if (skill < 25)
        {
            skill = 25;
        }

        if(skill > skillRollRandom)
        {
            if (fightHuman)
            {

                playerHealth.currentHealth -= damage *2;
                gameManger.PostCritDamageCodeLeft();
                TakeWill(willTaken);
            }
            else
            {
                if (enemyDodge == false && enemyMiss == false)
                {

                    enemyHealth.currentHealth -= damage *2;
                    gameManger.PostCritDamageCodeLeft();
                    TakeWill(willTaken);
                    Debug.Log("Crit Attack");

                }
                if(enemyDodge == true && enemyMiss == false)
                {

                    gameManger.PostDodgeLeft();
                    Debug.Log("Dodge Right");
                    TakeWill(willTaken);
                    enemyDodge = false;
                }

                if(enemyMiss == true)
                {
                    gameManger.PostMissAttackLeft();
                    Debug.Log("Miss Attack");
                    TakeWill(willTaken);
                    enemyMiss = false;
                }

            }
        }
        else
        {
            if (fightHuman)
            {

                playerHealth.currentHealth -= damage;
                gameManger.PostDamageCodeLeft();
                TakeWill(willTaken);
            }
            else
            {
                if (enemyDodge == false && enemyMiss == false)
                {

                    enemyHealth.currentHealth -= damage;
                    gameManger.PostDamageCodeLeft();
                    TakeWill(willTaken);

                }
                if( enemyDodge == true && enemyMiss == false)
                {

                    gameManger.PostDodgeLeft();
                    TakeWill(willTaken);
                    Debug.Log("Dodge Right");
                    enemyDodge = false;
                }
                if(enemyMiss == true)
                {
                    gameManger.PostMissAttackLeft();
                    TakeWill(willTaken);
                    Debug.Log("Miss Attack");
                    enemyMiss = false;
                }

            }
        }

        


        attack = false;
    }

    void DistanceToEnemy()
    {
        distanceToPlayerSlider.value = (Mathf.Abs(targetGameobject.transform.position.x - transform.position.x)) / 20;
        sliderValue = distanceToPlayerSlider.value;
    }

    private void MoveForward()
    {

        if (timeForState > 0 && moveForward && (Mathf.Abs(targetGameobject.transform.position.x - transform.position.x)) > 4) // only move forward far away from enemy
        {
            //  Debug.Log("MoveForward");
            timeForState -= Time.fixedDeltaTime;
            Vector3 direction = (targetGameobject.transform.position - transform.position).normalized; // movement should be root motion and is kinematic
            rb.velocity = direction * speed;


        }
        else
        {
            moveForward = false;
        }

    }

    private void MoveBack()
    {


        if (timeForState > 0 && moveBackward && (Mathf.Abs(targetGameobject.transform.position.x - transform.position.x)) < 20 && (Mathf.Abs(rightEdge.transform.position.x - transform.position.x)) > 3) // move backward only if so far from enemy
        {
            //  Debug.Log("MoveBack");
            timeForState -= Time.fixedDeltaTime;
            Vector3 direction = (targetGameobject.transform.position - transform.position).normalized;
            rb.velocity = -direction * speed;
        }
        else
        {
            moveBackward = false;
        }



    }

    private void Wait()
    {

        if (timeForState > 0 && wait)
        {
            //  Debug.Log("Wait");
            timeForState -= Time.fixedDeltaTime;
            rb.velocity = new Vector3(0, 0, 0);
        }
        else
        {
            wait = false;
        }

    }

    private void Attack() // instead of time base on animation in future, can not take damage at this time
    {

        if (attack)
        {

            rb.velocity = new Vector3(0, 0, 0);
           // timeForState -= Time.deltaTime;

        //    if (sliderValue < .25f)
        //    {
        //        GiveDamage(damage);
       //         Debug.Log("Attack Close");
       //         attack = false;
       //     }

       //     if (sliderValue >= .25f && sliderValue < .50f)
      //      {
      //          GiveDamage(damage);
      //          Debug.Log("Attack Mid Close");
      //          attack = false;
      //      }

     //       if (sliderValue >= .50f && sliderValue < .75f)
     //       {
      //          GiveDamage(damage);
      //          Debug.Log("Attack Mid Far");
      //          attack = false;
    //        }

    //        if (sliderValue >= .75f)
    //        {
    //            GiveDamage(damage);
     //           Debug.Log("Attack Far");
     //           attack = false;
     //       }

            GiveDamage(damage);
            Debug.Log("Attack Far");
            attack = false;

            attackReset = .5f;
        }


    }

    public void NextState()
    {
        float resetTimeForState = 2;
        attackReset -= Time.fixedDeltaTime;

        if (!attack && !wait && !moveBackward && !moveForward && (Mathf.Abs(targetGameobject.transform.position.x - transform.position.x)) < 20 && (Mathf.Abs(targetGameobject.transform.position.x - transform.position.x)) > 4) // no change if enemy too close or too far
        {
            int newState = Random.Range(1, 101);

            // test insted of - * newstate
            if (newState <= 20 * attackbonus) //(20 - attackbonus))
            {
                wait = true;
            }

            if (newState > (20 * attackbonus) && newState <= (40 * attackbonus)) //(20 - attackbonus) && newState <= (40 - attackbonus))
            {
                moveBackward = true;
            }

            if (newState > (40 * attackbonus) && newState <= (60 * attackbonus))//(40 - attackbonus) && newState <= (60 - attackbonus))
            {
                moveForward = true;
            }

            if (newState > (60 * attackbonus) && attackReset < 0 && willToAttackBool)//(60 - attackbonus) && attackReset < 0)
            {
                //DamageToGive();
                attack = true;
                Attack();
            }

            timeForState = resetTimeForState;
        }

        if (!attack && !wait && !moveBackward && !moveForward && Mathf.Abs(targetGameobject.transform.position.x - transform.position.x) >= 20) // if enemy is too far away
        {
            moveForward = true;

            timeForState = resetTimeForState;
        }

        if (!attack && !wait && !moveBackward && !moveForward && (Mathf.Abs(targetGameobject.transform.position.x - transform.position.x)) <= 4) // add push back chance, where both players seperate. Also less chance for attack if last state was att
        {
            int newState = Random.Range(1, 101);

            if (newState <= (50 * attackbonus) && attackReset > 0)
            {
                moveBackward = true;
            }

            if (newState > (50 * attackbonus) && attackReset < 0 && willToAttackBool)
            {
                attack = true;
                Attack();
            }

            timeForState = resetTimeForState;

        }
    }


}


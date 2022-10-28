using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyMonsterMovement : MonoBehaviour
{
    [SerializeField] MakeMonsterSO thisMonsterStats;
    [SerializeField] MakeMonsterSO enemyMonsterStats;

    //Transform targetDestination;
    GameObject targetGameobject;
    [SerializeField] GameObject leftEdge;
    [SerializeField] Slider distanceToPlayerSlider;
    [SerializeField] float speed = 4; // get this from scriptable object in future
    GameManger gameManger;

                                      // the more health you have less attack you have, for less health of enemy more attack will be added for attackbonus;
    [SerializeField] int attackbonus = 0; // if attackbonus increases it takes down wait chance. max will be 25 attack. This gives 50% attack and 50% move. 
    int attackBonuseMultiplier = 3;
    Rigidbody rb;
    bool moveForward;
    bool moveBackward;
    bool wait;
    bool attack;

    public bool enemyDodge;

   
    [SerializeField] public bool fightHuman;

    public float damage = 50;

    [SerializeField] float timeForState = 2;
    float sliderValue;
    [SerializeField] EnemyHealth enemyHealth;
    PlayerHealth playerHealth;

    [SerializeField] float attackReset = .5f;

    EnemyHealth selfHealthEnemy;
    EnemyWill enemyWill;

   public int powerVsDefenseBonus; // = thisMonsterStats.power - enemyMonsterStats.defense; // sets increase to damage or decrease, can be 998+ to 998-
   public int dodgeBonus; // = thisMonsterStats.skill - enemyMonsterStats.speed; // sets increase to dodge or decreae, can be 998+ to 998-

   public int dodgeBase = 100; // base dodge if speed an skill, 100 should be 10%
   public int dodgeRollRandom; // =Random.Range(1, 1001); // what number will be choose to see if enemy dodges
                               // public int dodge; // = dodgeBase + dodgeBonus; // increase or decrease to dodge
    int dodge;

    public int hitChanceForUI;
    [SerializeField] TextMeshProUGUI hitChanceText;
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

    private void Awake()
    {
        enemyWill = GetComponent<EnemyWill>();
        rb = GetComponent<Rigidbody>();
        gameManger = FindObjectOfType<GameManger>();
        // targetGameobject = FindObjectOfType<MonsterMove>().gameObject;

        selfHealthEnemy = GetComponent<EnemyHealth>();

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
        DistanceToEnemy();
        AttackBonus();
        DamageToGive();

        hitChanceForUI = (hitChanceBonus * (100 - (dodge / 10))) / 100;

        if (hitChanceForUI > 100)
        {
            hitChanceForUI = 99;
        }

        if (hitChanceForUI < 0)
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

    void AttackBonus()
    {
        int increaseAttackOnEnemyHealth;
        int increaseAttackOnPlayerHealth;

        if (fightHuman)
        {
            increaseAttackOnEnemyHealth = (int)(playerHealth.maxHealth / playerHealth.currentHealth); // makes number bigger that 1 to mutiply with

            increaseAttackOnPlayerHealth = (int)(selfHealthEnemy.maxHealth / selfHealthEnemy.currentHealth);
            attackbonus = increaseAttackOnEnemyHealth * attackBonuseMultiplier + increaseAttackOnPlayerHealth * attackBonuseMultiplier*2 - 4;
        }
        else
        {
            increaseAttackOnEnemyHealth = (int)(enemyHealth.maxHealth / enemyHealth.currentHealth); // makes number bigger that 1 to mutiply with

            increaseAttackOnPlayerHealth = (int)(selfHealthEnemy.maxHealth / selfHealthEnemy.currentHealth);
            attackbonus = increaseAttackOnEnemyHealth * attackBonuseMultiplier + increaseAttackOnPlayerHealth * attackBonuseMultiplier*2 - 4;
        }

        if(attackbonus >= 20)
        {
            attackbonus = 20;
        }
    }

    void DamageToGive()
    {
        int damageStartValue;
        dodgeRollRandom = Random.Range(1, 1001);
        int hitChance;

        attackRandomRoll = Random.Range(1, 101);



        dodge = dodgeBase + dodgeBonus; // increase or decrease to dodge

        if (dodge < 0)
        {
            dodge = 0;
        }



        // close
        if (sliderValue < .25f)
        {
            willTaken = willCostClose;

            if(willCostClose > enemyWill.willCurrent)
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

                    if (damage < 0) // can not give negitive or 0 damage
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

        // mid close dodge miss and will

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

    void TakeWill(int willTake)
    {
        enemyWill.willCurrent -= willTake;
    }

    void GiveDamage(float damage)
    {
        skillRollRandom = Random.Range(1, 1001);

        int skill = 25 + skillBonus;

        if(skill > 500)
        {
            skill = 500;
        }

        if (skill < 25)
        {
            skill = 25;
        }

        if (skill > skillRollRandom)
        {
            if (fightHuman)
            {

                playerHealth.currentHealth -= damage * 2;
                gameManger.PostCritDamageCodeRight();
                TakeWill(willTaken);
            }
            else
            {
                if (enemyDodge == false && enemyMiss == false)
                {

                    enemyHealth.currentHealth -= damage * 2;
                    gameManger.PostCritDamageCodeRight();
                    TakeWill(willTaken);

                }
                if( enemyDodge == true && enemyMiss == false)
                {
                    gameManger.PostDodgeRight();
                    Debug.Log("Dodge Left Monster");
                    enemyDodge = false;
                    TakeWill(willTaken);
                }
                if(enemyMiss == true)
                {
                    gameManger.PostMissAttackRight();
                    Debug.Log("Miss Attack");
                    enemyMiss = false;
                    TakeWill(willTaken);
                }                                             

            }
        }
        else
        {
            if (fightHuman)
            {

                playerHealth.currentHealth -= damage;
                gameManger.PostDamageCodeRight();
                TakeWill(willTaken);
            }
            else
            {
                if (enemyDodge == false && enemyMiss == false)
                {

                    enemyHealth.currentHealth -= damage;
                    gameManger.PostDamageCodeRight();
                    TakeWill(willTaken);

                }
                if (enemyDodge == true && enemyMiss == false)
                {

                    gameManger.PostDodgeRight();
                    Debug.Log("Dodge Right");
                    enemyDodge = false;
                    TakeWill(willTaken);
                }
                if (enemyMiss == true)
                {
                    gameManger.PostMissAttackRight();
                    Debug.Log("Miss Attack");
                    enemyMiss = false;
                    TakeWill(willTaken);
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
       
        if(timeForState > 0 && moveForward && (Mathf.Abs(targetGameobject.transform.position.x - transform.position.x) ) > 4) // only move forward far away from enemy
        {
          
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
       
       
            if (timeForState > 0 && moveBackward && (Mathf.Abs(targetGameobject.transform.position.x - transform.position.x)) < 20 && (Mathf.Abs(leftEdge.transform.position.x - transform.position.x)) > 3) // move backward only if so far from enemy
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
       
        if (attack && willToAttackBool == true) // added noWilltoAttack
        {         

            rb.velocity = new Vector3(0, 0, 0);
            timeForState -= Time.deltaTime;

      

            GiveDamage(damage);
            Debug.Log("Attack Close");
            attack = false;

            attackReset = .5f;
        }
       
               
    }

    public void NextState() 
    {
        float resetTimeForState = 2;
        attackReset -= Time.fixedDeltaTime;

        if (!attack && !wait && !moveBackward && !moveForward &&( Mathf.Abs(targetGameobject.transform.position.x - transform.position.x)) < 20 && (Mathf.Abs(targetGameobject.transform.position.x - transform.position.x)) > 4) // no change if enemy too close or too far
        {
            int newState = Random.Range(1, 101);
                               
            if(newState <=(20 - attackbonus))
            {
                wait = true;             
            }

            if(newState >(20 -attackbonus) && newState <=(40 - attackbonus))
            {
                moveBackward = true;
            }

            if(newState > (40- attackbonus) && newState <= (60-attackbonus))
            {
                moveForward = true;
            }

            if(newState >(60- attackbonus) && attackReset < 0)
            {                
                attack = true;
                Attack();
            }

            timeForState = resetTimeForState;
        }

        if(!attack && !wait && !moveBackward && !moveForward && Mathf.Abs(targetGameobject.transform.position.x - transform.position.x) >= 20) // if enemy is too far away
        {
            moveForward = true;

            timeForState = resetTimeForState;
        }

        if(!attack && !wait && !moveBackward && !moveForward && (Mathf.Abs(targetGameobject.transform.position.x - transform.position.x)) <= 4) // add push back chance, where both players seperate. Also less chance for attack if last state was att
        {
            int newState = Random.Range(1, 101);

            if(newState <= (50 - attackbonus))
            {
                moveBackward = true;
            }

            if(newState > (50 - attackbonus) && attackReset < 0)
            {
                attack = true;
                Attack();
            }

            timeForState = resetTimeForState;

        }
    }


}

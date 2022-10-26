using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System;

public class EnemyMovementStartRight : MonoBehaviour
{
    //Transform targetDestination;
    GameObject targetGameobject;
    [SerializeField] GameObject rightEdge;
    [SerializeField] Slider distanceToPlayerSlider;
    [SerializeField] float speed = 4; // get this from scriptable object in future

    // if player has more health from start less attacking, if enemy has less health from start increase attacking. change the attackbonus
    [SerializeField] int attackbonus = 10; // if attackbonus increases it takes down wait chance. max will be 25 attack. This gives 50% attack and 50% move. 
    Rigidbody rb;
    bool moveForward;
    bool moveBackward;
    bool wait;
    bool attack;

    int attackBonuseMultiplier = 3;
    [SerializeField] bool fightHuman;

    [SerializeField] float damageEditor = 50;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // targetGameobject = FindObjectOfType<MonsterMove>().gameObject;
        selfHealthEnemy = GetComponent<EnemyHealth>();

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
    }

    // Update is called once per frame
    void Update()
    {
       
        DistanceToEnemy();
        AttackBonus();


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

        if (attackbonus >= 20)
        {
            attackbonus = 20;
        }
    }

    void GiveDamage(float damage)
    {
       
        if (fightHuman)
        {
            playerHealth.currentHealth -= damage;
        }
        else
        {
            enemyHealth.currentHealth -= damage;
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
         //   Debug.Log("MoveForward Right");
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
             //   Debug.Log("MoveBack Right");
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
          //  Debug.Log("Wait Right");
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
            

            if (sliderValue < .25f)
            {
                GiveDamage(damageEditor);
                Debug.Log("Attack Close Right");
                attack = false;
            }

            if (sliderValue >= .25f && sliderValue < .50f)
            {
                GiveDamage(damageEditor);
                Debug.Log("Attack Mid Close Right");
                attack = false;
            }

            if (sliderValue >= .50f && sliderValue < .75f) 
            {
                GiveDamage(damageEditor);
                Debug.Log("Attack Mid Far Right");
                attack = false;
            }

            if (sliderValue >= .75f)
            {
                GiveDamage(damageEditor);
                Debug.Log("Attack Far Right");
                attack = false;
            }

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

            if (newState <= (20 - attackbonus))
            {
                wait = true;
            }

            if (newState > (20 - attackbonus) && newState <= (40 - attackbonus))
            {
                moveBackward = true;
            }

            if (newState > (40 - attackbonus) && newState <= (60 - attackbonus))
            {
                moveForward = true;
            }

            if (newState > (60 - attackbonus) && attackReset < 0)
            {
                
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

            if (newState <= (50 - attackbonus))
            {
                moveBackward = true;
            }

            if (newState > (50 - attackbonus) && attackReset < 0)
            {
                attack = true;
                Attack();
            }

            timeForState = resetTimeForState;

        }
    }


}


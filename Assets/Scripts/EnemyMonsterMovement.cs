using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMonsterMovement : MonoBehaviour
{
    //Transform targetDestination;
    GameObject targetGameobject;
    [SerializeField] GameObject leftEdge;
    [SerializeField] Slider distanceToPlayerSlider;
    [SerializeField] float speed = 4; // get this from scriptable object in future

                                      // the more health you have less attack you have, for less health of enemy more attack will be added for attackbonus;
    [SerializeField] int attackbonus = 0; // if attackbonus increases it takes down wait chance. max will be 25 attack. This gives 50% attack and 50% move. 
    int attackBonuseMultiplier = 3;
    Rigidbody rb;
    bool moveForward;
    bool moveBackward;
    bool wait;
    bool attack;

   
    [SerializeField] bool fightHuman;

    [SerializeField] float damage = 50;

    [SerializeField] float timeForState = 2;
    float sliderValue;
    [SerializeField] EnemyHealth enemyHealth;
    PlayerHealth playerHealth;

    [SerializeField] float attackReset = .5f;

    EnemyHealth selfHealthEnemy;
    EnemyWill enemyWill;



    private void Awake()
    {
        enemyWill = GetComponent<EnemyWill>();
        rb = GetComponent<Rigidbody>();
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
        
    }

    // Update is called once per frame
    void Update()
    {
       // NextState();
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

        if(attackbonus >= 20)
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
       
        if(timeForState > 0 && moveForward && (Mathf.Abs(targetGameobject.transform.position.x - transform.position.x) ) > 4) // only move forward far away from enemy
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
       
        if (attack)
        {
            
            rb.velocity = new Vector3(0, 0, 0);
            timeForState -= Time.deltaTime;

            if (sliderValue < .25f)
            {
                GiveDamage(damage);
                Debug.Log("Attack Close");
                attack = false;
            }

            if(sliderValue >=.25f && sliderValue <.50f)
            {
                GiveDamage(damage);
                Debug.Log("Attack Mid Close");
                attack = false;
            }

            if(sliderValue >=.50f && sliderValue <.75f)
            {
                GiveDamage(damage);
                Debug.Log("Attack Mid Far");
                attack = false;
            }

            if(sliderValue >=.75f)
            {
                GiveDamage(damage);
                Debug.Log("Attack Far");
                attack = false;
            }

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

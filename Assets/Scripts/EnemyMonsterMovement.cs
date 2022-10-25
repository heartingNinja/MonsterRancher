using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMonsterMovement : MonoBehaviour
{
    //Transform targetDestination;
    GameObject targetGameobject;
    [SerializeField] float speed = 4; // get this from scriptable object in future

                                      // if player has more health from start less attacking, if enemy has less health from start increase attacking. change the attackbonus
    [SerializeField] int attackbonus = 10; // if attackbonus increases it takes down wait chance. max will be 25 attack. This gives 50% attack and 50% move. 
    Rigidbody rb;
    bool moveForward;
    bool moveBackward;
    bool wait;
    bool attack;

    [SerializeField] float timeForState = 2;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        targetGameobject = FindObjectOfType<MonsterMove>().gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        NextState();
       
    }

    private void FixedUpdate()
    {
        MoveForward();
        MoveBack();
        Wait();
        Attack();
    }

    public void MoveForward()
    {
       
        if(timeForState > 0 && moveForward && (Mathf.Abs(targetGameobject.transform.position.x - transform.position.x) ) > 4) // only move forward far away from enemy
        {
            Debug.Log("MoveForward");
            timeForState -= Time.fixedDeltaTime;
            Vector3 direction = (targetGameobject.transform.position - transform.position).normalized; // movement should be root motion and is kinematic
            rb.velocity = direction * speed;     
           

        }
        else
        {
            moveForward = false;
        }
        
    }

    public void MoveBack()
    {
       
        if (timeForState > 0 && moveBackward && (Mathf.Abs(targetGameobject.transform.position.x - transform.position.x)) < 20) // move backward only if so far from enemy
        {
            Debug.Log("MoveBack");
            timeForState -= Time.fixedDeltaTime;
            Vector3 direction = (targetGameobject.transform.position - transform.position).normalized;
            rb.velocity = -direction * speed;        
        }
        else
        {
            moveBackward = false;
        }
        
    }

    public void Wait()
    {
      
        if (timeForState > 0 && wait)
        {
            Debug.Log("Wait");
            timeForState -= Time.fixedDeltaTime;
            rb.velocity = new Vector3(0, 0, 0);           
        }
        else
        {
            wait = false;
        }
           
    }

    public void Attack() // instead of time base on animation in future, can not take damage at this time
    {
       
        if (timeForState > 0 && attack)
        {
            Debug.Log("Attack");
            rb.velocity = new Vector3(0, 0, 0);
            timeForState -= Time.deltaTime;

        }
        else
        {
            attack = false;
        }
               
    }

    public void NextState() 
    {
        float resetTimeForState = 2;

        if (!attack && !wait && !moveBackward && !moveForward &&( Mathf.Abs(targetGameobject.transform.position.x - transform.position.x)) < 20 && (Mathf.Abs(targetGameobject.transform.position.x - transform.position.x)) > 4) // no change if enemy too close or too far
        {
            int newState = Random.Range(1, 101);
                               
            if(newState <=(25 - attackbonus))
            {
                wait = true;             
            }

            if(newState >(25 -attackbonus) && newState <=(50 - attackbonus))
            {
                moveBackward = true;
            }

            if(newState > (50- attackbonus) && newState <= (75-attackbonus))
            {
                moveForward = true;
            }

            if(newState >(75- attackbonus))
            {
                attack = true;
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

            if(newState > (50 - attackbonus))
            {
                attack = true;
            }

            timeForState = resetTimeForState;

        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] Rigidbody rb;
    [SerializeField] Slider distanceToEnemySlider;
    GameObject enemy;
    [SerializeField] Button closeAttack;
    [SerializeField] Button midCloseAttack;
    [SerializeField] Button midFarAttack;
    [SerializeField] Button farAttack;

    Vector3 movement;
    float enemyDistance;

    private void Awake()
    {
        enemy = FindObjectOfType<EnemyMonsterMovement>().gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        DistanceToEnemy();
        movement.x = Input.GetAxisRaw("Horizontal");
        AttackButtons();
    }

    private void FixedUpdate()
    {

        Vector3 direction = new Vector3(movement.x, 0, 0); // not kinesmatic
        // rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime); //change so is not kinesmatic. Right now can not hit back wall. OR keep and have a limit on where it can move on the map
        rb.velocity = direction * moveSpeed; // not kinesmatic
    }

    void DistanceToEnemy()
    {
        distanceToEnemySlider.value = (Mathf.Abs(enemy.transform.position.x - transform.position.x)) / 20;
        enemyDistance = distanceToEnemySlider.value;
    }

     void AttackButtons()
    {
        if (enemyDistance < .25f)
        {
            closeAttack.enabled = true;
            midCloseAttack.enabled = false;
            midFarAttack.enabled = false;
            farAttack.enabled = false;

           // Debug.Log("Attack Close");
        }

        if (enemyDistance >= .25f && enemyDistance < .50f)
        {
            closeAttack.enabled = false;
            midCloseAttack.enabled = true;
            midFarAttack.enabled = false;
            farAttack.enabled = false;

           // Debug.Log("Attack Mid Close");
        }

        if (enemyDistance >= .50f && enemyDistance < .75f)
        {
            // Debug.Log("Attack Mid Far");
            closeAttack.enabled = false;
            midCloseAttack.enabled = false;
            midFarAttack.enabled = true;
            farAttack.enabled = false;
        }

        if (enemyDistance >= .75f)
        {
            closeAttack.enabled = false;
            midCloseAttack.enabled = false;
            midFarAttack.enabled = false;
            farAttack.enabled = true;

            // Debug.Log("Attack Far");
        }
    }


}

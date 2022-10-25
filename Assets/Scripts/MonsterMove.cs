using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] Rigidbody rb;

    Vector3 movement;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {

        Vector3 direction = new Vector3(movement.x, 0, 0); // not kinesmatic
        // rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime); //change so is not kinesmatic. Right now can not hit back wall. OR keep and have a limit on where it can move on the map
        rb.velocity = direction * moveSpeed; // not kinesmatic
    }
}

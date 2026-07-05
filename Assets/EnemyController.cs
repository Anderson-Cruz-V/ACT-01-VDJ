using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyController:MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 4.0f;
    public float speed = 2.0f;

    private Rigidbody2D rd;
    private Vector2 movement;
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        float distaceTopayer =  Vector2.Distance( transform.position, player.position );
        if (distaceTopayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            
            movement = new Vector2 (direction.x,0);
        }

        else
        {
            movement = Vector2.zero;
        }
        rd.MovePosition(rd.position+ movement*speed*Time.deltaTime);
    }

}
    
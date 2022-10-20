using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public int damage;

    private SpriteRenderer sprite;
    private bool movingToEnd; //moving to end position
    private bool upDown = false;


    private void Start()
    {
        // No hace falta el find porque el componente esta metido dentro del sprite
        sprite = gameObject.transform.Find("crab-idle-1").GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        movingToEnd = true;
        
        if ((int)startPosition.x < (int)endPosition.x)
        {
            sprite.flipX = true;
        } else if (startPosition.x > endPosition.x)
        {
            sprite.flipX = false;
        }
        else
        {
            upDown = true;
        }
    }

    private void Update()
    {

        EnemyMove();
        
    }

    void EnemyMove()
    {
        //calculate the destination in order to movingToEnd
        Vector3 targetPosition = (movingToEnd) ? endPosition : startPosition;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition , speed * Time.deltaTime);
        // when arrive to the End
        if (transform.position == targetPosition)
        {
            movingToEnd = !movingToEnd;
            if (!upDown) sprite.flipX = !sprite.flipX;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Player Info")]
    public int speed;
    public int jumpForce;
    public int lives;
    public float levelTime; //seconds
    public Canvas canvas;

    private Rigidbody2D rb;
    private GameObject foot;
    private bool IsJumping;
    private SpriteRenderer sprite;
    private Animator animator;
    private HudController hud;

    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //find the reference to the foot gameobject child
        foot = transform.Find("foot").gameObject; 

        sprite = gameObject.transform.Find("player-idle-1").GetComponent<SpriteRenderer>();
        //get the animator controller of the sprite child object
        animator = gameObject.transform.Find("player-idle-1").GetComponent<Animator>();

        //get the HUD controller
        hud = canvas.GetComponent<HudController>();
        hud.SetLivesText(lives);
        hud.SetPowerUpsTxt(GameObject.FindGameObjectsWithTag("PowerUp").Length);

    }

    private void FixedUpdate()
    {
        //get the value from -1 to 1 of the horizontal move
        float inputX = Input.GetAxis("Horizontal");
        //apply phisic velocity to the object with the move value * speed
        //the y coordenate is the same
        rb.velocity = new Vector2 (inputX * speed, rb.velocity.y);

        if (levelTime <= 0)
        {
            Debug.Log("YOU LOOSE the time is up");
            winLevel(false);
        }
        else
        {
            levelTime -= Time.deltaTime;
            hud.SetTimeText((int)levelTime);
        }
        
        
    }

    private void Update()
    {
        //pressing space and touching the ground
        if (Input.GetKeyDown(KeyCode.Space) && TouchGround())
        {
            IsJumping = true;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (rb.velocity.x > 0) sprite.flipX = false;
        else if (rb.velocity.x < 0) sprite.flipX = true;

        //play animations
        PlayerAnimate();

        //Calculate if the time has ended
        
    }
    /// <summary>
    /// Check if touching the ground
    /// </summary>
    /// <returns>if touching or not</returns>
    private bool TouchGround()
    {
        //Send a imaginary line down 0.2f distance 
        RaycastHit2D hit = Physics2D.Raycast(foot.transform.position , Vector2.down, 0.2f);
        
        // touching something
        return hit.collider != null;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsJumping = false;
        }
    }
    /// <summary>
    /// Reduce lives from the player
    /// </summary>
    /// <param name="damage"> number of damage</param>
    public void TakeDamage(int damage)
    {
        lives -= damage;
        Debug.Log("Player Lives: "+lives);
        hud.SetLivesText(lives);
        if (lives == 0)
        {
            winLevel(false);
            //TODO:change escene
            Debug.Log("LOSE!!!!");
        }
    }

    private void PlayerAnimate()
    {
        //player jumping
        if (!TouchGround()) animator.Play("playerJump");
        //player running (touching ground and not touching horizontal input keys)
        else if (TouchGround() && Input.GetAxisRaw("Horizontal") != 0) animator.Play("playerRunning");
        //player idle (touching ground and touching horizontal input keys)
        else if (TouchGround() && Input.GetAxisRaw("Horizontal") == 0) animator.Play("playerIdle");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            //destroy the PowerUp
            Destroy(collision.gameObject);
            //and after 0.1 seconds show the info
            Invoke(nameof(InfoPowerUp), 0.1f);
            
            
           
        }
    }

    private void InfoPowerUp()
    {

        int powerUpsNum = GameObject.FindGameObjectsWithTag("PowerUp").Length;

        //TODO:write in HUD how many PowerUps are left
        Debug.Log("PowerUps: " + powerUpsNum);
        hud.SetPowerUpsTxt(powerUpsNum);

        if (powerUpsNum == 0)
        {
            winLevel(true);
            GameManager.instance.Win = true;
            //TODO: Change Scene WIN
            Debug.Log("WIN!!!!");
        }
    }

    private void winLevel(bool win)
    {
        GameManager.instance.Win = win;
        GameManager.instance.Score = (lives * 1000) + ((int)levelTime * 100);

    }
}

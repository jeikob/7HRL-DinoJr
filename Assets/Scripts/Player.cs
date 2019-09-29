using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int hpPerHeart = 1;
    public float restartLevelDelay = 1f;
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;

    private Animator animator;
    public int hp;

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        hp = GameManager.instance.playerHp;
        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerHp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle User Input in here
        // Handle User Input in here
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            //AttemptMove<Wall>(movement.x, movement.y);
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    // Fixed Update is called at a fixed time interval rather than being tied to framerate
    void FixedUpdate()
    {
        // Handle Player Movement in here
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

    }

    protected override void AttemptMove <T> (float xDir, float yDir)
    {
        base.AttemptMove<T>(xDir, yDir);
        //RaycastHit2D hit;
        CheckIfGameOver();
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            hp += hpPerHeart;
            other.gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseHp (int hpLoss)
    {
        hp -= hpLoss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (hp <= 0)
            GameManager.instance.GameOver();
    }
}

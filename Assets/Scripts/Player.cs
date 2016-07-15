using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class Player : MovingObject {

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator animator;
    private int food;


	protected override void Start () {

        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;
        base.Start();
	}

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }
	
	void Update () {
        if (!GameManager.instance.playerTurn) return;

        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0) vertical = 0;
        if (horizontal != 0 || vertical != 0)
            AttemptedMove<Wall>(horizontal, vertical);	
	}

    protected override void AttemptedMove<T>(int xDir, int yDir)
    {
        food--;
        base.AttemptedMove<T>(xDir, yDir);
        // If statement for audio here maybe?
        CheckIfGameOver();
        GameManager.instance.playerTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        animator.SetTrigger("playerChop");
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Probably should change "food" and "soda" to a single tag
        // and include point value on prefab as public value
        if(other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        } else if(other.tag == "Food")
        {
            food += pointsPerFood;
            other.gameObject.SetActive(false);                                                                                        
        } else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0) GameManager.instance.GameOver();
    }
}

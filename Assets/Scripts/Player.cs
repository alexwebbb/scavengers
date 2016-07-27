using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject {

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;

    private int playerChopID;
    private int playerHitID;

    private Animator animator;
    private int food;

	protected override void Start () {

        playerChopID = Animator.StringToHash("playerChop");
        playerHitID = Animator.StringToHash("playerHit");

        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;

        foodText.text = "Food: " + food;

        base.Start();
	}

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }
	
	void Update () {
        if (!GameManager.instance.playersTurn) return;

        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0) vertical = 0;
        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);	
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        foodText.text = "Food: " + food;

        base.AttemptMove<T>(xDir, yDir);
        // If statement for audio here maybe?
        CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        animator.SetTrigger(playerChopID);
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
            foodText.text = "+" + pointsPerFood + " Food: " + food;
            other.gameObject.SetActive(false);                                                                                        
        } else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + food;
            other.gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger(playerHitID);
        food -= loss;
        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0) GameManager.instance.GameOver();
    }
}

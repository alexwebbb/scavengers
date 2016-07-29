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

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

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

        // This raycast initialization.... there has got to be
        // a better way....
        RaycastHit2D hit;
        if (Move(xDir, yDir, out hit))
            SoundManager.instance.RandomizeSoundEffects(moveSound1, moveSound2);

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
            SoundManager.instance.RandomizeSoundEffects(eatSound1, eatSound2);
            other.gameObject.SetActive(false);                                                                                        
        } else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + food;
            SoundManager.instance.RandomizeSoundEffects(drinkSound1, drinkSound2);
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
        if (food <= 0)
        {
            SoundManager.instance.musicSource.Stop();
            SoundManager.instance.RandomizeSoundEffects(gameOverSound);
            GameManager.instance.GameOver();
        }
    }
}

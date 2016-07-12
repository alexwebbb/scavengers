using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider2d;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

	
	protected virtual void Start () {

        boxCollider2d = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();

        inverseMoveTime = 1f / moveTime;
	
	}

    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = new Vector2(xDir, yDir);

        boxCollider2d.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider2d.enabled = true;

        if (hit.transform = null)
        {
            // Left off at 8 minutes in the video
        }
    }

    protected IEnumerator SmoothMovement (Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;


	
	// Update is called once per frame
	void Update () {
	
	}
}

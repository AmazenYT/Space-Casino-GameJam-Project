using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    Vector2 checkpointPos;
    SpriteRenderer spriteRenderer;
    PlayerMovement playerMovement;
    Rigidbody2D playerbody;

    private void Awake()
    {
        playerbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        checkpointPos = transform.position;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
       
        if (collisionInfo.collider.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Die();
        }
        else if (collision.CompareTag("KillPlane"))
        {
            Die();
        }
    }

    void Die()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        StartCoroutine(Respawn(0.5f)); 
    }

    public void UpdateCheckpoint(Vector2 pos)
    {
        checkpointPos = pos;
    }

    System.Collections.IEnumerator Respawn(float duration)
    {
        spriteRenderer.enabled = false; 
        playerbody.velocity = Vector2.zero;  
        yield return new WaitForSeconds(duration);  
        transform.position = checkpointPos;  
        spriteRenderer.enabled = true;  

        if (playerMovement != null)
        {
            playerMovement.enabled = true; 
        }
    }
}

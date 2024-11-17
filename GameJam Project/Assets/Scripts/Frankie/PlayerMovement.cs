using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;

    [Header("Player Settings")]
    public float speed;
    public float maxSpeed = 20f;
    public float playerRotationSpeed = 2f;
    public float jumpHeight;

    [Header("Gravity Settings")]
    public float gravity = -1f;
    public bool moonGravity = false;
    public float downwardForce;
    public float moonDownwardForce;

    [Header("Grounding Settings")]
    public float groundCheckDistance = 1f;
    public float groundCheckOffset = 0.25f;
    public LayerMask groundLayer;
    public float slopeAngle;

    [Header("Sound Settings")]
    public AudioSource audioSource;        // The AudioSource component to play sounds
    public AudioClip jumpSound;            // The sound effect for jumping

    // Private variables
    private Rigidbody2D body;
    private bool grounded;
    private RaycastHit2D hit;
    private float defaultGroundCheckDistance;
    private float Move;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // Ensure the player object has an AudioSource attached
        Application.targetFrameRate = 60;
        defaultGroundCheckDistance = groundCheckDistance;
    }

    private void HandlePlayerVelocity(bool right)
    {
        if (right)
        {
            body.AddForce(transform.right * speed);
        }
        else
        {
            body.AddForce(-transform.right * speed);
        }
    }

    private void Update()
    {
        Move = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
            GroundCheckZero();
            Invoke(nameof(GroundCheckDefault), 0.5f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            HandlePlayerVelocity(true);
        }

        if (Input.GetKey(KeyCode.A))
        {
            HandlePlayerVelocity(false);
        }

        if (Move >= 0.1f || Move <= -0.1f)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if (Move > 0)
        {
            gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }

        if (Move < 0)
        {
            gameObject.transform.localScale = new Vector3(-0.5f, 0.5f, 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("isJumping", false);
        }
    }

    private void FixedUpdate()
    {
        body.velocity = Vector3.ClampMagnitude(body.velocity, maxSpeed);

        if (!moonGravity)
        {
            if (IsGrounded())
            {
                gravity = 0f;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, slopeAngle), Time.fixedDeltaTime * playerRotationSpeed);
            }
            else
            {
                gravity = 5f;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.fixedDeltaTime * playerRotationSpeed);
            }
        }
        if (!IsGrounded())
        {
            body.AddForce(-transform.up * gravity);
        }
    }

    private void GroundCheckZero()
    {
        groundCheckDistance = 0f;
    }

    private void GroundCheckDefault()
    {
        groundCheckDistance = defaultGroundCheckDistance;
    }

    private void Jump()
    {
        body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        grounded = false;

        if (!grounded)
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }

        // Play the jump sound
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    public bool IsGrounded()
    {
        return RaycastFromGroundCheck(-transform.up, groundCheckDistance, out hit);
    }

    protected bool RaycastFromGroundCheck(Vector2 direction, float distance, out RaycastHit2D hitInfo)
    {
        hitInfo = Physics2D.Raycast((Vector2)transform.position + Vector2.up * groundCheckOffset, direction, distance, groundLayer);
        bool hit = hitInfo.collider != null;

        if (hit)
        {
            slopeAngle = Vector2.Angle(hitInfo.normal, Vector2.up);
        }

        Color rayColor = hit ? Color.green : Color.red;
        Debug.DrawRay((Vector2)transform.position + Vector2.up * groundCheckOffset, direction * distance, rayColor);

        return hit;
    }
}


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
   
    

    // Private variables
    private Rigidbody2D body;
    private bool grounded;
    private RaycastHit2D hit;
    private float defaultGroundCheckDistance;
    private float Move;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
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
            Invoke(nameof(GroundCheckDefault),0.5f);

        }
        if (Input.GetKey(KeyCode.D))
        {
            HandlePlayerVelocity(true);
        }

        if (Input.GetKey(KeyCode.A))
        {
            HandlePlayerVelocity(false);
        }

        if(Move >= 0.1f || Move <= -0.1f)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        /*if (!grounded)
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }
        */
        if (Move > 0)
        {
            gameObject.transform.localScale = new Vector3((float)0.5, (float)0.5, (float)1);
        }

        if (Move < 0)
        {
            gameObject.transform.localScale = new Vector3((float)-0.5, (float)0.5, (float)1);
        }


    }

     private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("isJumping", false);
        }
    }

    /*private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("isJumping", true);        
        }
    }
    */




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
        //If you switch true and false around the player will run and idle with animation. he can jump but has no animationSS
        else
        {
            anim.SetBool("isJumping", false);
        }

        /*if (Input.GetButtonDown("Space"))
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpHeight));
        }*/




        // body.velocity = new Vector2(body.velocity.x, jumpHeight);
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
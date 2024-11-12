
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    public string axis = "Horizontal";
    public KeyCode jumpButton = KeyCode.Space;
    private Rigidbody2D body;
    private bool grounded;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        body.velocity = new Vector2(Input.GetAxis(axis) * speed, body.velocity.y);
        if (Input.GetKey(jumpButton) && grounded)
        {
            Jump();
        }
    }
    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpHeight);
        grounded = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }
}
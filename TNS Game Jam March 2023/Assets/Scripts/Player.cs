using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] BoxCollider2D feetCollider;
    [SerializeField] SpriteRenderer mySprite;
    [SerializeField] Animator myAnim;

    public InputAction playerControls;
    public bool isGrounded;
    public float runSpeed;
    public float jumpSpeed;

    Rigidbody2D myRB;
    BoxCollider2D bodyCollider;
    

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();

        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }
    }

    void Run() {
        float controlThrow = Input.GetAxisRaw("Horizontal");

        if (isGrounded && controlThrow != 0f) { //running anim
            myAnim.SetBool("isWalking", true);
        } else {
            myAnim.SetBool("isWalking", false);
        }

        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            if (controlThrow > 0.5f) {
                mySprite.transform.localScale = new Vector2(1f, 1f);
            } else if (controlThrow < -0.5f) {
                mySprite.transform.localScale = new Vector2(-1f, 1f);
            }
        }

        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRB.velocity.y);
        myRB.velocity = playerVelocity;

    }

    void Jump() {
        if (isGrounded && Input.GetButtonDown("Jump")) {
            myRB.velocity = new Vector2(myRB.velocity.x, jumpSpeed);
        }

        if (Input.GetButtonUp("Jump")) {
            Vector2 jumpVelocityToSub = new Vector2(0f, jumpSpeed / 3f);
            myRB.velocity -= jumpVelocityToSub;
        }
    }
}

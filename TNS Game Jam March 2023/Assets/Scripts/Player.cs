using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{
    [SerializeField] BoxCollider2D feetCollider;
    [SerializeField] SpriteRenderer mySprite;
    [SerializeField] Animator myAnim;

    public InputMaster playerControls;
    public bool isGrounded;
    public float runSpeed;
    public float jumpSpeed;
    public float throwTimer;
    public float throwThreshold;

    Rigidbody2D myRB;
    BoxCollider2D bodyCollider;

    Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private InputAction jump;
    private InputAction upAction;
    private InputAction leftAction;
    private InputAction rightAction;
    private InputAction pause;
    private InputAction recall;
    private InputAction leaveShield;


    private void Awake() {
        playerControls = new InputMaster();
    }

    private void OnEnable() {
        move = playerControls.Player.Movement;
        move.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;

        upAction = playerControls.Player.UpAction;
        upAction.Enable();
        upAction.performed += UpAction;

        leftAction = playerControls.Player.LeftAction;
        leftAction.Enable();
        leftAction.performed += LeftAction;

        rightAction = playerControls.Player.RightAction;
        rightAction.Enable();
        rightAction.performed += RightAction;

        pause = playerControls.Player.Pause;
        pause.Enable();
        pause.performed += Pause;

        recall = playerControls.Player.Recall;
        recall.Enable();
        recall.performed += Recall;

        leaveShield = playerControls.Player.LeaveShield;
        leaveShield.Enable();
        leaveShield.performed += LeaveShield;
    }

    private void OnDisable() {
        move.Disable();
        jump.Disable();
        upAction.Disable();
        leftAction.Disable();
        rightAction.Disable();
        pause.Disable();
        recall.Disable();
        leaveShield.Disable();
    }

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
        //Jump();

        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }

        throwTimer += Time.deltaTime;
    }

    void Run() {
        //float controlThrow = Input.GetAxisRaw("Horizontal");
        moveDirection = move.ReadValue<Vector2>();

        if (isGrounded && Mathf.Abs(moveDirection.x) >= 0.5f) { //running anim
            myAnim.SetBool("isWalking", true);
        } else {
            myAnim.SetBool("isWalking", false);
        }

        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            if (moveDirection.x > 0.5f) {
                mySprite.transform.localScale = new Vector2(1f, 1f);
            } else if (moveDirection.x < -0.5f) {
                mySprite.transform.localScale = new Vector2(-1f, 1f);
            }
        }

        if (Mathf.Abs(moveDirection.x) >= 0.5f) { //artificial deadzone
            Vector2 playerVelocity = new Vector2(moveDirection.x * runSpeed, myRB.velocity.y);
            myRB.velocity = playerVelocity;
        } else {
            myRB.velocity = new Vector2(0f, myRB.velocity.y);
        }
        

    }

    void Jump(InputAction.CallbackContext context) {
        if (isGrounded) {
            myRB.velocity = new Vector2(myRB.velocity.x, jumpSpeed);
        }

        if (jump.WasReleasedThisFrame()) {
            Vector2 jumpVelocityToSub = new Vector2(0f, jumpSpeed / 3f);
            myRB.velocity -= jumpVelocityToSub;
        }
    }

    void UpAction(InputAction.CallbackContext context) {

        if (context.interaction is PressInteraction) {
            if (upAction.WasPressedThisFrame()) {
                Debug.Log("Y pressed");
            }

            if (upAction.WasReleasedThisFrame()) {
                Debug.Log("Y released");
            }
        }

        
    }

    void LeftAction(InputAction.CallbackContext context) {

        if (leftAction.WasPressedThisFrame()) { //and has shield
            throwTimer = 0f;
            Debug.Log("X pressed");
        }

        if (leftAction.WasReleasedThisFrame()) {
            Debug.Log("X released");
            if (throwTimer <= throwThreshold) {
                Debug.Log("throw shield");
            }
        }
    }

    void RightAction(InputAction.CallbackContext context) {

        if (rightAction.WasPressedThisFrame()) { //and has shield
            throwTimer = 0f;
            Debug.Log("B pressed");
        }

        if (rightAction.WasReleasedThisFrame()) {
            Debug.Log("B released");
            if (throwTimer <= throwThreshold) {
                Debug.Log("throw shield");
            }
        }
    }

    void Pause(InputAction.CallbackContext context) {
        Debug.Log("pause pressed");
        PauseGame();
    }

    void Recall(InputAction.CallbackContext context) {
        Debug.Log("recall pressed");
    }

    void LeaveShield(InputAction.CallbackContext context) {
        Debug.Log("leave shield pressed");
    }

    void ThrowShield() {
    
    }

    void PauseGame() {
        
    }

}

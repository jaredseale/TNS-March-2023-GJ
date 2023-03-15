using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{
    [SerializeField] Shield shield;
    [SerializeField] BoxCollider2D feetCollider;
    public GameObject spriteObject;
    [SerializeField] SpriteRenderer mySprite;
    [SerializeField] Animator myAnim;
    [SerializeField] GameObject thrownShieldPrefab;
    [SerializeField] GameObject droppedShieldPrefab;
    [SerializeField] GameObject pauseScreen;

    public InputMaster playerControls;
    public bool isGrounded;
    public float playerSpeed;
    public float runSpeed;
    public float shieldingRunSpeed;
    public float jumpSpeed;
    public float throwTimer;
    public float throwThreshold;
    public bool shielding;
    public bool gliding;
    public bool paused;

    Rigidbody2D myRB;
    BoxCollider2D bodyCollider;

    //controls
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
        shield.beingCarried = true;
        myAnim.SetBool("draggingShield", true);
    }

    // Update is called once per frame
    void Update()
    {
        Run();

        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            isGrounded = true;
            myAnim.SetBool("grounded", true);
        } else {
            isGrounded = false;
            myAnim.SetBool("grounded", false);
        }

        if (shielding && isGrounded) {
            playerSpeed = shieldingRunSpeed;
        } else {
            playerSpeed = runSpeed;
        }

        throwTimer += Time.deltaTime;

        if (gliding && myRB.velocity.y < -5f) {
            myRB.velocity = new Vector2(myRB.velocity.x, -5f);
        }
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
            if (moveDirection.x > 0.5f && !shielding) {
                spriteObject.transform.localScale = new Vector2(1f, 1f);
            } else if (moveDirection.x < -0.5f && !shielding) {
                spriteObject.transform.localScale = new Vector2(-1f, 1f);
            }
        }

        if (Mathf.Abs(moveDirection.x) >= 0.5f) { //artificial deadzone
            Vector2 playerVelocity = new Vector2(moveDirection.x * playerSpeed, myRB.velocity.y);
            myRB.velocity = playerVelocity;
        } else {
            myRB.velocity = new Vector2(0f, myRB.velocity.y);
        }
        

    }

    void Jump(InputAction.CallbackContext context) {
        if (isGrounded) {
            myRB.velocity = new Vector2(myRB.velocity.x, jumpSpeed);

            if (myAnim.GetBool("draggingShield") ==  false) {
                myAnim.SetTrigger("jump");
            }
            
        }

        if (jump.WasReleasedThisFrame()) {
            Vector2 jumpVelocityToSub = new Vector2(0f, jumpSpeed / 3f);
            myRB.velocity -= jumpVelocityToSub;
        }
    }

    void UpAction(InputAction.CallbackContext context) {

        if (shield.beingCarried && !paused) {
            if (context.interaction is PressInteraction) {
                if (upAction.WasPressedThisFrame()) {
                    shielding = true;
                    myAnim.SetBool("blockingAbove", true);
                    HoldShield("up");
                    gliding = true;
                }

                if (upAction.WasReleasedThisFrame()) {
                    shielding = false;
                    myAnim.SetBool("blockingAbove", false);
                    DragShield();
                    gliding = false;
                }

            } 
        }
    }

    void LeftAction(InputAction.CallbackContext context) {

        if (shield.beingCarried && !paused) {
            if (leftAction.WasPressedThisFrame()) { //and has shield
                throwTimer = 0f;
                shielding = true;
                myAnim.SetBool("blocking", true);
                HoldShield("left");
            }

            if (leftAction.WasReleasedThisFrame()) {
                shielding = false;
                myAnim.SetBool("blocking", false);
                myAnim.SetBool("blockingAbove", false);
                myAnim.SetBool("draggingShield", false);
                if (throwTimer <= throwThreshold && GameObject.Find("Thrown Shield(Clone)") == null) {
                    ThrowShield("left");
                    myAnim.SetTrigger("throwShield");
                } else {
                    DragShield();
                    myAnim.SetBool("draggingShield", true);
                }
            } 
        }
    }

    void RightAction(InputAction.CallbackContext context) {

        if (shield.beingCarried && !paused) {
            if (rightAction.WasPressedThisFrame()) { //and has shield
                throwTimer = 0f;
                shielding = true;
                myAnim.SetBool("blocking", true);
                HoldShield("right");
            }

            if (rightAction.WasReleasedThisFrame()) {
                shielding = false;
                myAnim.SetBool("blocking", false);
                myAnim.SetBool("blockingAbove", false);
                myAnim.SetBool("draggingShield", false);
                if (throwTimer <= throwThreshold && GameObject.Find("Thrown Shield(Clone)") == null) {
                    ThrowShield("right");
                    myAnim.SetTrigger("throwShield");
                } else {
                    DragShield();
                    myAnim.SetBool("draggingShield", true);
                }
            } 
        }
    }

    void Pause(InputAction.CallbackContext context) {
        PauseGame();
    }

    void Recall(InputAction.CallbackContext context) {

        if (!shield.beingCarried && !paused) {

            ThrownShield thrownShield = FindObjectOfType<ThrownShield>();
            if (thrownShield != null) {
                thrownShield.RecallToPlayer();
            }
            
            DroppedShield droppedShield = FindObjectOfType<DroppedShield>();
            if (droppedShield != null) {
                droppedShield.RecallToPlayer();
            }

            myAnim.SetBool("draggingShield", true);
        }
    }

    void LeaveShield(InputAction.CallbackContext context) {

        if (shield.beingCarried && !paused) {
            shield.beingCarried = false;
            myAnim.SetBool("draggingShield", false);

            spriteObject.transform.localScale = new Vector2(-1f, 1f);
            GameObject droppedShield = Instantiate(droppedShieldPrefab,
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1.5f), Quaternion.identity); 
        }
    }

    void ThrowShield(string direction) {

        shield.beingCarried = false;
        gliding = false;

        switch (direction) {
            case "left":
                spriteObject.transform.localScale = new Vector2(-1f, 1f);
                GameObject thrownShieldL = Instantiate(thrownShieldPrefab, 
                    new Vector2(gameObject.transform.position.x - 3f, gameObject.transform.position.y - 1.5f), Quaternion.identity);
                thrownShieldL.GetComponent<Rigidbody2D>().velocity = new Vector2(-thrownShieldL.GetComponent<ThrownShield>().throwSpeed, 0f);
                break;
            case "right":
                spriteObject.transform.localScale = new Vector2(1f, 1f);
                GameObject thrownShieldR = Instantiate(thrownShieldPrefab, 
                    new Vector2(gameObject.transform.position.x + 3f, gameObject.transform.position.y - 1.5f), Quaternion.identity);
                thrownShieldR.GetComponent<Rigidbody2D>().velocity = new Vector2(thrownShieldR.GetComponent<ThrownShield>().throwSpeed, 0f);
                break;
        }
    }

    void PauseGame() {
        if (pauseScreen.activeSelf == false) {
            pauseScreen.SetActive(true);
            paused = true;
            Time.timeScale = 0f;
        } else {
            pauseScreen.SetActive(false);
            paused = false;
            Time.timeScale = 1f;
        }
    }

    void HoldShield(string direction) {

        switch (direction) {
            case "left":
                spriteObject.transform.localScale = new Vector2(-1f, 1f);
                shield.HoldLeft();
                break;
            case "right":
                spriteObject.transform.localScale = new Vector2(1f, 1f);
                shield.HoldRight();
                break;
            case "up":
                shield.HoldUp();
                break;
        }
    }

    void DragShield() {
        shield.beingCarried = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedShield : MonoBehaviour
{

    public float recallSpeed;

    bool recalling;

    Rigidbody2D myRB;
    BoxCollider2D myCollider;
    Player player;
    Shield shield;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start() {
        myRB = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        player = FindObjectOfType<Player>();
        shield = FindObjectOfType<Shield>();

    }

    // Update is called once per frame
    void Update() {
        if (recalling) {
            myRB.velocity = new Vector2(0f, 0f);
            Vector3 targetPosition = player.gameObject.transform.position;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, recallSpeed);

            if (Vector3.Distance(gameObject.transform.position, player.gameObject.transform.position) <= 0.3f) {
                shield.beingCarried = true;
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {

        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            if (other.gameObject.CompareTag("PlayerFeet")) {
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, 23f);
            }
        }
        
    }

    public void RecallToPlayer() {
        recalling = true;
        myCollider.enabled = false;
    }

}

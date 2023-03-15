using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turk : MonoBehaviour
{

    public float speed;

    Rigidbody2D myRB;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myRB.velocity = new Vector2(speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Wall") || collision.CompareTag("HeldShield")) {
            myRB.velocity = new Vector2(-myRB.velocity.x, 0f);
        }
    }
}

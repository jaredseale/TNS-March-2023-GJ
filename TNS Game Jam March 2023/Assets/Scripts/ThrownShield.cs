using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownShield : MonoBehaviour
{

    public float speed;

    Rigidbody2D myRB;
    BoxCollider2D myCollider;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Wall")) {
            myRB.constraints = RigidbodyConstraints2D.FreezeAll;
            myCollider.isTrigger = false;
        }
    }
}

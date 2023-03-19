using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turk : MonoBehaviour
{

    public float speed;

    Rigidbody2D myRB;
    GameObject myFrontTrigger;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myRB.velocity = new Vector2(speed, 0);
        myFrontTrigger = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Wall") || collision.CompareTag("HeldShield")) {
            myRB.velocity = new Vector2(-myRB.velocity.x, 0f);
            myFrontTrigger.transform.localPosition = new Vector3(myFrontTrigger.transform.localPosition.x * -1,
                myFrontTrigger.transform.localPosition.y,
                myFrontTrigger.transform.localPosition.z);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground")) {
            myRB.velocity = new Vector2(-myRB.velocity.x, 0f);
            myFrontTrigger.transform.localPosition = new Vector3(myFrontTrigger.transform.localPosition.x * -1,
                myFrontTrigger.transform.localPosition.y,
                myFrontTrigger.transform.localPosition.z);
        }
    }
}

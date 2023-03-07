using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float speed;
    public float gustPower;

    BoxCollider2D myCollider;
    Rigidbody2D myRB;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        myRB = GetComponent<Rigidbody2D>();
        myRB.velocity = new Vector2(0f, speed);

        StartCoroutine(DestroyOverTime());
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator DestroyOverTime() {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Player>() && collision.GetComponent<Player>().gliding) {
            collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x, gustPower);
            myCollider.enabled = false;
        }
    }
}

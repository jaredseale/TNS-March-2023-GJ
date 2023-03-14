using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject thingToFollow;
    [SerializeField] Vector3 followOffset;
    [SerializeField] Vector3 initOffset;
    [SerializeField] Vector3 initRotation;

    public bool beingCarried;

    BoxCollider2D shieldCollider;

    void Start()
    {
        shieldCollider = GetComponent<BoxCollider2D>();
        //followOffset = new Vector3(-2, 0, 0);
        shieldCollider.enabled = false;
    }

    void Update()
    {

        if (!beingCarried) {
            GetComponent<SpriteRenderer>().enabled = false;
        } else {
            GetComponent<SpriteRenderer>().enabled = true;
        }

        if (beingCarried && !player.shielding) {
            shieldCollider.enabled = false;
            followOffset = getFollowOffset();

            if (thingToFollow.transform.localScale.x > 0) { //flip the rotation depending on which way the player is facing
                gameObject.transform.rotation = Quaternion.Euler(initRotation);
            } else {
                gameObject.transform.rotation = Quaternion.Euler(initRotation.x, initRotation.y, -initRotation.z);
            }
            
        } else {
            shieldCollider.enabled = true;
        }

        transform.position = thingToFollow.transform.position + followOffset;
    }

    private float getFollowedDirection() {
        return thingToFollow.transform.localScale.x;
    }

    private Vector3 getFollowOffset() {
        return new Vector3(initOffset.x * getFollowedDirection(), initOffset.y, 0);
    }

    public void HoldRight() {
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 270f));
        followOffset = new Vector2(1.6f, 0f);
    }

    public void HoldLeft() {
        gameObject.transform.rotation = Quaternion.Euler(new Vector3 (0f, 0f, 90f));
        followOffset = new Vector2(-1.6f, 0f);
    }

    public void HoldUp() {
        followOffset = new Vector2(-0.05f, 0.3f);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }

    public void ResetFollowOffset() {
        followOffset = initOffset;
    }
}

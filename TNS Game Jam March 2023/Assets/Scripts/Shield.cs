using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject thingToFollow;
    [SerializeField] Vector3 followOffset;
    [SerializeField] Vector3 initOffset;

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
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
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
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
        followOffset = new Vector2(1.75f, 0f);
    }

    public void HoldLeft() {
        gameObject.transform.rotation = Quaternion.Euler(new Vector3 (0f, 0f, 90f));
        followOffset = new Vector2(-1.75f, 0f);
    }

    public void HoldUp() {
        followOffset = new Vector2(0f, 1f);
    }

    public void ResetFollowOffset() {
        followOffset = initOffset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    [SerializeField] GameObject thingToFollow;
    [SerializeField] Vector3 followOffset;
    // Start is called before the first frame update
    void Start()
    {
        followOffset = new Vector3(-2, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = thingToFollow.transform.position + getFollowOffset();
    }

    private float getFollowedDirection() {
        return thingToFollow.transform.GetChild(0).transform.localScale.x;
    }

    private Vector3 getFollowOffset() {
        return new Vector3(followOffset.x * getFollowedDirection(), 0, 0);
    }


}

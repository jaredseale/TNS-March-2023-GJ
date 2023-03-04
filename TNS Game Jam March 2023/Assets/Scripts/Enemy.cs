using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] float xSpeed;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(xSpeed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            xSpeed = xSpeed * -1;
        }
    }

}

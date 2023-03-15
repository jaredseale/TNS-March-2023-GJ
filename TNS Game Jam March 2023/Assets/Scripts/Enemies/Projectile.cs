using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update

    public float xSpeed;
    [SerializeField] float duration;
    float timeAlive;
    void Start()
    {
        timeAlive = 0;
    }

    // Update is called once per frame
    void Update()
    {
        addTime();
        transform.position = transform.position + new Vector3(xSpeed * Time.deltaTime, 0, 0);
        if (timeAlive >= duration)
        {
            Object.Destroy(gameObject);
        }
    }

    private void addTime()
    {
        timeAlive += Time.deltaTime;
    }
}

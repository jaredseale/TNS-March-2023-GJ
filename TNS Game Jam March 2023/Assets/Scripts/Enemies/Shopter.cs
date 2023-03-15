using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopter : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] float xSpeed;
    [SerializeField] GameObject projectile;
    [SerializeField] float fireCooldown;
    [SerializeField] Animator myAnim;
    float lastFire;

    void Start()
    {
        lastFire = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        fire();
        transform.position = transform.position + new Vector3(xSpeed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            xSpeed = xSpeed * -1;
            float xScale = transform.localScale.x;
            transform.localScale = new Vector3(xScale * -1, 1, 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Wall" || collision.gameObject.name.Contains("Shopter"))
        {

            xSpeed = xSpeed * -1;
            float xScale = transform.localScale.x;
            transform.localScale = new Vector3(xScale * -1, 1, 1);
        }
    }

    public void fire()
    {

        float now = Time.time;
        float timeSinceLastFire = now - lastFire;

        if (timeSinceLastFire >= fireCooldown && fireCooldown > 0)
        {
            lastFire = now;
            myAnim.SetTrigger("fire");

            GameObject spawnedProjetile = Instantiate(projectile, getProjectileSpawnPosition(transform.position), transform.rotation);
            spawnedProjetile.GetComponent<Projectile>().xSpeed *= transform.localScale.x;
        }
    }

    private Vector3 getProjectileSpawnPosition(Vector3 startingPosition)
    {
        return startingPosition + new Vector3(transform.localScale.x, -.5f, 0);
    }

}

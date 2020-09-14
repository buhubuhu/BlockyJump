using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunShotSingle : MonoBehaviour
{
    private Rigidbody2D rb;
    private float shotForce = 160;
    public GameObject hitEffect;
    public GameObject playerHitEffect;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(transform.right * shotForce);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 0)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        if (collision.collider.gameObject.layer == 11)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
           
            collision.collider.gameObject.GetComponent<blockSingle>().Cracking();
            Destroy(this.gameObject);


        }
        if (collision.collider.gameObject.layer == 12 || collision.collider.gameObject.layer == 10)
        {
            Instantiate(playerHitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
}

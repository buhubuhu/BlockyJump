using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class water : MonoBehaviour
{
   
    public float posy;
    float posx;
    public float targetpos;
    

    private void Start()
    {
        posy = transform.position.y;
        posx = transform.position.x;
    }

    private void Update()
    {

        posy = Mathf.Lerp(posy, targetpos, 0.005f * Time.deltaTime);

        if (posx >= 0.9f)
            posx = Mathf.Lerp(posx, -1, 0.1f * Time.deltaTime);
        else if(posx <= -0.9f)
            posx = Mathf.Lerp(posx, 1, 0.1f * Time.deltaTime);

        transform.position = new Vector2(posx, posy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 15)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.2f;
        }
    }


    

}

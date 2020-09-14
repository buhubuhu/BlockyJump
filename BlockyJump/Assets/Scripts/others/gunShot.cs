using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class gunShot : MonoBehaviour
{
    private Rigidbody2D rb;
    private float shotForce = 160;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(transform.right * shotForce);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.layer == 0)
        {
            PhotonNetwork.Instantiate("hiteffect", transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        if (collision.collider.gameObject.layer == 11)
        {
            PhotonNetwork.Instantiate("hiteffect", transform.position, Quaternion.identity);
            { collision.collider.gameObject.GetComponent<block>().photonView.RPC("Cracking", RpcTarget.All); }
            //collision.collider.gameObject.GetComponent<block>().Cracking();
            Destroy(this.gameObject);
            
            
        }
        if (collision.collider.gameObject.layer == 12 || collision.collider.gameObject.layer == 10)
        {
            PhotonNetwork.Instantiate("playerhiteffect", transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

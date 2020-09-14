using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class move : MonoBehaviourPunCallbacks, IPunObservable
{
    private Rigidbody2D rb;
    private float x;
    private float inputForce = 9;
    private float force;
    
    private bool left = false;
    private ObstaclePlacer op;
    private bool game;
    
    public int hits = 0;
    

    public Transform gun;
    public Transform gunPoint;
    public GameObject gunShot;
    public Transform dirPointer;
    public Transform HealthBar;

    private AudioSource au;
    public AudioClip shotsound;

    public AudioClip bulbulsound;

    private void Start()
    {

        au = GetComponent<AudioSource>();
        op = GameObject.FindGameObjectWithTag("manager").GetComponent<ObstaclePlacer>();
        rb = GetComponent<Rigidbody2D>();
        
        RandomizeFirstMove();

        if (!photonView.IsMine)
        {
            gameObject.layer = 12;
            
        }
        

    }

    private void Update()
    {
        game = op.game;
        if (game)
        {
            if (photonView.IsMine)
            {


                if (Input.GetKey(KeyCode.Mouse1))
                {
                    x += Time.deltaTime * 2;
                    if (x >= 1)
                        x = 1;
                }
                if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    force = x;
                    { photonView.RPC("Movement", RpcTarget.All, force); }
                    //Movement(force);
                    x = 0;
                }



                MouseLookAt();

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    { photonView.RPC("Shoot", RpcTarget.All); }

                }

               

            }
            DirectionPointer();

            

        }
    }

    

    [PunRPC]
    void Movement(float force)
    {

        if (!left)
        {
            rb.AddForce(transform.right * force * inputForce, ForceMode2D.Impulse);
            left = true;
            
        }
        else if (left)
        {
            rb.AddForce(-transform.right * force * inputForce, ForceMode2D.Impulse);
            left = false;
            
        }
    }

       
    [PunRPC]
    void Shoot()
    {
        if (photonView.IsMine)
        {
            rb.AddForce(-gun.transform.right * inputForce, ForceMode2D.Impulse);
            PhotonNetwork.Instantiate("gunShot", gunPoint.position, gun.rotation);
            
            
        }
        au.PlayOneShot(shotsound);
    }

   


    void MouseLookAt()
    {
        if (photonView.IsMine)
        {
            var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(gun.position);

            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            gun.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    
    void DirectionPointer()
    {
        if (left)
            dirPointer.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
        else
            dirPointer.position = new Vector2(transform.position.x - 0.5f, transform.position.y);

        if(x != 0)
        {
            dirPointer.localScale = new Vector2(Mathf.Lerp(dirPointer.localScale.x, 1.5f, 5 * Time.deltaTime), dirPointer.localScale.y);
        }
        else
        {
            dirPointer.localScale = new Vector2(0.5f, dirPointer.localScale.y);
        }
        
    }

   

    
    void RandomizeFirstMove()
    {
        int f = Random.Range(0, 2);
        if (f == 0)
            left = false;
        else if (f == 1)
            left = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            { photonView.RPC("Hit", RpcTarget.All); }
            
        }
        if(collision.gameObject.layer == 13 && hits >= 1)
        {
            Destroy(collision.gameObject);
            { photonView.RPC("Restore", RpcTarget.All); }
            
            
        }
        if(collision.gameObject.layer == 16)
        {
            op.Lowerwater();

            Destroy(collision.gameObject);
        }

        if (photonView.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "P1Health", hits } });
            else
                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "P2Health", hits } });
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 14)
        {
            if (photonView.IsMine)
            {
                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "P1Health", 6 } });
                else
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "P2Health", 6 } });

                
            }
            rb.sharedMaterial = null;
            au.PlayOneShot(bulbulsound);
        }
    }

    [PunRPC]
    void Hit()
    {
        hits = hits + 1;

        if (hits >= 0 && hits < 6)
        {
            HealthBar.position = new Vector2(HealthBar.position.x, HealthBar.position.y + 0.1f);
            HealthBar.localScale = new Vector2(HealthBar.localScale.x, HealthBar.localScale.y + 0.2f);

            
        }

        



    }

    [PunRPC]
    void Restore()
    {
        
        if (hits > 0 && hits < 6)
        {

            HealthBar.position = new Vector2(HealthBar.position.x, HealthBar.position.y - 0.1f);
            HealthBar.localScale = new Vector2(HealthBar.localScale.x, HealthBar.localScale.y - 0.2f);

            hits = hits - 1;
            if (hits < 0)
                hits = 0;
        }
        
    }
    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gun.rotation);
            stream.SendNext(left);
            stream.SendNext(x);
            stream.SendNext(hits);
        }
        else
        {
            gun.rotation = (Quaternion)stream.ReceiveNext();

            left = (bool)stream.ReceiveNext();

            x = (float)stream.ReceiveNext();

            hits = (int)stream.ReceiveNext();
        }
    }
    
    

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //op.GetComponent<ObstaclePlacer>().End();
        if (PhotonNetwork.IsMasterClient)
            op.P2health = 6;
        else
            op.P1health = 6;
    }

    
   

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveSingle : MonoBehaviour
{
    private Rigidbody2D rb;
    private float x;
    private float inputForce = 9;
    private float force;
    private float maxVelocity = 10;

    private bool left = false;
    private ObstaclePlacerSingle op;
    private bool game;

    public int hits = 0;


    public Transform gun;
    public Transform gunPoint;
    
    public Transform dirPointer;
    public Transform HealthBar;

    public GameObject gunShotSingle;
    private AudioSource au;
    public AudioClip shotsound;
    
    public AudioClip bulbulsound;

    private void Start()
    {

        au = GetComponent<AudioSource>();
        op = GameObject.FindGameObjectWithTag("manager").GetComponent<ObstaclePlacerSingle>();
        rb = GetComponent<Rigidbody2D>();

        RandomizeFirstMove();
        
    }
    private void Update()
    {
        game = op.game;
        if (game)
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
                
                Movement(force);
                x = 0;

                
            }



            MouseLookAt();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();

            }



        }
        DirectionPointer();



        
    }
    

    void Movement(float force)
    {
        if (rb.velocity.magnitude < maxVelocity)
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
    }

    void Shoot()
    {
            rb.AddForce(-gun.transform.right * inputForce, ForceMode2D.Impulse);
            Instantiate(gunShotSingle, gunPoint.position, gun.rotation);

        au.PlayOneShot(shotsound);
    }
    void MouseLookAt()
    {
            var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(gun.position);

            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            gun.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
    }

    void DirectionPointer()
    {
        if (left)
            dirPointer.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
        else
            dirPointer.position = new Vector2(transform.position.x - 0.5f, transform.position.y);

        if (x != 0)
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
        if (collision.gameObject.layer == 18)
        {
            Hit();

        }
        if (collision.gameObject.layer == 13 && hits >= 1)
        {
            Destroy(collision.gameObject);
            Restore();


        }
        if (collision.gameObject.layer == 16)
        {
            op.Lowerwater();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.layer == 17)
        {
            op.GenerateOneObstacle();
            Destroy(collision.gameObject);
        }


    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14 && game)
        {
            rb.sharedMaterial = null;
            op.End();
            au.PlayOneShot(bulbulsound);
        }
    }

    void Hit()
    {
        hits = hits + 1;

        if (hits >= 0 && hits < 6)
        {
            HealthBar.position = new Vector2(HealthBar.position.x, HealthBar.position.y + 0.1f);
            HealthBar.localScale = new Vector2(HealthBar.localScale.x, HealthBar.localScale.y + 0.2f);


        }
        if(hits >= 6 && game)
        {
            op.End();
        }
        
    }

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
}

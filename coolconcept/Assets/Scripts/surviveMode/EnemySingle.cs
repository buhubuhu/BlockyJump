using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySingle : MonoBehaviour
{
    private GameObject player;
    
    public GameObject gunShotSingle;
    public Transform gun;
    public Transform gunPoint;
    private float time;
    private float timer = 0.5f;
    private bool vision = false;
    private ObstaclePlacerSingle op;
  
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        op = GameObject.FindGameObjectWithTag("manager").GetComponent<ObstaclePlacerSingle>();
        time = timer;
        
    }

    private void Update()
    {
        if (op.game)
        {
            MouseLookAt();
            RaycastHit2D hit = Physics2D.Raycast(gunPoint.position, gunPoint.right, 10);
            if (hit.collider != null && hit.collider.gameObject.layer == 10)
            {
                vision = true;
            }
            else
            {
                vision = false;
            }


            if (vision)
            {
                time -= Time.deltaTime;
                if (time <= 0)
                {
                    Shot();
                    time = timer;
                }



            }
            else
            {
                time = timer;
            }
        }
        
    }
    void Shot()
    {
        Instantiate(gunShotSingle, gunPoint.position, gun.rotation);
        
        
    }
    

    void MouseLookAt()
    {
        var dir = player.transform.position - gun.position;

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        gun.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            op.Lowerwater();
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
        {
            Destroy(gameObject);
        }
    }
}

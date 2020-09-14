using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePlacerSingle : MonoBehaviour
{
    public int ObsToPlace = 10;
    public GameObject[] Obstacles = new GameObject[0];
    GameObject Obstacle;

    public int maxSpawnAttemptsPerObstacle = 10;
    private ContactFilter2D cf;
    public Transform[] sp1;
    

    public canvasEventSingle cE;
    public bool game = false;

    public int P1health;
    

    public waterSingle wat;

    public GameObject powerup;
    public GameObject timer;
    public GameObject blockAdd;
    public GameObject player;
    public GameObject platformblue;
    public GameObject playtformred;
    public GameObject platformgreen;
    public GameObject platformspikes;


    void Awake()
    {

        
        SpawnPlayer();
        
        GenerateMap();
        cf.useTriggers = true;
        cf.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        cf.useLayerMask = true;

        StartCoroutine(PowerUpSpawn());


        if (PlayerPrefs.HasKey("volume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("volume");
        }
        else
        {
            AudioListener.volume = 1;
        }


    }


    public void End()
    {
        game = false;
        cE.endPanel.SetActive(true);
        cE.anim.SetTrigger("end");
        cE.scoretxt.text = cE.timer.text;
        if (PlayerPrefs.HasKey("highscore"))
        {
            if(PlayerPrefs.GetFloat("highscore") < cE.time)
            {
                PlayerPrefs.SetFloat("highscore", cE.time);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("highscore", cE.time);
        }
        
    }


    public void Lowerwater()
    {
        if(wat.posy >= -2)
            wat.posy = wat.posy - 1;
    }

    IEnumerator PowerUpSpawn()
    {
        yield return new WaitForSeconds(7f);
        SpawnPowerUp();
    }

    void SpawnPowerUp()
    {
        Vector3 position = Vector3.zero;

        bool validPosition = false;

        int spawnAttempts = 0;

        while (!validPosition && spawnAttempts < maxSpawnAttemptsPerObstacle)
        {
            spawnAttempts++;

            position = new Vector3(transform.position.x + Random.Range(-23f, 24f), transform.position.y + Random.Range(-7f, 20f), 0);

            validPosition = true;
            List<Collider2D> result = new List<Collider2D>(10);

            Vector2 platformSize = new Vector2(1, 1);
            int contacts = Physics2D.OverlapBox(position, platformSize, 0, cf, result);

            if (contacts > 0)
            {
                validPosition = false;
            }

        }

        if (validPosition)
        {
            int randObj = Random.Range(0, 3);

            if (randObj == 0)
            {
                GameObject PowerUp = Instantiate(powerup, position, Quaternion.identity);
            }
            else if (randObj == 1)
            {
                GameObject PowerUp = Instantiate(timer, position, Quaternion.identity);
            }
            else if (randObj == 2)
            {
                GameObject PowerUp = Instantiate(blockAdd, position, Quaternion.identity);
            }
            StartCoroutine(PowerUpSpawn());
        }
    }

    void SpawnPlayer()
    {
        int place1 = Random.Range(0, sp1.Length);
        

        
        GameObject Player = Instantiate(player, sp1[place1].position, Quaternion.identity);

        
       
    }


    void GenerateMap()
    {
        for (int i = 0; i < ObsToPlace; i++)
        {
            int Obstacle = Random.Range(0, Obstacles.Length);

            Vector3 position = Vector3.zero;

            bool validPosition = false;

            int spawnAttempts = 0;

            while (!validPosition && spawnAttempts < maxSpawnAttemptsPerObstacle)
            {
                spawnAttempts++;

                position = new Vector3(transform.position.x + Random.Range(-23f, 24f), transform.position.y + Random.Range(-7f, 20f), 0);

                validPosition = true;
                List<Collider2D> result = new List<Collider2D>(10);

                Vector2 platformSize = new Vector2(5, 3);
                int contacts = Physics2D.OverlapBox(position, platformSize, 0, cf, result);

                if (contacts > 0)
                {
                    validPosition = false;
                }

            }

            if (validPosition)
            {
                if (Obstacle == 0)
                {
                    GameObject obs = Instantiate(platformblue, position, Quaternion.identity);
                    obs.transform.parent = transform;
                }
                if (Obstacle == 1)
                {
                    GameObject obs = Instantiate(platformgreen, position, Quaternion.identity);
                    obs.transform.parent = transform;
                }
                if (Obstacle == 2)
                {
                    GameObject obs = Instantiate(playtformred, position, Quaternion.identity);
                    obs.transform.parent = transform;
                }
                if (Obstacle == 3)
                {
                    GameObject obs = Instantiate(platformspikes, position, Quaternion.identity);
                    obs.transform.parent = transform;
                }

            }
        }
    }

    public void GenerateOneObstacle()
    {
        int Obstacle = Random.Range(0, Obstacles.Length);

        Vector3 position = Vector3.zero;

        bool validPosition = false;

        int spawnAttempts = 0;

        while (!validPosition && spawnAttempts < maxSpawnAttemptsPerObstacle)
        {
            spawnAttempts++;

            position = new Vector3(transform.position.x + Random.Range(-23f, 24f), transform.position.y + Random.Range(-7f, 20f), 0);

            validPosition = true;
            List<Collider2D> result = new List<Collider2D>(10);

            Vector2 platformSize = new Vector2(5, 3);
            int contacts = Physics2D.OverlapBox(position, platformSize, 0, cf, result);

            if (contacts > 0)
            {
                validPosition = false;
            }

        }

        if (validPosition)
        {
            if (Obstacle == 0)
            {
                GameObject obs = Instantiate(platformblue, position, Quaternion.identity);
                obs.transform.parent = transform;
            }
            if (Obstacle == 1)
            {
                GameObject obs = Instantiate(platformgreen, position, Quaternion.identity);
                obs.transform.parent = transform;
            }
            if (Obstacle == 2)
            {
                GameObject obs = Instantiate(playtformred, position, Quaternion.identity);
                obs.transform.parent = transform;
            }
            if (Obstacle == 3)
            {
                GameObject obs = Instantiate(platformspikes, position, Quaternion.identity);
                obs.transform.parent = transform;
            }

        }
    }
}

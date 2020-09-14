using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ObstaclePlacer : MonoBehaviourPunCallbacks
{
    public int ObsToPlace = 10;
    public GameObject[] Obstacles = new GameObject[0];
    GameObject Obstacle;
    
    public int maxSpawnAttemptsPerObstacle = 10;
    private ContactFilter2D cf;
    public Transform[] sp1;
    public Transform[] sp2;

    public canvasEvents cE;
    public bool game = false;

    public int P1health;
    public int P2health;

    public water wat;

    void Awake()
    {

        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
            
        }
        if (PhotonNetwork.IsMasterClient)
        {
            GenerateMap();
            cf.useTriggers = true;
            cf.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            cf.useLayerMask = true;

            StartCoroutine(PowerUpSpawn());
        }

        if (PlayerPrefs.HasKey("volume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("volume");
        }
        else
        {
            AudioListener.volume = 1;
        }


    }

    private void Update()
    {
        if ((P1health >= 6 || P2health >= 6) && game)
        {
            End();
            game = false;
        }

       
    }
    public void End()
    {
        
        cE.endPanel.SetActive(true);
        cE.anim.SetTrigger("end");
        if(P1health >= 6)
        {
            cE.loosetxt.SetActive(true);
            cE.wintxt.SetActive(false);
        }
        else if(P2health >= 6)
        {
            cE.loosetxt.SetActive(false);
            cE.wintxt.SetActive(true);

        }
    }

    
    public void Lowerwater()
    {
        wat.posy = wat.posy - 1;
    }

    IEnumerator PowerUpSpawn()
    {
        yield return new WaitForSeconds(10f);
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

            position = new Vector3(transform.position.x + Random.Range(-11.5f, 11.5f), transform.position.y + Random.Range(-7f, 11f), 0);

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
            int randObj = Random.Range(0, 2);

            if (randObj == 0)
            {
                GameObject PowerUp = PhotonNetwork.Instantiate("PowerUp", position, Quaternion.identity);
            }
            else if (randObj == 1)
            {
                GameObject PowerUp = PhotonNetwork.Instantiate("timer", position, Quaternion.identity);
            }
            StartCoroutine(PowerUpSpawn());
        }
    }

    void SpawnPlayer()
    {
        int place1 = Random.Range(0, sp1.Length);
        int place2 = Random.Range(0, sp2.Length);

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject Player = PhotonNetwork.Instantiate("Player1", sp1[place1].position, Quaternion.identity);
            
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            GameObject Player = PhotonNetwork.Instantiate("Player1", sp2[place2].position, Quaternion.identity);
            
        }
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

                position = new Vector3(transform.position.x + Random.Range(-11.5f, 11.5f), transform.position.y + Random.Range(-7f, 11f), 0);
                
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
                if(Obstacle == 0)
                {
                    GameObject obs = PhotonNetwork.Instantiate("platformBlue", position, Quaternion.identity);
                    obs.transform.parent = transform;
                }
                if (Obstacle == 1)
                {
                    GameObject obs = PhotonNetwork.Instantiate("platformGreen", position, Quaternion.identity);
                    obs.transform.parent = transform;
                }
                if (Obstacle == 2)
                {
                    GameObject obs = PhotonNetwork.Instantiate("platformRed", position, Quaternion.identity);
                    obs.transform.parent = transform;
                }

            }
        }
    }

    void SetHealth()
    {
        
        P1health = PhotonNetwork.CurrentRoom.CustomProperties["P1Health"].GetHashCode();
        P2health = PhotonNetwork.CurrentRoom.CustomProperties["P2Health"].GetHashCode();
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        SetHealth();
    }

    
}

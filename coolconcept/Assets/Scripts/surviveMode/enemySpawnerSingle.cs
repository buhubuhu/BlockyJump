using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawnerSingle : MonoBehaviour
{
    public GameObject enemy;
    int maxSpawnAttemptsPerObstacle = 5;
    private ContactFilter2D cf;



    private void Start()
    {
        cf.useTriggers = true;
        cf.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        cf.useLayerMask = true;
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        int randTime = Random.Range(5, 10);
        yield return new WaitForSeconds(randTime);
        Spawner();
    }

    void Spawner()
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
            

            
            GameObject PowerUp = Instantiate(enemy, position, Quaternion.identity);
            
            StartCoroutine(EnemySpawn());
        }
    }
}

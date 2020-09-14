using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerafollowsingle : MonoBehaviour
{
    private GameObject player;
    float x;
    float y;
    float smoothing = 10f;
    

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        x = transform.position.x;
        y = transform.position.y;
    }


    private void FixedUpdate()
    {


        //x = Mathf.Lerp(x, player.transform.position.x, 5 * Time.deltaTime);
        //y = Mathf.Lerp(y, player.transform.position.y, 5 * Time.deltaTime);

        Vector3 desiredPosition = new Vector3(player.transform.position.x, player.transform.position.y, -200);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothing * Time.deltaTime);
        transform.position = smoothedPosition;

        
        //transform.position = new Vector3(x, y, transform.position.z);
    }
}

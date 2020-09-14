using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockSingle : MonoBehaviour
{
    public GameObject crack;
    float hit = 0;

    
    public void Cracking()
    {

        if (hit == 1)
            crack.SetActive(true);
        else if (hit == 2)
        {
            gameObject.SetActive(false);
        }
        hit = hit + 0.5f;
    }
}

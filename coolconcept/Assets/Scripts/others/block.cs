using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class block : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject crack;
    float hit = 0;

    [PunRPC]
    public void Cracking()
    {
        
        if (hit == 1)
            crack.SetActive(true);
        else if(hit == 2)
        {
            gameObject.SetActive(false);
        }
        hit = hit + 0.5f;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hit);
            

        }
        else
        {
            hit = (float)stream.ReceiveNext();

            
        }
    }
}

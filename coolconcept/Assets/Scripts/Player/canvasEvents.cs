using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class canvasEvents : MonoBehaviourPunCallbacks
{
    public GameObject startPanel;
    public GameObject endPanel;
    public GameObject wintxt;
    public GameObject loosetxt;
    public GameObject player1;
    public GameObject player2;
    public ObstaclePlacer op;
    public Animator anim;
    private void Start()
    {
        
        startPanel.SetActive(true);
        endPanel.SetActive(false);
        wintxt.SetActive(false);
        loosetxt.SetActive(false);
        anim = GetComponent<Animator>();

        if (PhotonNetwork.IsMasterClient)
        {
            player1.SetActive(true);
            player2.SetActive(false);
        }
        else
        {
            player1.SetActive(false);
            player2.SetActive(true);
        }
    }


    public void StartGame()
    {
        op.game = true;
        startPanel.SetActive(false);
    }

    public void backToLobby()
    {

        StartCoroutine(LeaveRoom());


    }
    IEnumerator LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;
        StartCoroutine(Disconnect());


    }
    IEnumerator Disconnect()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        PhotonNetwork.LoadLevel(2);

    }
    



    /*public void Win()
    {

        if (op.P2health >= 6)
            wintxt.SetActive(true);
        else if (op.P1health >= 6)
            loosetxt.SetActive(true);

    }*/





}

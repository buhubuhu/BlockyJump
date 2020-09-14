using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    
    public GameObject playbtn;
    
    public GameObject searchIMG;
    
    public GameObject connectIMG;
    public GameObject survivebtn;
    public TextMeshProUGUI playercount;
    public Slider volslide;
    public TextMeshProUGUI highscore;
    private ConnectionProtocol TransportProtocol;

    private void Start()
    {
        playbtn.SetActive(false);
        searchIMG.SetActive(false);
        connectIMG.SetActive(false);
        playbtn.SetActive(true);
        survivebtn.SetActive(true);

        if (PlayerPrefs.HasKey("volume"))
        {
            volslide.value = PlayerPrefs.GetFloat("volume");
        }
        else
        {
            volslide.value = 1;
        }

        if (PlayerPrefs.HasKey("highscore"))
        {
            highscore.text = PlayerPrefs.GetFloat("highscore").ToString("F2");
        }
        else
        {
            highscore.text = "0";
            
        }
    }

    private void Update()
    {
        AudioListener.volume = volslide.value;
    }
    public void ConnectedPlay()
    {
        connectIMG.SetActive(true);
        playbtn.SetActive(false);

        PlayerPrefs.SetFloat("volume", volslide.value);

        this.TransportProtocol = ConnectionProtocol.WebSocketSecure;
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
            
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        connectIMG.SetActive(false);
        survivebtn.SetActive(false);
        FindMatch();

    }

    public void FindMatch()
    {
        searchIMG.SetActive(true);
        
        PhotonNetwork.JoinRandomRoom();
        
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("creating room");
        MakeRoom();
    }

    void MakeRoom()
    {
        int randomRoomName = Random.Range(0, 5000);
        RoomOptions roomOptions =
            new RoomOptions()
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = 2
            };

        Hashtable RoomCustomProps = new Hashtable();
        RoomCustomProps.Add("P1Health", 0);
        RoomCustomProps.Add("P2Health", 0);
        roomOptions.CustomRoomProperties = RoomCustomProps;

        PhotonNetwork.CreateRoom("RoomName_" + randomRoomName, roomOptions);
        Debug.Log("room created - waiting for player");
    }

    public void StopSearch()
    {
        searchIMG.SetActive(false);
        playbtn.SetActive(true);
        survivebtn.SetActive(true);
        PhotonNetwork.Disconnect();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2)// && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        
        playercount.text = PhotonNetwork.CountOfPlayers.ToString();
        
    }



    public void Quit()
    {
        Application.Quit();
    }


    public void SurviveMode()
    {
        PlayerPrefs.SetFloat("volume", volslide.value);
        SceneManager.LoadScene(3);

    }

    public void Kenney()
    {
        Application.OpenURL("https://www.kenney.nl/assets");
    }
    public void Music()
    {
        Application.OpenURL("https://incompetech.filmmusic.io/song/3707-electrodoodle");
    }
}

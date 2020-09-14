using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class canvasEventSingle : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject endPanel;
    public GameObject wintxt;
    public GameObject loosetxt;
    public ObstaclePlacerSingle op;
    public Animator anim;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI scoretxt;
    public float time = 0;
    
    private void Start()
    {

        startPanel.SetActive(true);
        endPanel.SetActive(false);
        wintxt.SetActive(false);
        loosetxt.SetActive(false);
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (op.game)
        {
            time += Time.deltaTime;

            timer.text = time.ToString("F2");
        }
        
    }


    public void StartGame()
    {
        op.game = true;
        startPanel.SetActive(false);
    }


    public void backToLobby()
    {


        SceneManager.LoadScene(2);

    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(3);
    }
}

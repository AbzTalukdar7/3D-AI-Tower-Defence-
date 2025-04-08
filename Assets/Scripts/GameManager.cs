using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerStats playerStats;

    public AgentController aiController;

    public Waypoints waypoints;

    private bool isEnded = false;

    public GameObject gameOverUI;

    void Start() {
        isEnded = false;
    }
    
    void Update()
    {
        if (isEnded)
            return;
            
        if(playerStats.Lives <= 0){
            
            aiController.OnAllLivesLost();
        }
    }

    void EndGame(){
        isEnded = true;

        gameOverUI.SetActive(true);
    }
}

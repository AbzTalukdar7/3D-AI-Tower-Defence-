using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{   
    public PlayerStats playerStats;

    public Text livesText ;
    void Update()
    {
        livesText.text = playerStats.Lives.ToString() ;
    }
}

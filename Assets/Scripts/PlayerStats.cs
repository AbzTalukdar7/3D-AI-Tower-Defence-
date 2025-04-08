using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int Money;
    public int startMoney = 100;

    public int Lives;
    public int startLives = 100;

    public int Score;
    public int kills;

    void Start()
    {
        Money = startMoney;
        Lives = startLives;

        Score = 0;
        kills = 0;
    }

    public void Reset(){
        Money = startMoney;
        Lives = startLives;

        Score = 0;
        kills = 0;
    }
}

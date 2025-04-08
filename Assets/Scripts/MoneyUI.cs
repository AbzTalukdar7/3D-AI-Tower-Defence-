using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public Text moneyText;

    public PlayerStats playerStats;

    void Update()
    {
        moneyText.text = "$" + playerStats.Money.ToString();
    }
}

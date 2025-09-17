using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    private int coinAmount = 0;

    public void Start()
    {
        UpdateUI();
    }

    public void IncrementCoins()
    {
        coinAmount++;
        UpdateUI();
    }
    private void UpdateUI()
    {
        coinText.SetText($"Coins: {coinAmount}");
    }
}

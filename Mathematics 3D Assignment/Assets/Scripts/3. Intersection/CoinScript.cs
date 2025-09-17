using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private CustomInstersectionScript intersection;

    void Start()
    {
        intersection = GetComponent<CustomInstersectionScript>();
        intersection.OnIntersectStay += CollectCoin;
    }

    private void CollectCoin(CustomInstersectionScript collider)
    {
        CoinUI coinUi = FindObjectOfType<CoinUI>();
        if (coinUi != null)
        {
            coinUi.IncrementCoins();
        }
        Destroy(this.gameObject);
    }

}

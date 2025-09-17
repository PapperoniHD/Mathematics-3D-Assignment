using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Range(0f, 2f)]
    public float timeScale = 1;
    private float currentTimeScale = 1;

    // Update is called once per frame
    void Update()
    {
        if (timeScale != currentTimeScale)
        {
            Time.timeScale = timeScale;
            currentTimeScale = timeScale;
        }
    }
}

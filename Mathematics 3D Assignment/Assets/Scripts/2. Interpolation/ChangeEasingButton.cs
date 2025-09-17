using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEasingButton : MonoBehaviour
{
    [SerializeField] private MovingPlatform platform;
    private CustomInstersectionScript intersection;

    private void Start()
    {
        intersection = GetComponent<CustomInstersectionScript>();
        if (intersection != null)
        {
            intersection.OnIntersectEnter += OnIntersectEnter;
        }
    }

    private void OnIntersectEnter(CustomInstersectionScript obj)
    {
        platform.SetEasingMethod(platform.GetNextEasingMethod());
    }

}

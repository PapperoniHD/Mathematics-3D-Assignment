using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSphereSimulationButton : MonoBehaviour
{
    [SerializeField] private CustomRigidbody[] rigidbodies;
    private CustomInstersectionScript intersection;

    private bool hasStarted = false;
    void Start()
    {
        intersection = GetComponent<CustomInstersectionScript>();
        if (intersection != null)
        {
            intersection.OnIntersectEnter += OnIntersectEnter;
        }
    }

    private void OnIntersectEnter(CustomInstersectionScript obj)
    {
        if (!hasStarted)
        {
            foreach (var body in rigidbodies)
            {
                body.SetKinematic(false);
            }
            hasStarted = true;
        }
    }
}

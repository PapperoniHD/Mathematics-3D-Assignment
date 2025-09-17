using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSphereButton : MonoBehaviour
{
    private CustomInstersectionScript intersection;

    private float spawnDelay = 1f;
    private float timer = 1f;

    [SerializeField] private CustomRigidbody[] bodies;
    private int bodyIndex = 0;

    private bool hasActivated = false;

    void Start()
    {
        intersection = GetComponent<CustomInstersectionScript>();
        if (intersection != null)
        {
            intersection.OnIntersectStay += OnIntersectStay;
        }
    }

    private void OnIntersectStay(CustomInstersectionScript obj)
    {
        if (hasActivated) return;
        foreach (var body in bodies)
        {
            body.SetKinematic(false);
        }
        hasActivated = true;

    }

    void StartSphere()
    {
        if (bodyIndex > bodies.Length)
        {
            return;
        }

        bodies[bodyIndex].SetKinematic(false);
        bodyIndex++;         
    }
}

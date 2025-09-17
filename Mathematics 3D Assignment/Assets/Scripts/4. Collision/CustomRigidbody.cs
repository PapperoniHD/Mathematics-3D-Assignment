using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GabrielAssignment;

[RequireComponent(typeof(CustomInstersectionScript))]
public class CustomRigidbody : MonoBehaviour
{
    private CustomInstersectionScript intersectionScript;
    private PhysicsObject physicsObject;

    public Vector3 startingVelocity = new Vector3(0, 0, 0);
    public float mass = 1;
    public bool useGravity = true;
    public bool isKinematic = false;
    [Range(0f,1f)]
    public float bounciness = 0.5f;

    private PhysicsWorld instanceRef;

    private void Awake()
    {
        intersectionScript = GetComponent<CustomInstersectionScript>();
        intersectionScript.isTrigger = false;
        instanceRef = PhysicsWorld.GetInstance();
    }

    private void OnEnable()
    {
        if (intersectionScript.intersection is SphereIntersection)
        {
            SphereIntersection sphere = (SphereIntersection)intersectionScript.intersection;
            GabrielAssignment.SphereCollider collider = new();

            collider.center = sphere.center;
            collider.radius = sphere.radius;

            physicsObject = new PhysicsObject(transform.position, startingVelocity, Vector3.zero, mass, useGravity, isKinematic, bounciness, collider, this.transform);
        }
        else if (intersectionScript.intersection is BoxIntersection)
        {
            BoxIntersection bounds = (BoxIntersection)intersectionScript.intersection;
            GabrielAssignment.BoxCollider collider = new();

            collider.center = bounds.center;
            collider.bounds = bounds.bounds;

            physicsObject = new PhysicsObject(transform.position, startingVelocity, Vector3.zero, mass, useGravity, isKinematic, bounciness, collider, this.transform);
        }

        if (instanceRef != null)
        {
            instanceRef.AddObject(physicsObject);
        }
        
    }

    private void OnDisable()
    {
        if (instanceRef != null)
        {
            instanceRef.RemoveObject(physicsObject);
        }
    }

    private void Update()
    {
        transform.position = physicsObject.position;
    }

    public void SetKinematic(bool isKinematic)
    {
        physicsObject.isKinematic = isKinematic;
    }

    public void SetBounciness(float bounciness)
    {
        physicsObject.bounciness = bounciness;
    }
}



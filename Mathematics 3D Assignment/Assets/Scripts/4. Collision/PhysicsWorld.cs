using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GabrielAssignment;

public class PhysicsWorld : MonoBehaviour
{
    public static PhysicsWorld instance;

    [SerializeField] private List<PhysicsObject> physicsObjects = new();
    [SerializeField] private List<Solver> solvers = new();

    public static Vector3 gravity = new Vector3(0, -9.81f, 0);

    private void Awake()
    {
        CollisionSolver solver = new();
        solvers.Add(solver);
    }
    public void AddObject(PhysicsObject obj)
    {
        physicsObjects.Add(obj);
    }

    public void RemoveObject(PhysicsObject obj)
    {
        physicsObjects.Remove(obj);
    }

    public void AddSolver(Solver solver)
    {
        solvers.Add(solver);
    }

    public void RemoveSolver(Solver solver)
    {
        solvers.Remove(solver);
    }

    private void FixedUpdate()
    {
        Step(Time.fixedDeltaTime);
    }

    void Step(float dt)
    {
        ResolveCollisions(dt);

        for (int i = 0; i < physicsObjects.Count; i++)
        {
            var obj = physicsObjects[i];
            if (!obj.useGravity) continue;
            if (obj.isKinematic) continue;

            obj.force += obj.mass * gravity;

            obj.velocity += obj.force / obj.mass * dt;
            obj.position += obj.velocity * dt;

            obj.force = Vector3.zero;
        }
    }

    void ResolveCollisions(float dt)
    {
        List<GabrielAssignment.Collision> collisions = new();

        foreach (PhysicsObject a in physicsObjects)
        {
            foreach (PhysicsObject b in physicsObjects)
            {
                if (a == b) continue;

                if (a.collider == null || b.collider == null) continue;

                CollisionPoints points = a.collider.TestCollision(a.transform, b.collider, b.transform);

                if (points.hasCollision)
                {
                    GabrielAssignment.Collision collision = new();
                    collision.objA = a;
                    collision.objB = b;
                    collision.points = points;

                    Debug.Log("Has Collision");
                    collisions.Add(collision);
                }
            }
        }

        foreach (Solver solver in solvers)
        {
            solver.Solve(collisions, dt);
        }
    }

    public static PhysicsWorld GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<PhysicsWorld>();
        }
        return instance;
    }
}

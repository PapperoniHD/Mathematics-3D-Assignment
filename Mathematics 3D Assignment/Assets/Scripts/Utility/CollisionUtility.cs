using System.Collections.Generic;
using UnityEngine;

namespace GabrielAssignment
{
    public struct CollisionPoints
    {
        public Vector3 a;
        public Vector3 b;
        public Vector3 normal;
        public float depth;
        public bool hasCollision;
    }
    public static class CollisionUtility
    {
        // Intersection
        public static bool SphereSphereIntersection(Vector3 centerA, float radiusA, Vector3 centerB, float radiusB)
        {
            Vector3 diff = centerA - centerB;
            float radiusSum = radiusA + radiusB;

            return Vector3.Dot(diff, diff) <= radiusSum * radiusSum;
        }

        public static bool AABBIntersection(Vector3 minA, Vector3 maxA, Vector3 minB, Vector3 maxB)
        {
            if (minA.x > maxB.x || minB.x > maxA.x) return false;
            if (minA.y > maxB.y || minB.y > maxA.y) return false;
            if (minA.z > maxB.z || minB.z > maxA.z) return false;

            return true;
        }

        // Source: https://gdbooks.gitbooks.io/3dcollisions/content/Chapter2/static_sphere_aabb.html
        public static bool SphereAABBIntersection(Vector3 centerA, float radiusA, Vector3 minB, Vector3 maxB)
        {
            Vector3 closestPoint = new Vector3(
                Mathf.Clamp(centerA.x, minB.x, maxB.x), 
                Mathf.Clamp(centerA.y, minB.y, maxB.y), 
                Mathf.Clamp(centerA.z, minB.z, maxB.z) 
            );

            Vector3 differenceVec = centerA - closestPoint;
            float distanceSqr = Vector3.SqrMagnitude(differenceVec);

            float radiusSqr = radiusA * radiusA;

            return distanceSqr < radiusSqr;
        }


        // Source: https://www.youtube.com/watch?v=-_IspRG548E
        public static CollisionPoints FindSphereCollisionPoint(SphereCollider a, Transform ta, SphereCollider b, Transform tb)
        {
            CollisionPoints point = new();

            Vector3 centerA = ta.position + a.center;
            Vector3 centerB = tb.position + b.center;

            float radiusA = (a.radius * 0.5f) * ((ta.lossyScale.x + ta.lossyScale.y + ta.lossyScale.z) / 3);
            float radiusB = (b.radius * 0.5f) * ((tb.lossyScale.x + tb.lossyScale.y + tb.lossyScale.z) / 3);

            if (!SphereSphereIntersection(centerA, radiusA, centerB, radiusB))
            {
                point.hasCollision = false;
                return point;
            }


            Vector3 dir = centerA - centerB;
            float dist = dir.magnitude;

            Vector3 normal = dir / dist;

            float depth = radiusA + radiusB - dist;

            point.a = centerA - normal * radiusA;
            point.b = centerB + normal * radiusB;
            point.normal = normal;
            point.depth = depth;
            point.hasCollision = true;

            return point;
        }

        public static CollisionPoints FindSphereAABBCollisionPoint(SphereCollider a, Transform ta, BoxCollider b, Transform tb)
        {
            CollisionPoints point = new();

            Vector3 centerA = ta.position + a.center;
            float radiusA = (a.radius * 0.5f) * ((ta.lossyScale.x + ta.lossyScale.y + ta.lossyScale.z) / 3);

            Vector3 boundsB = b.bounds * 0.5f;
            boundsB.Scale(tb.lossyScale);

            Vector3 minB = tb.position + b.center - boundsB;
            Vector3 maxB = tb.position + b.center + boundsB;
            if (!SphereAABBIntersection(centerA, radiusA, minB, maxB))
            {
                point.hasCollision = false;
                return point;
            }

            Vector3 closestPoint = new Vector3(
                Mathf.Clamp(centerA.x, minB.x, maxB.x),
                Mathf.Clamp(centerA.y, minB.y, maxB.y),
                Mathf.Clamp(centerA.z, minB.z, maxB.z)
            );

            Vector3 dir = centerA - closestPoint;
            float dist = dir.magnitude;

            Vector3 normal = dir / dist; 
            float depth = radiusA - dist;

            point.a = centerA - normal * radiusA;
            point.b = closestPoint;
            point.normal = normal;
            point.depth = depth;
            point.hasCollision = true;

            return point;
        }

        public static CollisionPoints FindAABBSphereCollisionPoint(BoxCollider b, Transform tb, SphereCollider a, Transform ta)
        {
            var result = FindSphereAABBCollisionPoint(a, ta, b, tb);

            // Swapping normal and a, b points for collision consistency, this was making me crazy
            result.normal = -result.normal;

            Vector3 temp = result.a;
            result.a = result.b;
            result.b = temp;

            return result;
        }

        public static CollisionPoints FindAABBCollisionPoint(BoxCollider a, Transform ta, BoxCollider b, Transform tb)
        {
            CollisionPoints point = new();

            var boundsA = new Vector3(
                a.bounds.x * 0.5f * ta.lossyScale.x,
                a.bounds.y * 0.5f * ta.lossyScale.y,
                a.bounds.z * 0.5f * ta.lossyScale.z
            );

            var boundsB = new Vector3(
                b.bounds.x * 0.5f * tb.lossyScale.x,
                b.bounds.y * 0.5f * tb.lossyScale.y,
                b.bounds.z * 0.5f * tb.lossyScale.z
            );

            var minA = ta.position + a.center - boundsA;
            var maxA = ta.position + a.center + boundsA;

            var minB = tb.position + b.center - boundsB;
            var maxB = tb.position + b.center + boundsB;

            if (!CollisionUtility.AABBIntersection(minA, maxA, minB, maxB))
            {
                point.hasCollision = false;
                return point;
            }


            float overlapX = Mathf.Min(maxA.x, maxB.x) - Mathf.Max(minA.x, minB.x);
            float overlapY = Mathf.Min(maxA.y, maxB.y) - Mathf.Max(minA.y, minB.y);
            float overlapZ = Mathf.Min(maxA.z, maxB.z) - Mathf.Max(minA.z, minB.z);

            float minOverlap = Mathf.Min(overlapX, Mathf.Min(overlapY, overlapZ));


            Vector3 normal = Vector3.zero;

            if (minOverlap == overlapX)
            {
                normal = (ta.position.x < tb.position.x) ? Vector3.left : Vector3.right;
            }
            else if (minOverlap == overlapY)
            {
                normal = (ta.position.y < tb.position.y) ? Vector3.down : Vector3.up;
            }
            else 
            {
                normal = (ta.position.z < tb.position.z) ? Vector3.back : Vector3.forward;
            }
            

            point.a = ta.position + a.center;
            point.b = tb.position + b.center;
            point.normal = normal;
            point.depth = minOverlap;
            point.hasCollision = true;

            return point;
        }

    }

    public class PhysicsObject
    {
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 force;
        public float mass;
        public bool useGravity;
        public bool isKinematic;
        public float bounciness;

        public CustomCollider collider;
        public Transform transform;

        public PhysicsObject(Vector3 position, Vector3 velocity, Vector3 force, float mass, bool useGravity, bool isKinematic, float bounciness, CustomCollider collider, Transform transform)
        {
            this.position = position;
            this.velocity = velocity;
            this.force = force;
            this.mass = mass;
            this.useGravity = useGravity;
            this.isKinematic = isKinematic;
            this.bounciness = bounciness;

            this.collider = collider;
            this.transform = transform;
        }
    }

    public abstract class CustomCollider 
    {
        public abstract CollisionPoints TestCollision(Transform transform, CustomCollider collider, Transform colliderTransform);
        public abstract CollisionPoints TestCollision(Transform transform, SphereCollider collider, Transform sphereTransform);
        public abstract CollisionPoints TestCollision(Transform transform, BoxCollider collider, Transform boxTransform);

    }
    public class SphereCollider : CustomCollider
    {
        public Vector3 center;
        public float radius;

        public override CollisionPoints TestCollision(Transform transform, CustomCollider collider, Transform colliderTransform)
        {

            return collider.TestCollision(colliderTransform, this, transform);
        }

        public override CollisionPoints TestCollision(Transform transform, SphereCollider collider, Transform sphereTransform)
        {
            return CollisionUtility.FindSphereCollisionPoint(this, transform, collider, sphereTransform);
        }

        public override CollisionPoints TestCollision(Transform transform, BoxCollider collider, Transform boxTransform)
        {
            return CollisionUtility.FindSphereAABBCollisionPoint(this, transform, collider, boxTransform);
        }
    }

    public class BoxCollider : CustomCollider
    {
        public Vector3 center;
        public Vector3 bounds;

        public override CollisionPoints TestCollision(Transform transform, CustomCollider collider, Transform colliderTransform)
        {
            return collider.TestCollision(colliderTransform, this, transform);
        }

        public override CollisionPoints TestCollision(Transform transform, SphereCollider collider, Transform sphereTransform)
        {
            return CollisionUtility.FindAABBSphereCollisionPoint(this, transform, collider, sphereTransform);
        }

        public override CollisionPoints TestCollision(Transform transform, BoxCollider collider, Transform boxTransform)
        {
            return CollisionUtility.FindAABBCollisionPoint(this, transform, collider, boxTransform);
        }
    }

    public struct Collision
    {
        public PhysicsObject objA;
        public PhysicsObject objB;
        public CollisionPoints points;
    }

    public abstract class Solver
    {
        public abstract void Solve(List<GabrielAssignment.Collision> collisions, float dt);
    }

    public class CollisionSolver : Solver
    {
        public override void Solve(List<GabrielAssignment.Collision> collisions, float dt)
        {        
            foreach (var collision in collisions)
            {
                bool aKinematic = collision.objA.isKinematic;
                bool bKinematic = collision.objB.isKinematic;

                Vector3 normal = collision.points.normal;
                float depth = collision.points.depth;
                Debug.Log(collision.points.depth);

                if (!aKinematic)
                {
                    float bouncyness = collision.objA.bounciness;
                    Debug.Log("A BOUNCYNIESS: " + bouncyness);

                    float normalVelocity = Vector3.Dot(collision.objA.velocity, normal);
                     if (normalVelocity < 0)
                     {
                         collision.objA.velocity -= (1f + bouncyness) * normal * normalVelocity;
                     }
                    //collision.objA.velocity = Vector3.zero;
                    collision.objA.position -= normal * depth * 0.5f;
                }

                if (!bKinematic)
                {
                    float bouncyness = collision.objB.bounciness;

                    float normalVelocity = Vector3.Dot(collision.objB.velocity, normal);
                    if (normalVelocity < 0)
                    {
                        collision.objB.velocity -= (1f + bouncyness) * normal * normalVelocity;
                    }
                    //collision.objB.velocity = Vector3.zero;
                    //ollision.objB.position += normal * depth * 0.5f;
                    collision.objB.position += normal * depth * 0.5f;

                    Debug.Log("B BOUNCYNIESS: " + bouncyness);
                }
            }

        }
    }

}


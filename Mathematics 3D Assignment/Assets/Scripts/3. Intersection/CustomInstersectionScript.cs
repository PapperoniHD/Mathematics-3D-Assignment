using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GabrielAssignment;
using System;

public class CustomInstersectionScript : MonoBehaviour
{
    public IntersectionType type;
    [SerializeReference] public Intersection intersection;

    public bool isTrigger = true;

    private IntersectionManager intersectionInstance;
    public event Action<CustomInstersectionScript> OnIntersectEnter;
    public event Action<CustomInstersectionScript> OnIntersectStay;
    public event Action<CustomInstersectionScript> OnIntersectExit;

    private bool isIntersected;
    public void OnEnable() 
    {
        intersectionInstance = IntersectionManager.GetInstance();
        if (intersection != null)
        {
            intersectionInstance.allColliders.Add(this);
        }
    } 

    public void OnDisable()
    {
        intersectionInstance = IntersectionManager.GetInstance();
        if (intersectionInstance != null)
        {
            intersectionInstance.allColliders.Remove(this);
        }
        
    }

    private void Update()
    {
        if (isTrigger)
        {
            CheckAllIntersections();
        }
    }

    public void CheckAllIntersections()
    {
        if (intersectionInstance == null) return;
        bool intersected = false;

       
        foreach (var collider in new List<CustomInstersectionScript>(IntersectionManager.GetInstance().allColliders))
        {
            if (collider == null) continue;
            if (collider == this) continue;
            if (!collider.isTrigger) continue;

            // Sphere-Sphere intersection
            if (intersection is SphereIntersection && collider.intersection is SphereIntersection)
            {
                SphereIntersection intersectionA = (SphereIntersection)intersection;
                SphereIntersection intersectionB = (SphereIntersection)collider.intersection;

                var centerA = transform.position + intersectionA.center;
                var centerB = collider.transform.position + intersectionB.center;

                var radiusA = (intersectionA.radius * 0.5f) * ((transform.lossyScale.x + transform.lossyScale.y + transform.lossyScale.z) / 3);
                var radiusB = (intersectionB.radius * 0.5f) * ((collider.transform.lossyScale.x + collider.transform.lossyScale.y + collider.transform.lossyScale.z) / 3);

                if (CollisionUtility.SphereSphereIntersection(centerA, radiusA, centerB, radiusB))
                {
                    CallIntersectActions(collider, true);
                    intersected = true;
                }
                
            }
            // AABB intersection
            else if(intersection is BoxIntersection && collider.intersection is BoxIntersection)
            {
                BoxIntersection intersectionA = (BoxIntersection)intersection;
                BoxIntersection intersectionB = (BoxIntersection)collider.intersection;

                var centerA = transform.position + intersectionA.center;
                var centerB = collider.transform.position + intersectionB.center;

                var boundsA = new Vector3(
                    intersectionA.bounds.x * 0.5f * transform.lossyScale.x, 
                    intersectionA.bounds.y * 0.5f * transform.lossyScale.y, 
                    intersectionA.bounds.z * 0.5f * transform.lossyScale.z
                );

                var boundsB = new Vector3(
                    intersectionB.bounds.x * 0.5f * collider.transform.lossyScale.x, 
                    intersectionB.bounds.y * 0.5f * collider.transform.lossyScale.y, 
                    intersectionB.bounds.z * 0.5f * collider.transform.lossyScale.z
               );

                var minA = centerA - boundsA;
                var maxA = centerA + boundsA;

                var minB = centerB - boundsB;
                var maxB = centerB + boundsB;
                

                if (CollisionUtility.AABBIntersection(minA, maxA, minB, maxB))
                {
                    CallIntersectActions(collider, true);
                    intersected = true;
                }
            }
            // Box-Sphere intersection
            else if(intersection is BoxIntersection && collider.intersection is SphereIntersection)
            {
                SphereIntersection intersectionA = (SphereIntersection)collider.intersection;
                BoxIntersection intersectionB = (BoxIntersection)intersection;

                var centerA = collider.transform.position + intersectionA.center;
                var centerB = transform.position + intersectionB.center;

                var radiusA = (intersectionA.radius * 0.5f) * ((collider.transform.lossyScale.x + collider.transform.lossyScale.y + collider.transform.lossyScale.z) / 3);

                var boundsB = new Vector3(
                    intersectionB.bounds.x * 0.5f * transform.lossyScale.x,
                    intersectionB.bounds.y * 0.5f * transform.lossyScale.y,
                    intersectionB.bounds.z * 0.5f * transform.lossyScale.z
                );

                var minB = centerB - boundsB;
                var maxB = centerB + boundsB;

                if (CollisionUtility.SphereAABBIntersection(centerA, radiusA, minB, maxB))
                {
                    CallIntersectActions(collider, true);
                    intersected = true;
                }
            }
            // Sphere-Box intersection
            else if (intersection is SphereIntersection && collider.intersection is BoxIntersection)
            {
                SphereIntersection intersectionA = (SphereIntersection)intersection;
                BoxIntersection intersectionB = (BoxIntersection)collider.intersection;

                var centerA = transform.position + intersectionA.center;
                var centerB = collider.transform.position + intersectionB.center;

                var radiusA = (intersectionA.radius * 0.5f) * ((transform.lossyScale.x + transform.lossyScale.y + transform.lossyScale.z) / 3);

                var boundsB = new Vector3(
                    intersectionB.bounds.x * 0.5f * collider.transform.lossyScale.x,
                    intersectionB.bounds.y * 0.5f * collider.transform.lossyScale.y,
                    intersectionB.bounds.z * 0.5f * collider.transform.lossyScale.z
                );

                var minB = centerB - boundsB;
                var maxB = centerB + boundsB;

                if (CollisionUtility.SphereAABBIntersection(centerA, radiusA, minB, maxB))
                {
                    CallIntersectActions(collider, true);
                    intersected = true;
                }
            }
        }

        if (!intersected && isIntersected)
        {
            OnIntersectExit?.Invoke(this);
            isIntersected = false;
        }
        
    }

    public void CallIntersectActions(CustomInstersectionScript other, bool intersect)
    {
        if (!intersect)
        {
            return;    
        }

        if (!isIntersected)
        {
            OnIntersectEnter?.Invoke(other);
            isIntersected = true;
        }

        OnIntersectStay?.Invoke(other);
    }


    private void OnValidate()
    {
        switch (type)
        {
            case IntersectionType.Sphere:
                if (!(intersection is SphereIntersection))
                {
                    SphereIntersection sphereTemp = new SphereIntersection();
                    sphereTemp.radius = 1f;
                    intersection = sphereTemp;
                }
                break;
            case IntersectionType.Box:
                if (!(intersection is BoxIntersection))
                {
                    BoxIntersection boxTemp = new BoxIntersection();
                    boxTemp.bounds = new(1f, 1f, 1f);
                    intersection = boxTemp;
                }
                break;
            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isIntersected ? Color.red : Color.green;

        if (intersection is SphereIntersection)
        {
            SphereIntersection sphere = (SphereIntersection)intersection;

            Gizmos.DrawWireSphere(transform.position + sphere.center, (sphere.radius * 0.5f) * ((transform.lossyScale.x + transform.lossyScale.y + transform.lossyScale.z) / 3));
        }
        else if (intersection is BoxIntersection)
        {
            BoxIntersection box = (BoxIntersection)intersection;

            Gizmos.DrawWireCube(transform.position + box.center, new Vector3(box.bounds.x * transform.lossyScale.x, box.bounds.y * transform.lossyScale.y, box.bounds.z * transform.lossyScale.z));
        }
        
    }
}

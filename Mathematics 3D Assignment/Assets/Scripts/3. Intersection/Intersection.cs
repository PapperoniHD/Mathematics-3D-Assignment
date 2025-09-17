using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Intersection { }

[System.Serializable]
public class SphereIntersection : Intersection
{
    public Vector3 center;
    public float radius;
}

[System.Serializable]
public class BoxIntersection : Intersection
{
    public Vector3 center;
    public Vector3 bounds;
}

public enum IntersectionType
{
    Sphere,
    Box
}

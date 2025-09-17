using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionManager : MonoBehaviour
{
    public static IntersectionManager instance;

    public List<CustomInstersectionScript> allColliders = new();
   
    public static IntersectionManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<IntersectionManager>();
        }
        return instance;
    }

}

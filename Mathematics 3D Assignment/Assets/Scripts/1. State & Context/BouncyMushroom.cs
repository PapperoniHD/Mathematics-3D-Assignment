using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BouncyMushroom : MonoBehaviour
{
    [Range(0f, 360f)]
    [SerializeField] private float bounceAngle = 90f;
    [SerializeField] private float radius = 2f;
    [SerializeField] private float bounceHeight = 5f;

    Animator anim;

    private CustomInstersectionScript intersection;
    private float interactBuffer = 1f;
    private float timer = 0;

    public void Start()
    {
        intersection = GetComponent<CustomInstersectionScript>();
        if (intersection != null)
        {
            intersection.OnIntersectEnter += OnIntersectEnter;
        }
        
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        timer++;
    }

    private void OnIntersectEnter(CustomInstersectionScript obj)
    {
        if (timer < interactBuffer) return;
        Vector3 hitDirection = (obj.transform.position - transform.position).normalized;
        if (Vector3.Angle(Vector3.up, hitDirection) <= bounceAngle * 0.5f)
        {
            if (obj.gameObject.TryGetComponent<PlayerController>(out PlayerController controller))
            {
                controller.AddUpwardForce(bounceHeight);
                anim.SetTrigger("Bounce");
            }
        }
        else
        {
            anim.SetTrigger("Destroy");
            Destroy(this.gameObject, 0.1f);
        }
        timer = 0;
    }


    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.forward, Quaternion.Euler(0, 0, -bounceAngle / 2f) * Vector3.up, bounceAngle, radius * transform.lossyScale.magnitude);

#endif
    }
}

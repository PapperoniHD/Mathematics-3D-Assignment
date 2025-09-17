using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GabrielAssignment;
using System;
using TMPro;

public class MovingPlatform : MonoBehaviour
{
    public enum EasingFunction { Linear, EaseInCubic, EaseOutCubic, EaseInOutCubic, EaseInSine, EaseOutSine };

    [Header("Easing Function")]
    [SerializeField] private EasingFunction currentEasingMethod = EasingFunction.EaseInCubic;

    [Header("Transform")]
    [SerializeField] private Transform endLocation;
    private Vector3 _startPos;
    private Vector3 _endPos;

    private Coroutine _movingCoroutine;
    private bool moveToStart = false;

    [Header("Color")]
    [SerializeField] private Color startColor = Color.green;
    [SerializeField] private Color endColor = Color.red;

    private Material _material;

    [Header("Animator")]
    [SerializeField] private Animator characterAnimator;

    [Header("UI")]
    [SerializeField] private TextMeshPro easingText;
    [SerializeField] private TextMeshPro currentT;

    void Start()
    {
        _startPos = transform.position;
        _endPos = endLocation.position;

        _material = GetComponent<MeshRenderer>().material;

        SetEasingMethod(EasingFunction.EaseInOutCubic);
    }

    public void SetEasingMethod(EasingFunction easing)
    {
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
        }

        characterAnimator.SetLayerWeight(1, 0);
        _material.color = startColor;
        transform.position = _startPos;
        currentEasingMethod = easing;

        easingText.SetText($"Current Easing Function: \n {currentEasingMethod}");
        currentT.SetText("0");

        _movingCoroutine = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {         
            Vector3 targetLocation = moveToStart ? _startPos : _endPos;
            Vector3 startLocation = transform.position;

            Color startColor = _material.color;
            Color targetColor = moveToStart ? this.startColor : endColor;

            float startWeight = characterAnimator.GetLayerWeight(1);
            float targetWeight = moveToStart ? 0f : 1f;

            float duration = 2;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;

                var normalizedT = time / duration;
                var easedT = EvaluateEasing(normalizedT);

                currentT.SetText(easedT.ToString("0.00"));

                characterAnimator.SetLayerWeight(1, MathUtility.Lerp(startWeight, targetWeight, easedT));
                transform.position = MathUtility.VectorLerp(startLocation, targetLocation, easedT);
                _material.color = MathUtility.ColorLerp(startColor, targetColor, easedT);

                yield return null;
            }

            moveToStart = !moveToStart;
            yield return new WaitForSeconds(1);
        }
    }

    private float EvaluateEasing(float t)
    {
        switch (currentEasingMethod)
        {
            case EasingFunction.EaseInCubic:
                return MathUtility.EaseInCubic(t);
            case EasingFunction.EaseOutCubic:
                return MathUtility.EaseOutCubic(t);
            case EasingFunction.EaseInOutCubic:
                return MathUtility.EaseInOutCubic(t);
            case EasingFunction.EaseInSine:
                return MathUtility.EaseInSine(t);
            case EasingFunction.EaseOutSine:
                return MathUtility.EaseOutSine(t);
            case EasingFunction.Linear:
                return t;
            default:
                return t;
        }
    }

    public EasingFunction GetNextEasingMethod()
    {
        int enumCount = Enum.GetNames(typeof(EasingFunction)).Length - 1;
        EasingFunction next = (int)currentEasingMethod + 1 > enumCount ? 0 : currentEasingMethod + 1;

        return next;
    }
}

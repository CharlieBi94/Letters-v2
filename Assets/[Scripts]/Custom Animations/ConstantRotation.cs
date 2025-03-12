using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    
    [SerializeField]
    float animationTime = 1f;
    RectTransform targetRect;
    public bool animate;
    float timeElapsed = 0;
    float currentTarget;
    float starting;
    int counter = 0;
    private void Start()
    {
        targetRect = GetComponent<RectTransform>();
        ResetVariables();
    }

    private void Update()
    {
        // Rotate between -15 to 15 degrees
        if (animate)
        {
            timeElapsed += Time.deltaTime;
            float newAngle = Mathf.Clamp(SimpleTween.LinearTween(starting, currentTarget, animationTime, timeElapsed),-15f, 15f);
            targetRect.rotation = Quaternion.Euler(0, 0, newAngle);
            if(newAngle == currentTarget && counter == 0)
            {
                timeElapsed = 0;
                float save = currentTarget;
                currentTarget = starting;
                starting = save;
            }
        }
    }

    public void StartAnimation()
    {
        ResetVariables();
        animate = true;
    }

    private void ResetVariables()
    {
        currentTarget = 15f;
        starting = -15f;
        timeElapsed = 0f;
    }

}

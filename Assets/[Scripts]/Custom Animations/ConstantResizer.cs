using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstantResizer : MonoBehaviour
{
    [SerializeField]
    RectTransform rect;
    [SerializeField]
    float animationTime;
    float stepSize;


    // Start is called before the first frame update
    void Start()
    {
        // Calculate stepSize based on animationTime
        stepSize = 1 / animationTime;

    }

    /// <summary>
    /// An simple animation with constant acceleration that shrinks the row
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyAnimation()
    {
        // Calculate the step-size for the animation
        float multiplier = 1.0f;
        float scaleX = rect.localScale.x;
        float scaleY = rect.localScale.y;
        while (multiplier > 0)
        {
            multiplier = Mathf.Max(0, multiplier - (stepSize * 0.016f));
            rect.localScale = new Vector3(rect.lossyScale.x, scaleY * multiplier, rect.lossyScale.z * multiplier);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect.parent as RectTransform);
            yield return new WaitForSeconds(0.016f);
        }
    }

    /// <summary>
    /// An simple animation that grows the gameobject to full size
    /// </summary>
    IEnumerator SpawnAnimation()
    {
        // Set the 
        // Calculate the step-size for the animation
        float multiplier = 0f;
        float scaleX = rect.localScale.x;
        float scaleY = rect.localScale.y;
        while (multiplier < 1)
        {
            multiplier = Mathf.Min(1f, multiplier + stepSize * 0.03f);
            rect.localScale = new Vector3(scaleX * multiplier, scaleY * multiplier, 1f);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect.parent as RectTransform);
            yield return new WaitForSeconds(0.016f);
        }

    }

    public void ShrinkAnimation()
    {
        StartCoroutine(nameof(DestroyAnimation));
    }

    public void GrowAnimation()
    {
        StartCoroutine(nameof(SpawnAnimation));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A simple animator that has a queue of animations that it will play in order
/// Currently only works for UI elements (objects with rect transform)
/// </summary>
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
public class SimpleAnimator : MonoBehaviour
{
    public enum AnimationType { POP_IN, POP_OUT };
    private Queue<AnimationData> data;
    Image targetImg;
    private bool animating;
    private AnimationData currentAnim;
    private float elapsedTime;
    RectTransform rect;
    
    // Start is called before the first frame update
    void Start()
    {
        targetImg = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        data = new Queue<AnimationData>();
        animating = false;
    }

    private void Update()
    {
        if (data.Count > 0 && animating == false)
        {
            animating = true;
            currentAnim = data.Dequeue();
            elapsedTime = 0;
            targetImg.enabled = true;
        }
        else if (data.Count == 0 && animating == false)
        {
            targetImg.sprite = null;
            targetImg.enabled = false;
        }
        if(currentAnim != null)
        {
            elapsedTime += Time.deltaTime;
            if(targetImg.sprite != currentAnim.Sprite)
            {
                targetImg.sprite = currentAnim.Sprite;
            }
            float size = 0;
            if(currentAnim.Type == AnimationType.POP_IN)
            {
                size = SimpleTween.LinearTween(0, 1, currentAnim.Duration, elapsedTime);
            }
            else if (currentAnim.Type == AnimationType.POP_OUT)
            {
                size = SimpleTween.LinearTween(1, 0, currentAnim.Duration, elapsedTime);
            }
            rect.localScale = new Vector3(size, size, size);
            if(elapsedTime > currentAnim.Duration)
            {
                animating = false;
                currentAnim = null;
            }
        }
    }

    public bool QueueAnimation(AnimationData animData)
    {
        data.Enqueue(animData);
        return true;
    }

    public bool IsAnimating()
    {

        return (data.Count > 0 || animating == true);
    }


}

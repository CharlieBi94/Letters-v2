using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    [SerializeField]
    GameObject window;

    [SerializeField]
    AudioClip openWindowClip;

    [SerializeField]
    float animationTime = 1f;
    [SerializeField]
    bool startActive = true;

    float targetValue;
    float startingValue;
    bool animate = false;
    float progress = 0f;

    RectTransform rect;
    float originalSize;

    private void Start()
    {
        window.SetActive(startActive);
    }

    public void ShowWindow()
    {
        if(window != null & !window.activeInHierarchy)
        {
            window.SetActive(true);
            if (rect == null) rect = window.GetComponent<RectTransform>();
            GrowAnimation();
            SoundController.Instance.PlayAudio(openWindowClip);
        }
        
    }

    public void CloseWindow()
    {
        if(window != null && window.activeInHierarchy)
        {
            window.SetActive(false);
            // Won't work right now because window is hidden before animation is complete
            //ShrinkAnimation();
        }
        
    }

    private void GrowAnimation()
    {
        startingValue = 0;
        targetValue = rect.localScale.x;
        progress = 0f;
        animate = true;
    }

    private void ShrinkAnimation()
    {
        startingValue = rect.localScale.x;
        targetValue = 0;
        animate = false;
    }

    private void Update()
    {
        if (animate)
        {
            progress += Time.deltaTime;
            float val = SimpleTween.LinearTween(startingValue, targetValue, animationTime, progress);
            rect.localScale = new Vector3(val, val, val);
            if (progress >= animationTime)
            {
                rect.localScale = new Vector3(targetValue, targetValue, targetValue);
                animate = false;
            }
            LayoutRebuilder.MarkLayoutForRebuild(rect);

        }
    }
}

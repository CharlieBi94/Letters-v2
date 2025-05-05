using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarouselController : MonoBehaviour
{
    [SerializeField]
    int startingIndex;
    [SerializeField]
    List<CarouselItem> items;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private RectTransform contentBoxHorizontal;
    [SerializeField]
    private IndicatorController indicatorController;

    [Header("Animation Setup")]
    [SerializeField, Range(0.25f, 1f)]
    private float duration = 0.5f;
    [SerializeField]
    private AnimationCurve easeCurve;

    public Action<int> IndexChanged;

    private int currentIndex;
    private Coroutine scrollCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        indicatorController.CreateIndicators(items.Count, startingIndex);
        currentIndex = startingIndex;
    }

    private void OnEnable()
    {
        ScrollTo(startingIndex);
    }

    private void ScrollTo(int index)
    {
        currentIndex = index;
        IndexChanged?.Invoke(index);
        float targetXPos = (float)currentIndex/ (items.Count-1);

        // stop and in duration animation
        if(scrollCoroutine != null) StopCoroutine(scrollCoroutine);
        scrollCoroutine = StartCoroutine(LerpToPos(targetXPos));
    }

    private IEnumerator LerpToPos(float targetPos)
    {
        float elapsedTime = 0f;
        float initalPos = scrollRect.horizontalNormalizedPosition;

        if (duration > 0)
        {
            while (elapsedTime <= duration)
            {
                float easeValue = easeCurve.Evaluate(elapsedTime / duration);
                float newPos = Mathf.Lerp(initalPos, targetPos, easeValue);
                scrollRect.horizontalNormalizedPosition = newPos;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        scrollRect.horizontalNormalizedPosition = targetPos;
    }

    public void ScrollToNext()
    {
        if (currentIndex == items.Count - 1) return;
        currentIndex++;
        ScrollTo(currentIndex);
    }

    public void ScrollToPrevious()
    {
        if (currentIndex == 0) return;
        currentIndex--;
        ScrollTo(currentIndex);
    }


}

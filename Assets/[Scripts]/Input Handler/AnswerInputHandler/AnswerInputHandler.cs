using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnswerInputHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private float MAX_Y_PERCENT = 0.05f;
    [SerializeField] private float MIN_SWIPE_PERCENT = 0.3f;
    [SerializeField] private Scrollbar scrollBar;

    public Action<RowController, string> AnswerSubmitted;

    private RowController row;
    private Vector2 startViewportPos;

    private void Start()
    {
        row = GetComponent<RowController>();
        scrollBar.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsValidGameState()) return;

        Camera cam = eventData.pressEventCamera ?? Camera.main;
        startViewportPos = cam.ScreenToViewportPoint(eventData.position);

        scrollBar.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (scrollBar != null)
        {
            scrollBar.size = CalculateVisualProgress(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsValidGameState() && IsValidAction(eventData))
        {
            string word = row.GetWord();
            if (!string.IsNullOrEmpty(word))
            {
                AnswerSubmitted?.Invoke(row, word);
                VibrationHandler.Instance.Vibrate();
            }
        }

        startViewportPos = Vector2.zero;
        scrollBar.gameObject.SetActive(false);
    }

    private bool IsValidGameState()
    {
        return GameManager.Instance.CurrentState == GameManager.GameState.IN_PLAY
            || GameManager.Instance.CurrentState == GameManager.GameState.GOD_MODE;
    }

    private bool IsValidAction(PointerEventData eventData)
    {
        Camera cam = eventData.pressEventCamera ?? Camera.main;
        Vector2 current = cam.ScreenToViewportPoint(eventData.position);

        float deltaX = current.x - startViewportPos.x;
        float deltaY = Mathf.Abs(current.y - startViewportPos.y);

        if (deltaY > MAX_Y_PERCENT) return false;

        return deltaX > MIN_SWIPE_PERCENT;
    }

    private float CalculateVisualProgress(PointerEventData eventData)
    {
        if (!scrollBar.IsActive()) return 0f;

        Camera cam = eventData.pressEventCamera ?? Camera.main;
        Vector2 current = cam.ScreenToViewportPoint(eventData.position);

        float deltaY = Mathf.Abs(current.y - startViewportPos.y);
        if (deltaY > MAX_Y_PERCENT) return 0f;

        float deltaX = Mathf.Max(current.x - startViewportPos.x, 0f);

        float visualStart = MIN_SWIPE_PERCENT * 0.5f;
        float progress = Mathf.InverseLerp(visualStart, MIN_SWIPE_PERCENT, deltaX);

        return Mathf.Clamp01(progress);
    }
}

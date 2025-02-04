using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Used for tracking movement for submitting answers
/// </summary>
[RequireComponent(typeof(RowController))]
public class AnswerInputHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private readonly int MAX_Y_MOVEMENT = 40;
    [SerializeField]
    private readonly int MIN_X_MOVEMENT = 50;
    [SerializeField]
    Scrollbar scrollBar;
    public Action<RowController, string> AnswerSubmitted;
    Vector2 startingPos = Vector2.zero;
    RowController row;
    RectTransform rect;
    private void Start()
    {
        row = GetComponent<RowController>();
        rect = GetComponent<RectTransform>();
        scrollBar.gameObject.SetActive(false);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsValidGameState()) return;
        startingPos = eventData.position;
        scrollBar.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(scrollBar != null)
        {
            scrollBar.size = CalculateVisualProgress(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsValidGameState())
        {
            if (IsValidAction(eventData))
            {
                string word = row.GetWord();
                if (word != string.Empty)
                {
                    AnswerSubmitted?.Invoke(row, word);
                }
            }
        }        
        // Reset the starting position for next time
        startingPos = Vector2.zero;
        scrollBar.gameObject.SetActive(false);
    }

    private bool IsValidGameState()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.IN_PLAY) return true;
        if (GameManager.Instance.CurrentState == GameManager.GameState.GOD_MODE) return true;
        return false;
    }

    private bool IsValidAction(PointerEventData eventData)
    {
        // Check Y movement, if it passes limit, automatically cancel it
        float deltaY = Mathf.Abs(eventData.position.y - startingPos.y);
        if (deltaY > MAX_Y_MOVEMENT) { return false; }
        // Check if swipe was in the same direction (left to right)
        // also do a sanity check to ensure its a large enough movement
        float deltaX = eventData.position.x - startingPos.x;
        return deltaX > MIN_X_MOVEMENT;
    }

    /// <summary>
    /// Calculates the visual value of the scroll bar based on current position of mouse
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    private float CalculateVisualProgress(PointerEventData eventData)
    {
        if (!scrollBar.IsActive()) { return 0; }
        // Check y delta to ensure user appears to still within valid constraints
        float deltaY = Mathf.Abs(eventData.position.y - startingPos.y);
        // if user has flicked their mouse up, visually cancel the move
        if (deltaY > MAX_Y_MOVEMENT) { return 0; }
        // visually show the minimum movement along x required to submit the answer
        // this equation will mean that they need to move at least half the minimum value before it will start to visually show a bar
        float deltaX = Mathf.Max((eventData.position.x - startingPos.x - MIN_X_MOVEMENT)/0.5f, 0);
        float ans = (deltaX / rect.sizeDelta.x);
        return ans;
    }


}

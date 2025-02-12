using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Data class that contains all information for a tile on the playing field
/// </summary>
public class Tile : MonoBehaviour, IUICollider, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public enum TileState { IN_PLAY, PHANTOM, OFF }
    public Action<Tile> TileChange;
    private char character;
    private TileState currentState;

    public event Action SizeChanged;
    public event Action Spawned;

    private RowController rowController;

    private AnswerInputHandler ansInput;
    [SerializeField]
    bool isDragging = false;

    public bool playerAdded { get; private set; }
    public char GetChar()
    {
        return character;
    }

    private void Start()
    {
        rowController = GetComponentInParent<RowController>();
        ansInput = GetComponentInParent<AnswerInputHandler>();
    }

    public void Update()
    {
        if (currentState == TileState.IN_PLAY || currentState == TileState.PHANTOM)
        {
            InvokeSizeChanged();
        }
    }

    public void SetChar(char c)
    {
        character = char.ToLower(c);
        TileChange?.Invoke(this);
    }

    public void SetState(TileState state)
    {
        if (state == currentState) return;
    }

    public TileState GetCurrentState()
    {
        return currentState;
    }

    public void SetPlayerAdded(bool added)
    {
        playerAdded = added;
    }

    public void InvokeSizeChanged()
    {
        SizeChanged?.Invoke();
    }
    public RowController GetParentRow()
    {
        if(rowController == null)
        {
            rowController = GetComponentInParent<RowController>();
        }

        return rowController;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            ansInput.OnDrag(eventData);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = InventoryInputHandler.Instance.HandleTilePointerDown(this);
        if (!isDragging)
        {
            if(ansInput == null) ansInput = GetComponentInParent<AnswerInputHandler>();
            ansInput.OnBeginDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        if (isDragging)
        {
            InventoryInputHandler.Instance.HandleTilePointerUp();
            isDragging = false;
        }
        else
        {
            ansInput.OnEndDrag(eventData);
        }
    }

    public void StartSpawnAnimation()
    {
        Spawned?.Invoke();
    }

    public void HandleWildCardSelect(GameObject selectionScreen)
    {

    }

}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(OptionData))]
public class OptionBehaviour : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
{
    OptionData data;
    RectTransform uiCanvas;
    GameObject draggedObject;
    private void Start()
    {
        data = GetComponent<OptionData>();
    }

    public void SetUICanvas(RectTransform rect)
    {
        uiCanvas = rect;
    }
    internal void HandleGodModeSelect()
    {
        GameManager.Instance.StartGodMode();
        SwapInputHandler.Instance.Restart();
    }

    public void HandleWildCardSelect()
    {
        //throw new NotImplementedException();
        // Look for eligible rows
        List<RowController> eligibleRows = SwapInputHandler.Instance.playArea.GetEmptyRows();
        foreach(RowController row in eligibleRows)
        {
            Tile t = row.GetAllTiles()[0];
            t.SetChar('?');
            t.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            t.gameObject.GetComponent<Button>().onClick.AddListener(() => { SwapInputHandler.Instance.ShowWildCardOptions(t); });

        }
        SwapInputHandler.Instance.CompleteLevelUp();
    }

    public void HandleLetterSwapSelect(char targetChar, char newChar)
    {
        SwapInputHandler.Instance.SetNewLetter(newChar);
        SwapInputHandler.Instance.SetOldLetter(targetChar);
        SwapInputHandler.Instance.CompleteSwap();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if(IsDraggable())
        {
            // Spawn phantom object
            draggedObject = TileFactory.Instance.SpawnTile(uiCanvas, true, data.content);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!draggedObject) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Perform the raycast
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, 1<<8);
        if (hit.collider != null)
        {
            InventorySlot slot = hit.collider.gameObject.GetComponent<InventorySlot>();
            // Check what to do
            if(data.content == '-')
            {
                if (!slot.MiddleLetter)
                {
                    slot.UpgradeMiddle();
                    SwapInputHandler.Instance.CompleteLevelUp();
                }
                
            }
            else
            {
                HandleLetterSwapSelect(slot.Content[0], data.content);
            }
            
        }
        Destroy(draggedObject);
        draggedObject = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!draggedObject) return;
        // Place dragged Object at mouse
        // if setting it to the canvas, ignore the layout and display its preferred size
        LayoutElement layout = draggedObject.GetComponent<LayoutElement>();
        draggedObject.transform.SetParent(uiCanvas, false);
        layout.ignoreLayout = true;
        draggedObject.transform.position = Input.mousePosition;
        //tileRect.sizeDelta = new Vector2(layout.preferredWidth, layout.preferredHeight);
    }

    private bool IsDraggable()
    {
        if (data.content == '?') return false;
        if (data.content == '*') return false;
        return true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (data.content == '?')
        {
            HandleWildCardSelect();
        }
        else if (data.content == '*')
        {
            HandleGodModeSelect();
        }
    }
}

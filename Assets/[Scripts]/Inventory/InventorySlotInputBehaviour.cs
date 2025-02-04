using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventorySlot))]
public class InventorySlotInputBehaviour : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    InventorySlot inv;

    private void Start()
    {
        inv = GetComponent<InventorySlot>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InventoryInputHandler.Instance.HandleInventoryPointerUp();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        InventoryInputHandler.Instance.HandleInventoryPointerDown(inv.Content, inv.MiddleLetter);
    }

}

using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventorySlot))]
public class InventorySlotInputBehaviour : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    InventorySlot inv;
    private void Start()
    {
        inv = GetComponent<InventorySlot>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryInputHandler.Instance.HandleInventoryPointerDown(inv.Content, inv.MiddleLetter);

    }

    public void OnDrag(PointerEventData eventData)
    {
        InventoryInputHandler.Instance.PlaceTile();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        InventoryInputHandler.Instance.HandleInventoryPointerUp();
    }
}

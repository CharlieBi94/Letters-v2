using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform), typeof(IUICollider))]
public class BoxColliderResizer : MonoBehaviour
{
    BoxCollider2D col;
    RectTransform rect;
    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rect = GetComponent<RectTransform>();
        // Subscribe to the event
        IUICollider observ = GetComponent<IUICollider>();
        observ.SizeChanged += OnSizeChanged;
        // Manually Update the size to ensure collider currently matches the size
        OnSizeChanged();
    }

    private void OnSizeChanged()
    {
        Vector2 size = rect.sizeDelta;
        if(rect.pivot.y != 0.5)
        {
            col.offset = new Vector2(0, size.y/2);
        }
        col.size = rect.sizeDelta;
    }
}

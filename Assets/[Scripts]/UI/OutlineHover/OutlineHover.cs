using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class OutlineHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!outline.enabled)
        {
            outline.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (outline.enabled)
        {
            outline.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (outline)
        {
            outline.enabled = false;
        }
    }



}

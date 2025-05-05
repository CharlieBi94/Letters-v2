using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    [SerializeField]
    Image iconImg;
    [SerializeField]
    Sprite selectedIcon;
    [SerializeField]
    Sprite unselectedIcon;

    private bool isSelected = false;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void UpdateVisual()
    {
        if (isSelected)
        {
            iconImg.sprite = selectedIcon;
        }
        else
        {
            iconImg.sprite = unselectedIcon;
        }
    }
    public void SetSelctedState(bool isSelected)
    {
        if (isSelected == this.isSelected) return;
        this.isSelected = isSelected;
        UpdateVisual();
    }
}

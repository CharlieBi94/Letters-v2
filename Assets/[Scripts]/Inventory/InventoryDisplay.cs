using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InventorySlot))]
public class InventoryDisplay : MonoBehaviour
{
    InventorySlot inv;
    [SerializeField]
    Image backgroundImg;
    [SerializeField]
    Image contentImg;
    [SerializeField]
    GameObject leftDashImg;
    [SerializeField]
    GameObject rightDashImg;
    // Start is called before the first frame update
    void Start()
    {
        inv = GetComponent<InventorySlot>();
        OnContentChanged();
        inv.ContentChanged += OnContentChanged;
    }

    private void OnContentChanged()
    {
        if(inv.IsEmpty())
        {
            contentImg.sprite = null;
        }
        else if(inv.Content.Length == 1)
        {
            contentImg.sprite = LetterSpriteLoader.GetSprite(inv.Content[0]);
        }

        bool showDashes = inv.MiddleLetter;
        leftDashImg.SetActive(showDashes);
        rightDashImg.SetActive(showDashes);
    }

    private void OnDestroy()
    {
        if(inv != null)
        {
            inv.ContentChanged -= OnContentChanged;
        }
    }
}

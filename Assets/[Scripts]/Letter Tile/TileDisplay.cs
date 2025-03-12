using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TileDisplay : MonoBehaviour
{
    [SerializeField]
    Image letterImg;
    [SerializeField]
    Image backgroundImg;

    [SerializeField]
    Color playerAdded;
    [SerializeField]
    Color systemAdded;

    [SerializeField]
    float animationTime = 1f;

    bool animate = false;
    float progress = 0f;

    RectTransform rect;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        Tile tile = gameObject.GetComponent<Tile>();
        OnTileChange(tile);
        tile.TileChange += OnTileChange;
        tile.Spawned += OnTileSpawn;
        
    }

    private void OnTileChange(Tile tile)
    {
        letterImg.sprite = LetterSpriteLoader.GetSprite(tile.GetChar(), true);
        if (tile.playerAdded)
        {
            backgroundImg.color = playerAdded;
        }
        else
        {
            backgroundImg.color = systemAdded;
        }
    }

    private void Update()
    {
        if (animate)
        {
            LayoutElement layout = GetComponent<LayoutElement>();
            progress += Time.deltaTime;
            float val = SimpleTween.LinearTween(0f, 1f, animationTime, progress);
            rect.localScale = new Vector3(val, val, val);
            LayoutRebuilder.MarkLayoutForRebuild(rect);
            if (progress >= animationTime)
            {
                rect.localScale = Vector3.one;
                animate = false;
            }
        }
    }

    private void OnTileSpawn()
    {
        //print("Starting resize");
        animate = true;
        progress = 0f;
        //StopAllCoroutines();
        //StartCoroutine(SpawnAnimation());
    }
}
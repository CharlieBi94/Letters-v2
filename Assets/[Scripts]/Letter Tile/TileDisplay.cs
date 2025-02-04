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
    float stepSize = 0.1f;
    [SerializeField]
    float animationTime = 1f;

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
        letterImg.sprite = LetterSpriteLoader.GetSprite(tile.GetChar());
        if (tile.playerAdded)
        {
            backgroundImg.color = playerAdded;
        }
        else
        {
            backgroundImg.color = systemAdded;
        }
    }

    private void OnTileSpawn()
    {
        print("Starting resize");
        StopAllCoroutines();
        StartCoroutine(SpawnAnimation());
    }

    IEnumerator SpawnAnimation()
    {
        if (rect == null) rect = GetComponent<RectTransform>();
        float timeElapsed = 0f;
        while (timeElapsed <= animationTime)
        {
            float val = SimpleTween.LinearTween(0f, 1f, animationTime, timeElapsed);
            rect.localScale = new Vector3(val, val, val);
            yield return new WaitForSeconds(stepSize);
            timeElapsed += stepSize;
        }
        rect.localScale = Vector3.one;
    }

}
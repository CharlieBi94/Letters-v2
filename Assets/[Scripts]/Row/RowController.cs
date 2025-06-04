using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that represents a row in the play area
/// Used to manage tiles that for the word in the row
/// </summary>
public class RowController : MonoBehaviour, IUICollider
{
    public Action<string, RowController> SubmitAnswer;
    private List<Tile> tiles = new();
    [SerializeField]
    RectTransform tilesContainer;
    string word;
    public event Action SizeChanged;
    [SerializeField]
    private int maxTileCount;
    private void Start()
    {
        word = string.Empty;
        InventoryInputHandler.Instance.LetterAdded += OnLetterAdded;
    }

    private void Update()
    {
        InvokeSizeChanged();
    }

    /// <summary>
    /// Add a letter to the row, set position to -1 to place it at the end
    /// </summary>
    /// <param name="position"></param>
    /// <param name="letter"></param>
    /// <param name="playerAdded"></param>
    public Tile AddLetter(int position, char letter, bool playerAdded=true)
    {
        Tile tile;
        if(position < tiles.Count)
        {
            tile = SpawnLetter(playerAdded, letter, position);
        }
        else
        {
            tile = SpawnLetter(playerAdded, letter, tiles.Count);
        }
        tiles = tilesContainer.GetComponentsInChildren<Tile>().ToList();
        return tile;
    }

    public Tile AddLetter(Tile tile, int position, bool playerAdded = true)
    {
        GameObject tileObj = tile.gameObject;
        tileObj.transform.SetParent(tilesContainer, false);
        if(position > tiles.Count) position = tiles.Count;
        tileObj.transform.SetSiblingIndex(position);
        tile.gameObject.GetComponent<LayoutElement>().ignoreLayout = false;
        tile.StartSpawnAnimation();
        tiles.Add(tile);
        return tile;
    }

    public bool CanAdd()
    {
        if (tiles.Count > maxTileCount) return false;
        return true;
    }

    private void OnLetterAdded(RowController row, bool countMoves)
    {
        if (row != this) return;
        tiles = tilesContainer.GetComponentsInChildren<Tile>().ToList();
        if (countMoves) GameManager.Instance.IncrementMoves();

    }
    private Tile SpawnLetter(bool playerAdded, char letter, int position)
    {
        var tileObj = TileFactory.Instance.SpawnTile(tilesContainer, playerAdded, letter);
        tileObj.transform.SetSiblingIndex(position);
        Tile tile = tileObj.GetComponent<Tile>();
        tile.StartSpawnAnimation();
        if (playerAdded) { GameManager.Instance.IncrementMoves(); }
        return tile;
    }

    public string GetWord()
    {
        word = string.Empty;
        foreach (Tile t in tiles)
        {
            word += t.GetChar();
        }
        return word;
    }

    public void InvokeSizeChanged()
    {
        SizeChanged?.Invoke();
    }

    /// <summary>
    /// Gets the position of the letter tile in the hierarchy
    /// the positional index 
    /// </summary>
    /// <param name="letter"></param>
    /// <returns>-1 if unable to find letter</returns>
    public int GetPositionIndex(Tile tile)
    {
        tiles = tilesContainer.GetComponentsInChildren<Tile>().ToList();
        for (int i = 0; i < tiles.Count; i++)
        {
            if(tiles[i] == tile) return i;
        }
        return -1;
    }

    public RectTransform GetContainer()
    {
        return tilesContainer;
    }

    public void RemoveTile(Tile t)
    {
        tiles.Remove(t);
    }

    public List<Tile> GetAllTiles()
    {
        return tilesContainer.GetComponentsInChildren<Tile>().ToList();
    }

    public int Count(Tile tile)
    {
        tiles = tilesContainer.GetComponentsInChildren<Tile>().ToList();
        int ans = 0;
        foreach(Tile t in tiles)
        {
            if(t != tile) ans++;
        }
        return ans;
    }
}

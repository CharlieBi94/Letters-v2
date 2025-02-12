using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the rows in the play area.
/// </summary>
public class PlayAreaController : MonoBehaviour
{
    [SerializeField]
    public int MAX_ROWS;    
    [SerializeField]
    private RectTransform parent;

    public Action<string> AnswerSubmitted;

    private List<RowController> rows;
    private Dictionary<Tile, RowController> tilesDict;

    // Count is used for nameing gameobjects for debugging
    private int count = 0;
    private void Start()
    {
        rows = parent.GetComponentsInChildren<RowController>().ToList();
        tilesDict = new();
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        yield return new WaitUntil(() => GameManager.Instance);
        char[] letters = GameManager.Instance.GetInitalFieldState();
        foreach (char c in letters)
        {
            OnSpawnNewRow(c, false);
        }
        GameManager.Instance.SpawnNewRow += OnSpawnNewRow;
    }

    public void OnSpawnNewRow(char c, bool playerAdded)
    {
        RowController row = SpawnRow();
        RectTransform parent = row.gameObject.GetComponent<RectTransform>();
        row.gameObject.GetComponent<ConstantResizer>().GrowAnimation();
        Tile t = row.AddLetter(0, c, playerAdded);
        tilesDict.Add(t, row);
        row.gameObject.name += count;
        count++;
    }

    private RowController SpawnRow()
    {
        var rowObj = RowFactory.Instance.SpawnRow(parent);
        RowController row = rowObj.GetComponent<RowController>();
        AnswerInputHandler ansHandler = row.gameObject.GetComponent<AnswerInputHandler>();
        ansHandler.AnswerSubmitted += OnAnswerSubmit;
        rows.Insert(0, row);
        rowObj.transform.SetAsLastSibling();
        // Check if we we are at max rows
        if (rows.Count > MAX_ROWS)
        {
            GameManager.Instance.TrySetGameState(GameManager.GameState.LOST);
        }
        return row;
    }

    private void OnAnswerSubmit(RowController row, string word)
    {
        row.gameObject.GetComponent<AnswerInputHandler>().AnswerSubmitted -= OnAnswerSubmit;
        List<Tile> tiles = row.GetAllTiles();
        foreach (Tile tile in tiles)
        {
            tilesDict.Remove(tile);
        }
        rows.Remove(row);
        Destroy(row.gameObject);
        AnswerSubmitted?.Invoke(word);
    }

    private void OnDestroy()
    {
        if(GameManager.Instance)
        {
            GameManager.Instance.SpawnNewRow -= OnSpawnNewRow;
        }
        ClearContents();
    }

    private void ClearContents()
    {
        foreach (var row in rows)
        {
            if (row == null) return;
            if (row.gameObject.TryGetComponent<AnswerInputHandler>(out var ansInput))
            {
                ansInput.AnswerSubmitted -= OnAnswerSubmit;
            }

        }
    }

    public void Restart(List<char> startingLetters)
    {
        ClearContents();
        foreach(var row in rows)
        {
            if (row)
            {
                Destroy(row.gameObject);
            }
            
        }
        rows.Clear();
        tilesDict.Clear();
        foreach(char c in startingLetters)
        {
            OnSpawnNewRow(c, false);
        }
    }

    public int RowCount()
    {
        return rows.Count;
    }

    /// <summary>
    /// Add a tile to the specified position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public void AddTile(TilePosition position)
    {
        TilePosition currentPos = GetTilePosition(position.GetTile());
        // Check to see if current position same as target position
        if (currentPos == position) return;       
        if (tilesDict.ContainsKey(position.GetTile()))
        {
            tilesDict[position.GetTile()] = position.GetRow();
        }
        else
        {
            tilesDict.Add(position.GetTile(), position.GetRow());
        }
        RowController row = position.GetRow();
        row.AddLetter(position.GetTile(), position.IndexPosition());
    }

    /// <summary>
    /// Tries to find specified tile and returns position data in the form of a TilePosition
    /// </summary>
    /// <param name="t"></param>
    /// <returns>Returns null if tile does not exist in play area</returns>
    public TilePosition GetTilePosition(Tile t)
    {
        if (!tilesDict.ContainsKey(t)) return null;
        RowController row = tilesDict[t];
        if (row == null) return null;
        int index = row.GetPositionIndex(t);
        if (index == -1) return null;
        return new TilePosition(t, row, index);
    }

    public List<RowController> GetEmptyRows()
    {
        List<RowController> ans = new();
        foreach(RowController row in rows)
        {
            if(row.TileCount() == 1)
            {
                ans.Add(row);
            }
        }
        return ans;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the rows in the play area.
/// </summary>
public class PlayAreaController : MonoBehaviour
{
    [SerializeField]
    private RectTransform rowContainer;
    [SerializeField]
    private CatAnimator catAnim;
    public readonly int MAX_ROWS = 6;
    public Action<string, Vector2> AnswerSubmitted;
    private List<RowController> rows;
    private Dictionary<Tile, RowController> tilesDict;
    public Action<Tile> TileSpawned;

    // Count is used for nameing gameobjects for debugging
    private int count = 0;
    private void Start()
    {

        rows = rowContainer.GetComponentsInChildren<RowController>().ToList();
        tilesDict = new();
        StartCoroutine(Initialize());
    }
    
    IEnumerator Initialize()
    {
        yield return new WaitUntil(() => GameManager.Instance);
        string[] substrings = GameManager.Instance.GetInitalFieldState();
        foreach (string s in substrings)
        {
            OnSpawnNewRow(s, false);
        }
        GameManager.Instance.SpawnNewRow += OnSpawnNewRow;
    }

    public void OnSpawnNewRow(string s, bool playerAdded)
    {
        RowController row = SpawnRow();
        row.gameObject.GetComponent<ConstantResizer>().GrowAnimation();
        for(int i = 0; i < s.Length; i++)
        {
            Tile t = row.AddLetter(-1, s[i], playerAdded);
            tilesDict.Add(t, row);
            if (i == 0) TileSpawned?.Invoke(t);
        }
        row.gameObject.name += count;
        count++;
    }


    private RowController SpawnRow()
    {
        var rowObj = RowFactory.Instance.SpawnRow(rowContainer);
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
        Vector2 pos = row.GetComponent<RectTransform>().localPosition;
        RemoveRow(row);
        AnswerSubmitted?.Invoke(word, pos);
        if(rows.Count == 0)
        {
            GameManager.Instance.SpawnNextLetter();
        }
    }

    private void RemoveRow(RowController row)
    {
        List<Tile> tiles = row.GetAllTiles();
        foreach (Tile tile in tiles)
        {
            tilesDict.Remove(tile);
        }
        rows.Remove(row);
        Destroy(row.gameObject);
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
        while(rows.Count >= 1)
        {
            RowController row = rows[0];
            rows.Remove(row);
            if(row != null)
            {
                if (row.gameObject.TryGetComponent<AnswerInputHandler>(out var ansInput))
                {
                    ansInput.AnswerSubmitted -= OnAnswerSubmit;
                }
                Destroy(row.gameObject);
            }
        }
    }

    public void Restart(List<string> startingSubstring)
    {
        ClearContents();
        tilesDict.Clear();
        foreach(string s in startingSubstring)
        {
            OnSpawnNewRow(s, false);
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

    /// <summary>
    /// This returns only the tiles added by the system.
    /// </summary>
    /// <returns></returns>
    public List<RowController> GetSysTileOnlyRows()
    {
        List<RowController> ans = new();
        foreach(RowController row in rows)
        {
            bool allSysTiles = true;
            foreach(Tile t in row.GetAllTiles())
            {
                if (t.playerAdded)
                {
                    allSysTiles = false;
                    break;
                }
            }
            if(allSysTiles) ans.Add(row);
        }
        return ans;
    }

    /// <summary>
    /// Returns all rows that have no tiles in them
    /// </summary>
    /// <returns></returns>
    public void RemoveEmptyRows()
    {
        List<RowController> emptyRows = new();
        foreach(var row in rows)
        {
            if (row.GetAllTiles().Count == 0) emptyRows.Add(row);
        }

        foreach(var row in emptyRows)
        {
            RemoveRow(row);
        }
    }

}

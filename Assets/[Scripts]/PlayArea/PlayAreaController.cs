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
    [SerializeField]
    Animator animator;
    [SerializeField]
    Vector2 leftOffset;
    [SerializeField]
    Vector2 rightOffset;

    public Action<string, Vector2> AnswerSubmitted;

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
        RectTransform parent = row.gameObject.GetComponent<RectTransform>();
        row.gameObject.GetComponent<ConstantResizer>().GrowAnimation();
        bool hasAnimated = false;
        for(int i = 0; i < s.Length; i++)
        {
            Tile t = row.AddLetter(-1, s[i], playerAdded);
            tilesDict.Add(t, row);
            if (!hasAnimated)
            {
                // Calculate position:
                Vector2 tilePos = t.GetComponent<RectTransform>().position;
                // Figure out if tile position is left or right of the screen
                if (tilePos.x < 0)
                {
                    Vector3 scale = animator.transform.localScale;
                    if (scale.x > 0)
                    {
                        scale.x *= -1;
                    }
                    animator.transform.localScale = scale;
                    animator.transform.position = tilePos + leftOffset;
                }
                else
                {
                    Vector3 scale = animator.transform.localScale;
                    if (scale.x < 0)
                    {
                        scale.x *= -1;
                    }
                    animator.transform.localScale = scale;
                    animator.transform.position = tilePos + rightOffset;
                }

                //Play Animation                
                animator.Play("CatSlap", -1, 0f);

                hasAnimated = true;
            }
        }
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
        Debug.Log(row.GetComponent<RectTransform>().localPosition);
        AnswerSubmitted?.Invoke(word, row.GetComponent<RectTransform>().localPosition);
        RemoveRow(row);
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
        foreach (var row in rows)
        {
            if (row == null) return;
            if (row.gameObject.TryGetComponent<AnswerInputHandler>(out var ansInput))
            {
                ansInput.AnswerSubmitted -= OnAnswerSubmit;
            }
        }
    }

    public void Restart(List<string> startingSubstring)
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
    /// This 
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

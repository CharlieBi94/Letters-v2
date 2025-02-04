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
    private int MAX_ROWS;
    private List<RowController> rows;
    [SerializeField]
    private RectTransform parent;
    public Action<string> AnswerSubmitted;

    // Count is used for nameing gameobjects for debugging
    private int count = 0;
    private void Start()
    {
        rows = parent.GetComponentsInChildren<RowController>().ToList();
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
        row.AddLetter(0, c, playerAdded);
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
        return row;
    }

    private void OnAnswerSubmit(RowController row, string word)
    {
        row.gameObject.GetComponent<AnswerInputHandler>().AnswerSubmitted -= OnAnswerSubmit;
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
        foreach(char c in startingLetters)
        {
            OnSpawnNewRow(c, false);
        }
    }

    public int RowCount()
    {
        return rows.Count;
    }
}

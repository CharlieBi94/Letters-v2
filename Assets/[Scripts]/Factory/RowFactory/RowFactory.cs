using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowFactory : Singleton<RowFactory>
{
    [SerializeField]
    private GameObject rowPrefab;

    public GameObject SpawnRow(RectTransform rect)
    {
        GameObject row = GetRow();
        row.transform.SetParent(rect, false);
        return row;
    }

    private GameObject GetRow()
    {
        var obj = Instantiate(rowPrefab);
        return obj;
    }

}

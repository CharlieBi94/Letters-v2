using UnityEngine;

public class TileFactory : Singleton<TileFactory>
{
    [SerializeField]
    private GameObject tilePrefab;

    public GameObject SpawnTile(RectTransform rect, bool playerAdded, char c)
    {
        GameObject obj = GetTile();
        obj.transform.SetParent(rect, false);
        Tile t = obj.GetComponent<Tile>();
        t.SetPlayerAdded(playerAdded);
        t.SetChar(c);
        return obj;
    }

    private GameObject GetTile()
    {
        var obj = Instantiate(tilePrefab);
        return obj;
    }


}

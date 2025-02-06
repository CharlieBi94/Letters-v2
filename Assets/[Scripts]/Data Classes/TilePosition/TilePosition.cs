using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePosition
{
    Tile tile;
    RowController parentRow;
    int indexPosition;

    public TilePosition(Tile tile, RowController parentRow, int position)
    {
        this.tile = tile;
        this.parentRow = parentRow;
        indexPosition = position;
    }

    public Tile GetTile()
    {
        return tile;
    }

    public RowController GetRow()
    {
        return parentRow;
    }

    public int IndexPosition()
    {
        return indexPosition;
    }

    public override bool Equals(object obj)
    {
        if (obj is not TilePosition other) return false;
        return tile == other.tile &&
              parentRow == other.parentRow &&
              indexPosition == other.indexPosition;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(tile, parentRow, indexPosition);
    }

    public static bool operator ==(TilePosition left, TilePosition right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(TilePosition left, TilePosition right)
    {
        return !(left == right);
    }
}

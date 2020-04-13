using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class PaintOnLayerRuleTile : RuleTile<PaintOnLayerRuleTile.Neighbor>
{
    [Serializable]
    public struct PaintTile
    {
        public Vector3Int offset;
        public TileBase paintTile;
    }
    public List<PaintTile> paintTileList = new List<PaintTile>();

    public class Neighbor : RuleTile.TilingRule.Neighbor 
    {
    }

    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        base.RefreshTile(location, tilemap);
        var foilageLayer = tilemap.GetComponent<FoilageAddTileLayer>();
        if (foilageLayer != null)
        {
            for (int y = -1; y < 2; ++y)
            {
                for (int x = -1; x < 2; ++x)
                {
                    var neighbour = new Vector3Int(location.x + x, location.y + y, location.z);
                    if (neighbour == location || tilemap.GetTile(neighbour) == this)
                        foilageLayer.AddUpdate(neighbour, this);
                }
            }
        }
    }
}

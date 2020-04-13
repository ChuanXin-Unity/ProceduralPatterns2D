using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
[RequireComponent(typeof(Tilemap))]
public class FoilageAddTileLayer : MonoBehaviour
{
    private struct AddUpdateData
    {
        public Vector3Int position;
        public PaintOnLayerRuleTile tile;
    }
    private List<AddUpdateData> updateList = new List<AddUpdateData>();
    private Tilemap baseTilemap;

    public Tilemap paintTilemap = null;

    void Awake()
    {
        baseTilemap = GetComponent<Tilemap>();
        if (paintTilemap == null)
            paintTilemap = baseTilemap;
    }

    public void AddUpdate(Vector3Int position, PaintOnLayerRuleTile tile)
    {
        updateList.Add(new AddUpdateData() { position = position, tile = tile });
    }

    public void LateUpdate()
    {
        foreach (var update in updateList)
        {
            if (update.tile == null)
                continue;

            var tile = baseTilemap.GetTile<PaintOnLayerRuleTile>(update.position);
            if (tile != null)
            {
                foreach (var paintTile in update.tile.paintTileList)
                {
                    if (!baseTilemap.HasTile(update.position + paintTile.offset))
                        paintTilemap.SetTile(update.position + paintTile.offset, paintTile.paintTile);
                }
            }
            else if (baseTilemap != paintTilemap && !baseTilemap.HasTile(update.position))
            {
                foreach (var paintTile in update.tile.paintTileList)
                {
                    if (paintTilemap.HasTile(update.position + paintTile.offset))
                        paintTilemap.SetTile(update.position + paintTile.offset, null);
                }
            }
        }
        updateList.Clear();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlighter : MonoBehaviour {
    public Color unitHighlight;
    public Color emptyTileHighlight;
    public SpriteRenderer sprite;
	
    public void setPos(Tile tile)
    {
        if (tile == null) return;
        transform.position = tile.transform.position;
        if (tile.unit != null) sprite.color = unitHighlight;
        else sprite.color = emptyTileHighlight;
    }
}

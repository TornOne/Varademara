using UnityEngine;

public class TileHighlighter : MonoBehaviour {
	public SpriteRenderer sprite;

	public void SetPos(Tile tile) {
		if (tile == null) {
			return;
		}
		transform.position = tile.transform.position;
		Color c = tile.sprite.color;
		//Adjusts automatically to be distinct from the underlying color
		sprite.color = new Color(1 - c.r, 1 - c.g, 1 - c.b);
	}
}

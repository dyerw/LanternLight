using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGUI : MonoBehaviour {

	public World world;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void DrawQuad(Rect position, Color color) {
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(position, GUIContent.none);
	}


	public void DrawHealthBars(List<Entity> entities) {
		foreach (Entity entity in entities) {
			DrawQuad (new Rect (0, 0, 10, 10), Color.red);
		}
	}

	void OnGUI() {
		if (world != null) {
			DrawHealthBars (world.Entities);
		}
	}
}


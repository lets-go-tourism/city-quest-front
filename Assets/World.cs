using Assets;
using UnityEngine;
using System.Collections;

public class World : MonoBehaviour 
{
    public Vector2 Center;
    public int Zoom;

	void Start () 
    {
        var go = new GameObject("tile");
        var tile = go.AddComponent<Tile>();
        StartCoroutine(tile.CreateTile(this, new Vector2(Center.x, Center.y), new Vector2(0, 0), Zoom));
    }
	
	void Update () 
    {
	
	}

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 500, 30), Center.x.ToString());
        GUI.Label(new Rect(10, 50, 500, 30), Center.y.ToString());
    }
}

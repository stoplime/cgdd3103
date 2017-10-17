using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Item class that represents an item being in the inventory.
/// </summary>
public class Item : MonoBehaviour {

    public int id;

    public Texture sprite;

    public Vector2 size;

    // center coordinate
    public Vector2 pos;

    public GameObject player;

    private Inventory inventory;

    public void Drag()
    {
        // pos += deltaPos;
        pos = new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y);

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            inventory.ItemDragTo(pos);
        }
    }

	// Use this for initialization
	void Start () {
		inventory = player.GetComponent<Inventory>();
        pos = new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y);
	}
	
	// Update is called once per frame
    void OnGUI () {
        GUI.depth = -1;
        GUI.DrawTexture(GuiClass.GetCenteredRect(pos, size), sprite, ScaleMode.StretchToFill, true, 10.0F, Color.green, 0, 1);
    }
}

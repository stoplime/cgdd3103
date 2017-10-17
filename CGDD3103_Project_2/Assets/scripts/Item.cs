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

    public virtual void Drag()
    {
        pos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse X"));

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            inventory.ItemDragTo(pos);
        }
    }

	// Use this for initialization
	void Start () {
		inventory = player.GetComponent<Inventory>();
        size = new Vector2(32, 32);
	}
	
	// Update is called once per frame
    void OnGUI () {
        GUI.DrawTexture(new Rect(pos.x-size.x/2, pos.y-size.y/2, size.x, size.y), sprite, ScaleMode.ScaleToFit, true, 10.0F, Color.green, 0, 1);
    }
}

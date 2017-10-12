using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public bool OpenInventoryToggle;

	public int HotBarSlots = 4;
	public int InternalVisibleSlots = 6;
	public int InternalMaxSlots = 12;

	/// <summary>
	/// the data structure for inventory storing the (id, count) of each item
	/// </summary>
	private List< List<int> > inv;

	private Vector2 screenCenter;
	private Vector2 menuSize;
	private int margin;

	private Vector2 itemSize;

	private GUIStyle inventoryStyle = null;

	private void addEmptyInventory()
	{
		inv.Add(new List<int>());
		inv[inv.Count-1].Add(-1); 	// default id of -1 for nothing
		inv[inv.Count-1].Add(0);	// default count of items as 0
	}

	public int getInvItemID(int index)
	{
		if(index < inv.Count && index >= 0)
		{
			if(inv[index][1] > 0){
				return inv[index][0];
			}
		}
		return -1;
	}

	public int removeInvItem(int index)
	{
		if(index < inv.Count && index >= 0)
		{
			if(inv[index][1] > 0){
				int tempID = inv[index][0];
				if (inv[index][1] > 2)
				{
					inv[index][1]--;
				}
				else
				{
					inv[index].RemoveAt(0);
					if (index < InternalVisibleSlots)
					{
						addEmptyInventory();
					}
				}
				return tempID;
			}
		}
		return -1;
	}

	public bool setInvItem(int ID)
	{
		// Check for existing inventory entry
		for (int i = 0; i < inv.Count; i++)
		{
			if(inv[i][0] == ID){
				if (inv[i][1] > 0 && inv[i][1] < 64)
				{
					inv[i][1]++;
					return true;
				}
			}
		}
		// Check for empty slots in inventory
		for (int i = 0; i < inv.Count; i++)
		{
			if(inv[i][0] == -1){
				inv[i][0] = ID;
				inv[i][1] = 1;
				return true;
			}
		}
		// Check to expand inventory
		if (inv.Count < InternalMaxSlots)
		{
			addEmptyInventory();
			inv[inv.Count-1][0] = ID;
			inv[inv.Count-1][0] = 1;
			return true;
		}
		return false;
	}

	private void InitStyles()
	{
		if( inventoryStyle == null )
		{
			inventoryStyle = new GUIStyle( GUI.skin.box );
			inventoryStyle.normal.background = MakeTex( 2, 2, new Color( 0.2f, 0.5f, 0.2f, 0.8f ) );
		}
	}

	private Texture2D MakeTex( int width, int height, Color col )
	{
		Color[] pix = new Color[width * height];
		for( int i = 0; i < pix.Length; ++i )
		{
			pix[ i ] = col;
		}
		Texture2D result = new Texture2D( width, height );
		result.SetPixels( pix );
		result.Apply();
		return result;
	}

	// Use this for initialization
	void Start () {
		OpenInventoryToggle = false;
		inv = new List<List<int>>();
		for (int i = 0; i < InternalVisibleSlots; i++)
		{
			addEmptyInventory();
		}
		margin = 15;
		itemSize = new Vector2(64, 64);
	}
	
	// Update is called once per frame
	void Update () {
        // Update the GUI sizes if the screen were to resize
        screenCenter = new Vector2(Screen.width/2, Screen.height/2);
        menuSize = new Vector2(Screen.width * 4/5, Screen.height * 4/5);

		// toggle the Inventory menu
        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     OpenInventoryToggle = !OpenInventoryToggle;
        // }

		// closes the menu if the main menu is open
		if (GuiClass.Controls)
		{
			OpenInventoryToggle = false;
		}

		Help.isPause = OpenInventoryToggle || GuiClass.Controls;
	}

	void OnGUI () {
		InitStyles();
		if(OpenInventoryToggle)
		{
			// draw the inventory menu
			// helper rect
            Rect box = GuiClass.GetCenteredRect(screenCenter, menuSize);
			// draws the main controls box
            GUI.Box(box, "Inventory", inventoryStyle);

			// Draw the internal inventory
			Vector2 internalInventorySize = new Vector2(menuSize.x - (margin*2), menuSize.y * 2f/4);
			Rect internalInventoryRect = GuiClass.GetCenteredRect(new Vector2(screenCenter.x, screenCenter.y - menuSize.y * 1/8), internalInventorySize);
            GUI.Box(internalInventoryRect, "");

			Vector2 itemsBound = new Vector2(internalInventorySize.x - (margin*2) - itemSize.x, internalInventorySize.y - (margin*2) - itemSize.y);
			Vector2 itemsTopLeft = new Vector2(internalInventoryRect.x + margin + itemSize.x/2, internalInventoryRect.y + margin + itemSize.y/2);
			int horizontalShift = (int)(itemsBound.x / (itemSize.x+margin)) + 1;
			// print(horizontalShift);
			for (int i = 0; i < inv.Count; i++)
			{
				float verticalShift = (itemSize.y+margin)*(int)((float)i/(horizontalShift));
				Vector2 itemLocation = new Vector2(itemsTopLeft.x + (i%horizontalShift)*(itemSize.x+margin), itemsTopLeft.y + verticalShift);
				if (inv[i][0] != -1)
				{
					if (GUI.Button(GuiClass.GetCenteredRect(itemLocation, itemSize), i.ToString()))
					{
						
					}
				}
				else
				{
					if (GUI.Button(GuiClass.GetCenteredRect(itemLocation, itemSize), ""))
					{
						
					}
				}
			}

			// Draw the hotbar inside the inventory menu
            GUI.Box(GuiClass.GetCenteredRect(new Vector2(screenCenter.x, screenCenter.y + menuSize.y * 5/16), new Vector2(menuSize.x - (margin*2), menuSize.y * 3f/12)), "Hotbar");
			
		}
		else if(!GuiClass.Controls)
		{
			// Draw the hotbar
			Vector2 hotbarSize = new Vector2(menuSize.x - (margin*2), menuSize.y * 1f/6);
            GUI.Box(GuiClass.GetCenteredRect(new Vector2(screenCenter.x, Screen.height - hotbarSize.y/2 - margin), hotbarSize), "");			
		}
	}
}

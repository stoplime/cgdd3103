using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public GUISkin guiSkin;

	public bool OpenInventoryToggle;

	public int HotBarSlots = 4;
	public int InternalVisibleSlots = 6;
	public int InternalMaxSlots = 12;

	/// <summary>
	/// the data structure for inventory storing the (id, count) of each item
	/// </summary>
	private List< List<int> > inv;

	private List<Item> itemSprites;

	private List<int> hotbar;

	private Vector2 screenCenter;
	private Vector2 menuSize;
	private int margin;

	private Vector2 itemSize;

	private GUIStyle inventoryStyle = null;

	private int dragItem = -1;

	private void addEmptyInventory()
	{
		inv.Add(new List<int>());
		inv[inv.Count-1].Add(-1); 	// default id of -1 for nothing
		inv[inv.Count-1].Add(0);	// default count of items as 0
		itemSprites.Add((Instantiate(Resources.Load("InventoryItems/EmptyGuiItem", typeof(GameObject))) as GameObject).GetComponent<Item>());
	}

	private void addItemSprite(int id)
	{
		Item clone = (Instantiate(Resources.Load("InventoryItems/GuiItem", typeof(GameObject))) as GameObject).GetComponent<Item>();
		clone.id = id;
		itemSprites[id] = clone;
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
				addItemSprite(ID);
				return true;
			}
		}
		// Check to expand inventory
		if (inv.Count < InternalMaxSlots)
		{
			addEmptyInventory();
			inv[inv.Count-1][0] = ID;
			inv[inv.Count-1][0] = 1;
			addItemSprite(ID);
			return true;
		}
		return false;
	}

	public void SwapItems(int item1, int item2)
	{
		List<int> temp = inv[item1];
		inv[item1] = inv[item2];
		inv[item2] = temp;

		Item tempI = itemSprites[item1];
		itemSprites[item1] = itemSprites[item2];
		itemSprites[item2] = tempI;
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
		// GUI.skin = guiSkin;
		OpenInventoryToggle = false;
		inv = new List<List<int>>();
		itemSprites = new List<Item>();
		for (int i = 0; i < InternalVisibleSlots; i++)
		{
			addEmptyInventory();
		}
		margin = 15;
		itemSize = new Vector2(64, 64);
		hotbar = new List<int>();
		for (int i = 0; i < HotBarSlots; i++)
		{
			hotbar.Add(i); // The index of an inventory slot
		}
	}
	
	// Update is called once per frame
	void Update () {
        // Update the GUI sizes if the screen were to resize
        screenCenter = new Vector2(Screen.width/2, Screen.height/2);
        menuSize = new Vector2(Screen.width * 4/5, Screen.height * 4/5);

		// closes the menu if the main menu is open
		if (GuiClass.Controls)
		{
			OpenInventoryToggle = false;
		}

		Help.isPause = OpenInventoryToggle || GuiClass.Controls;
	}

	public delegate void Func<in T>(T arg);
	private void DrawInventoryItems(Rect box, float margin, Vector2 itemSize, Func<bool> clicked)
	{

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

			int droppedItem = -1;
			Vector2 mouseDrop = Vector2.zero;
			// print(horizontalShift);
			if (!Input.GetKey(KeyCode.Mouse0))
			{
				if (dragItem != -1)
				{
					droppedItem = dragItem;
					dragItem = -1;
					mouseDrop = new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y);
				}
			}
			for (int i = 0; i < inv.Count; i++)
			{
				float verticalShift = (itemSize.y+margin)*(int)((float)i/(horizontalShift));
				Vector2 itemLocation = new Vector2(itemsTopLeft.x + (i%horizontalShift)*(itemSize.x+margin), itemsTopLeft.y + verticalShift);
				
				Rect buttonRect = GuiClass.GetCenteredRect(itemLocation, itemSize);
				// Rect itemRect = buttonRect;

				Vector2 mousePos = new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y);
				if(buttonRect.Contains(mousePos) && Input.GetKey(KeyCode.Mouse0) && dragItem == -1)
				{
					dragItem = i;
				}

				if (droppedItem != -1 && buttonRect.Contains(mouseDrop))
				{
					SwapItems(i, droppedItem);
				}

				// checks if item exists
				if (inv[i][0] != -1)
				{
					// string itemString = inv[i][0].ToString()+","+inv[i][1].ToString();
					string itemString = "";

					if(dragItem == i)
					{
						itemSprites[i].pos = mousePos;
						// buttonRect.center = mousePos;
					}
					else
					{
						itemSprites[i].pos = buttonRect.center;
					}

					// draw button that exists
					if (GUI.Button(buttonRect, itemString))
					{
						
					}
				}
				else
				{
					// draw button that doesn't exist
					if (GUI.Button(buttonRect, ""))
					{
						
					}
				}
			}
			// print(dragItem);

			// Draw the hotbar inside the inventory menu
			Vector2 hotbarSize = new Vector2(menuSize.x - (margin*2), menuSize.y * 3f/12);
			Rect hotbarRect = GuiClass.GetCenteredRect(new Vector2(screenCenter.x, screenCenter.y + menuSize.y * 5/16), hotbarSize);
            GUI.Box(hotbarRect, "Hotbar");

			itemsBound = new Vector2(hotbarSize.x - (margin*2) - itemSize.x, hotbarSize.y - (margin*2) - itemSize.y);
			itemsTopLeft = new Vector2(screenCenter.x - ((itemSize.x + margin) * HotBarSlots/2f) + (itemSize.x + margin)/2, hotbarRect.y + hotbarSize.y - margin - itemSize.y/2);
			for (int i = 0; i < hotbar.Count; i++)
			{
				float verticalShift = (itemSize.y+margin)*(int)((float)i/(horizontalShift));
				Vector2 itemLocation = new Vector2(itemsTopLeft.x + (i%horizontalShift)*(itemSize.x+margin), itemsTopLeft.y + verticalShift);
				if (inv[hotbar[i]][0] != -1)
				{
					// itemSprites[hotbar[i]].pos = itemLocation;
					if (GUI.Button(GuiClass.GetCenteredRect(itemLocation, itemSize), inv[hotbar[i]][0].ToString()+","+inv[hotbar[i]][1].ToString()))
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
		}
		else if(!GuiClass.Controls)
		{
			// Draw the hotbar
			Vector2 hotbarSize = new Vector2(menuSize.x - (margin*2), menuSize.y * 1f/6);
			Rect hotbarRect = GuiClass.GetCenteredRect(new Vector2(screenCenter.x, Screen.height - hotbarSize.y/2 - margin), hotbarSize);
            GUI.Box(hotbarRect, "");

			Vector2 itemsBound = new Vector2(hotbarSize.x - (margin*2) - itemSize.x, hotbarSize.y - (margin*2) - itemSize.y);
			Vector2 itemsTopLeft = new Vector2(screenCenter.x - ((itemSize.x + margin) * HotBarSlots/2f) + (itemSize.x + margin)/2, hotbarRect.y + hotbarSize.y - margin - itemSize.y/2);
			int horizontalShift = (int)(itemsBound.x / (itemSize.x+margin)) + 1;
			for (int i = 0; i < hotbar.Count; i++)
			{
				float verticalShift = (itemSize.y+margin)*(int)((float)i/(horizontalShift));
				Vector2 itemLocation = new Vector2(itemsTopLeft.x + (i%horizontalShift)*(itemSize.x+margin), itemsTopLeft.y + verticalShift);
				if (inv[hotbar[i]][0] != -1)
				{
					itemSprites[hotbar[i]].pos = itemLocation;
					if (GUI.Button(GuiClass.GetCenteredRect(itemLocation, itemSize), inv[hotbar[i]][0].ToString()+","+inv[hotbar[i]][1].ToString()))
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
		}
		else
		{
			for (int i = 0; i < itemSprites.Count; i++)
			{
				itemSprites[i].pos = new Vector2(-itemSprites[i].size.x, -itemSprites[i].size.y);
			}
		}
	}
}

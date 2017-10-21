using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem {

    private int id;
    public int Id{
        get{
            return id;
        }
        set{
            id = value;
        }
    }
    public Texture Sprite{
        get{
            if (id != -1)
            {
                return Help.ItemVocabulary[id];
            }
            Texture2D blank = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            blank.SetPixel(0, 0, Color.clear);
            blank.Apply();
            return blank;
        }
    }
    private int count;
    public int Count{
        get{
            return count;
        }
        set{
            count = value;
        }
    }
    public InventoryItem()
    {
        id = -1;
        count = 0;
    }
}

public class InventoryControl : MonoBehaviour {

    public bool OpenInventoryToggle = false;

    public Vector2 ItemSize = new Vector2(64, 64);

    public int VisibleInventorySize = 6;
    public int MaxInventorySize = 12;

    public int HotbarSize = 4;

    public int MaxStack = 64;

    public int Margin = 15;

    public Vector2 ItemTextSize = new Vector2(20, 20);

    public bool InventoryFullFlag = false;

    public int SelectedHotbar;

    private List<Rect> GuiBoxes;
    private List<Rect> ItemBoxes;
    private List<Rect> HotbarInBoxes;
    private List<Rect> HotbarOutBoxes;

    private List<InventoryItem> inventory;

    private List<int> hotbar;

    private List<Vector2> ItemSpritePos;
    private List<Vector2> HotbarSpritePos;

    private bool needGuiUpdate;
    private bool needSpriteUpdate;

    private int dragId;
    private int dropId;

    private Resolution res;

    private bool preMenuOpen;

    private float InventoryFullTimer = 2;

    public void InitInventory(){
        inventory = new List<InventoryItem> ();
        hotbar = new List<int>();

        for (int i = 0; i < VisibleInventorySize; i++)
        {
            inventory.Add(new InventoryItem());
        }
        for (int i = 0; i < HotbarSize; i++)
        {
            hotbar.Add(i);
        }
    }

    public void InitItemVocabulary() {
        Help.ItemVocabulary = new List<Texture>();
        Help.ItemVocabulary.Add(Resources.Load("ItemTextures/item_cs")               as Texture);
        Help.ItemVocabulary.Add(Resources.Load("ItemTextures/item_emergency_kit_01") as Texture);
        Help.ItemVocabulary.Add(Resources.Load("ItemTextures/item_emergency_kit_02") as Texture);
        Help.ItemVocabulary.Add(Resources.Load("ItemTextures/item_epi_pen")          as Texture);
        Help.ItemVocabulary.Add(Resources.Load("ItemTextures/item_flash_light_01")   as Texture);
        Help.ItemVocabulary.Add(Resources.Load("ItemTextures/item_flash_light_02")   as Texture);
        Help.ItemVocabulary.Add(Resources.Load("ItemTextures/item_flashbang")        as Texture);
        Help.ItemVocabulary.Add(Resources.Load("ItemTextures/item_m16_frag")         as Texture);
        Help.ItemVocabulary.Add(Resources.Load("ItemTextures/item_m18_smoke")        as Texture);
    }

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        needGuiUpdate = true;
    }

    void Start() {
        InitItemVocabulary();
        InitInventory();
        needGuiUpdate = true;
        needSpriteUpdate = true;

        dragId = -1;
        dropId = -1;

        res = Screen.currentResolution;
        preMenuOpen = OpenInventoryToggle;

        SelectedHotbar = 0;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Check menu open
        if (res.width != Screen.currentResolution.width || res.height != Screen.currentResolution.height) {
            needGuiUpdate = true;
            needSpriteUpdate = true;
            res = Screen.currentResolution;
        }

        // closes the menu if the main menu is open
		if (GuiClass.Controls)
		{
			OpenInventoryToggle = false;
		}

		Help.isPause = OpenInventoryToggle || GuiClass.Controls;

        Vector2 mousePos = new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y);

        // calculate drag
        if(OpenInventoryToggle)
		{
            if (Input.GetKey(KeyCode.Mouse0))
            {
                needSpriteUpdate = true;
            }

            // check for start of drag
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                needSpriteUpdate = true;
                dragId = -1;
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (GetItemBoxes(false)[i].Contains(mousePos))
                    {
                        dragId = i;
                        break;
                    }
                }
                // if (dragId == -1)
                // {
                    for (int i = 0; i < hotbar.Count; i++)
                    {
                        if (GetHotbarInBoxes(false)[i].Contains(mousePos))
                        {
                            dragId = i + inventory.Count;
                            break;
                        }
                    }
                // }
            }

            // check for end of drag
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                needSpriteUpdate = true;
                dropId = -1;
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (GetItemBoxes(false)[i].Contains(mousePos))
                    {
                        dropId = i;
                        break;
                    }
                }
                if (dropId == -1)
                {
                    for (int i = 0; i < hotbar.Count; i++)
                    {
                        if (GetHotbarInBoxes(false)[i].Contains(mousePos))
                        {
                            dropId = i + inventory.Count;
                            break;
                        }
                    }
                }
                // Apply the drag & drop
                if (dragId != -1 && dropId != -1)
                {
                    needSpriteUpdate = true;

                    bool dragHotbar = false;
                    if (dragId >= inventory.Count)
                    {
                        dragHotbar = true;
                        dragId -= inventory.Count;
                    }

                    bool dropHotbar = false;
                    if (dropId >= inventory.Count)
                    {
                        dropHotbar = true;
                        dropId -= inventory.Count;
                    }

                    // both in inventory
                    if (!dragHotbar && !dropHotbar)
                    {
                        SwapItems(dragId, dropId);
                    }
                    // drag from inventory to hotbar
                    else if (!dragHotbar && dropHotbar)
                    {
                        hotbar[dropId] = dragId;
                    }
                    // swap hotbar items
                    else if (dragHotbar && dropHotbar)
                    {
                        int temp = hotbar[dragId];
                        hotbar[dragId] = hotbar[dropId];
                        hotbar[dropId] = temp;
                    }
                    // drag hotbar to inventory should be like an item Swap
                    else if (dragHotbar && !dropHotbar)
                    {
                        SwapItems(hotbar[dragId], dropId);
                        hotbar[dragId] = dropId;
                    }

                    dragId = -1;
                    dropId = -1;
                }
                else if (dragId != -1 && dropId == -1)
                {
                    dragId = -1;
                }
            }
        }

        // checks if menu had just been opened
        if (preMenuOpen != OpenInventoryToggle)
        {
            needGuiUpdate = true;
            needSpriteUpdate = true;
            preMenuOpen = OpenInventoryToggle;
        }

        // Checks if any Lists need updating
        CheckUpdate();

        if (dragId >= 0)
        {
            bool dragHotbar = false;
            if (dragId >= inventory.Count)
            {
                dragHotbar = true;
                // dragId -= inventory.Count;
            }
            if (dragHotbar)
            {
                HotbarSpritePos[dragId-inventory.Count] = mousePos;
            }
            else
            {
                ItemSpritePos[dragId] = mousePos;
            }
        }
    }

    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// This function can be called multiple times per frame (one call per event).
    /// </summary>
    void OnGUI()
    {
        InventoryFullNotification();
        if(OpenInventoryToggle)
		{
            // Overall inventory menu box
            GUI.Box(GetGuiBoxes(false)[0], "Inventory");

            // Inside inventory box
            GUI.Box(GetGuiBoxes(false)[1], "");

            // Internal Hotbar box
            GUI.Box(GetGuiBoxes(false)[2], "Hotbar");

            // Inventory slots
            for (int i = 0; i < GetItemBoxes(false).Count; i++)
            {
                if (GUI.Button(GetItemBoxes(false)[i], ""))
                {
                    
                }
                // Draw item icons
                GUI.depth = -1;
                if (i == dragId) {
                    GUI.depth = -2;
                }
                GUI.DrawTexture(GuiClass.GetCenteredRect(GetItemSpritePos(false)[i], ItemSize), inventory[i].Sprite, ScaleMode.StretchToFill, true, 10.0F, Color.white, 0, 1);
                GUI.depth--;
                GUI.Label(GetItemTextRect(GetItemBoxes(false)[i]), inventory[i].Count.ToString());
                GUI.depth = 0;
            }

            // Internal Hotbar slots
            for (int i = 0; i < GetHotbarInBoxes(false).Count; i++)
            {
                if (GUI.Button(GetHotbarInBoxes(false)[i], ""))
                {
                    
                }
                // Draw item icons
                GUI.depth = -1;
                if (i == dragId - inventory.Count) {
                    GUI.depth = -2;
                }
                GUI.DrawTexture(GuiClass.GetCenteredRect(GetHotbarSpritePos(false)[i], ItemSize), inventory[hotbar[i]].Sprite, ScaleMode.StretchToFill, true, 10.0F, Color.white, 0, 1);
                GUI.depth--;
                GUI.Label(GetItemTextRect(GetHotbarInBoxes(false)[i]), inventory[hotbar[i]].Count.ToString());
                GUI.depth = 0;
            }
        }
        else if (!GuiClass.Controls)
        {
            // External Hotbar box
            GUI.Box(GetGuiBoxes(false)[3], "");

            // Internal Hotbar slots
            for (int i = 0; i < GetHotbarOutBoxes(false).Count; i++)
            {
                if (SelectedHotbar == i)
                {
                    // draw highlight
                    Rect largerRect = GetHotbarOutBoxes(false)[i];
                    largerRect.size += new Vector2(Margin, Margin);
                    largerRect.x -= Margin/2f;
                    largerRect.y -= Margin/2f;
                    GUI.Box(largerRect, "");
                }
                if (GUI.Button(GetHotbarOutBoxes(false)[i], ""))
                {
                    
                }
                // Draw item icons
                GUI.depth = -1;
                GUI.DrawTexture(GuiClass.GetCenteredRect(GetHotbarSpritePos(false)[i], ItemSize), inventory[hotbar[i]].Sprite, ScaleMode.StretchToFill, true, 10.0F, Color.white, 0, 1);
                GUI.depth--;
                GUI.Label(GetItemTextRect(GetHotbarOutBoxes(false)[i]), inventory[hotbar[i]].Count.ToString());
                GUI.depth = 0;
            }
        }
    }

    public void CheckPause() {
        Help.isPause = OpenInventoryToggle || GuiClass.Controls;
    }

    public void CheckUpdate() {
        if (needGuiUpdate)
        {
            needGuiUpdate = false;
            GetGuiBoxes(true);
            GetItemBoxes(true);
            GetHotbarInBoxes(true);
            GetHotbarOutBoxes(true);
        }
        if (needSpriteUpdate)
        {
            needSpriteUpdate = false;
            GetItemSpritePos(true);
            GetHotbarSpritePos(true);
        }
    }

    public void InventoryFullNotification() {
        if (InventoryFullFlag)
        {
            InventoryFullTimer -= Time.deltaTime;
            GUIStyle fontStyle = new GUIStyle();
            fontStyle.fontSize = 24;
            // Color tempColor = GUI.color;
            Color textColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            textColor.a = Mathf.Lerp(0f, 1f, InventoryFullTimer/2);
            fontStyle.normal.textColor = textColor;
            // GUI.color = textColor;
            Vector2 screenCenter = new Vector2(Screen.width/2, Screen.height/2);
            Vector2 notificationSize = new Vector2(Screen.width * 1/4, Screen.height * 1/6);
            GUI.Label(GuiClass.GetCenteredRect(screenCenter, notificationSize), "Inventory Is Full!", fontStyle);
            // GUI.color = tempColor;
        }
        if (InventoryFullTimer <= 0)
        {
            InventoryFullFlag = false;
            InventoryFullTimer = 2;
        }
    }

    public int getHotbarItemID(int index) {
		return getInvItemID(hotbar[index]);
	}
    
    public int getInvItemID(int index) {
		if(index < inventory.Count && index >= 0)
		{
			if(inventory[index].Count > 0){
				return inventory[index].Id;
			}
		}
		return -1;
	}

    public int removeOneHotbarItem(int index) {
		return removeOneInvItem(hotbar[index]);
	}

	public int removeOneInvItem(int index) {
		if(index < inventory.Count && index >= 0)
		{
			if(inventory[index].Count > 0){
				int tempID = inventory[index].Id;
				if (inventory[index].Count > 1)
				{
					inventory[index].Count = inventory[index].Count -1;
				}
				else
				{
                    if (inventory.Count > VisibleInventorySize)
                    {
                        inventory.RemoveAt(index);
                        for (int i = 0; i < hotbar.Count; i++)
                        {
                            if (hotbar[i] == index)
                            {
                                hotbar[i] = i;
                            }
                        }
                    }
                    else
                    {
                        inventory[index].Id = -1;
                        inventory[index].Count = 0;
                    }
				}
				return tempID;
			}
		}
		return -1;
	}

	public bool addOneInvItem(int ID) {
        return addInvItem(ID, 1);
	}

    public bool addInvItem(int ID, int amount) {
        int prevAmount = amount;
        while (amount > 0)
        {
            prevAmount = amount;
            // Check for existing inventory entry
            for (int i = 0; i < inventory.Count; i++)
            {
                if(inventory[i].Id == ID){
                    if (inventory[i].Count > 0 && inventory[i].Count < MaxStack)
                    {
                        int difference = MaxStack - inventory[i].Count;
                        if (difference >= amount)
                        {
                            inventory[i].Count += amount;
                            amount = 0;
                        }
                        else
                        {
                            inventory[i].Count = MaxStack;
                            amount -= difference;
                        }
                        break;
                    }
                }
            }
            if (prevAmount == amount)
            {
                break;
            }
        }
        while (amount > 0)
        {
            prevAmount = amount;
            // Check for empty slots in inventory
            for (int i = 0; i < inventory.Count; i++)
            {
                if(inventory[i].Id == -1){
                    inventory[i].Id = ID;
                    if (amount <= MaxStack)
                    {
                        inventory[i].Count = amount;
                        amount = 0;
                    }
                    else
                    {
                        inventory[i].Count = MaxStack;
                        amount -= MaxStack;
                    }
                    break;
                }
            }
            if (prevAmount == amount)
            {
                break;
            }
        }
        while (amount > 0)
        {
            prevAmount = amount;
            // Check to expand inventory
            if (inventory.Count < MaxInventorySize)
            {
                inventory.Add(new InventoryItem());
                inventory[inventory.Count-1].Id = ID;
                if (amount <= MaxStack)
                {
                    inventory[inventory.Count-1].Count = amount;
                    amount = 0;
                }
                else
                {
                    inventory[inventory.Count-1].Count = MaxStack;
                    amount -= MaxStack;
                }
            }
            if (prevAmount == amount)
            {
                break;
            }
        }
        if (amount == 0)
        {
            return true;
        }
		return false;
    }

	public void SwapItems(int item1, int item2) {
		InventoryItem temp = inventory[item1];
		inventory[item1] = inventory[item2];
		inventory[item2] = temp;
	}

    /// <summary>
    /// Get the Rect for the inventory menu
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public List<Rect> GetGuiBoxes(bool update) {
        if (GuiBoxes != null && !update)
        {
            return GuiBoxes;
        }
        GuiBoxes = new List<Rect>();
        
        // draws the main controls box
        Vector2 screenCenter = new Vector2(Screen.width/2, Screen.height/2);
        Vector2 menuSize = new Vector2(Screen.width * 4/5, Screen.height * 4/5);
        // GuiBoxes[0]
        GuiBoxes.Add(GuiClass.GetCenteredRect(screenCenter, menuSize));
        
        // Draw the internal inventory
        Vector2 internalInventorySize = new Vector2(menuSize.x - (Margin*2), menuSize.y * 2f/4);
        // GuiBoxes[1]
        GuiBoxes.Add(GuiClass.GetCenteredRect(new Vector2(screenCenter.x, screenCenter.y - menuSize.y * 1/8), internalInventorySize));
        
        // Draw the hotbar inside the inventory menu
        Vector2 hotbarMenuSize = new Vector2(menuSize.x - (Margin*2), menuSize.y * 3f/12);
        // GuiBoxes[2]
        GuiBoxes.Add(GuiClass.GetCenteredRect(new Vector2(screenCenter.x, screenCenter.y + menuSize.y * 5/16), hotbarMenuSize));
        
        // Draw the hotbar outside of the inventory
        Vector2 hotbarOutSize = new Vector2(menuSize.x - (Margin*2), menuSize.y * 1f/6);
        // GuiBoxes[3]
        GuiBoxes.Add(GuiClass.GetCenteredRect(new Vector2(screenCenter.x, Screen.height - hotbarOutSize.y/2 - Margin), hotbarOutSize));
        return GuiBoxes;
    }

    /// <summary>
    /// Get the Rects for the inventory item slots
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public List<Rect> GetItemBoxes(bool update) {
        if (ItemBoxes != null && !update)
        {
            return ItemBoxes;
        }
        if (GuiBoxes == null)
        {
            GetGuiBoxes(true);
        }
        ItemBoxes = new List<Rect>();

        // Calculate the location of the items
        Vector2 itemsBound = new Vector2(GuiBoxes[1].size.x - (Margin*2) - ItemSize.x, GuiBoxes[1].size.y - (Margin*2) - ItemSize.y);
        Vector2 itemsTopLeft = new Vector2(GuiBoxes[1].x + Margin + ItemSize.x/2, GuiBoxes[1].y + Margin + ItemSize.y/2);
        int horizontalShift = (int)(itemsBound.x / (ItemSize.x+Margin)) + 1;
        for (int i = 0; i < inventory.Count; i++)
        {
            float verticalShift = (ItemSize.y+Margin)*(int)((float)i/(horizontalShift));
            Vector2 itemLocation = new Vector2(itemsTopLeft.x + (i%horizontalShift)*(ItemSize.x+Margin), itemsTopLeft.y + verticalShift);
            
            ItemBoxes.Add(GuiClass.GetCenteredRect(itemLocation, ItemSize));
        }
        return ItemBoxes;
    }

    /// <summary>
    /// Get the Rects for the Hotbar items when the inventory menu is open
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public List<Rect> GetHotbarInBoxes(bool update) {
        if (HotbarInBoxes != null && !update)
        {
            return HotbarInBoxes;
        }
        if (GuiBoxes == null)
        {
            GetGuiBoxes(true);
        }
        HotbarInBoxes = new List<Rect>();

        Vector2 itemsBound = new Vector2(GuiBoxes[2].size.x - (Margin*2) - ItemSize.x, GuiBoxes[2].size.y - (Margin*2) - ItemSize.y);
        Vector2 itemsTopLeft = new Vector2(GuiBoxes[2].center.x - ((ItemSize.x + Margin) * HotbarSize/2f) + (ItemSize.x + Margin)/2, GuiBoxes[2].y + GuiBoxes[2].size.y - Margin - ItemSize.y/2);
        int horizontalShift = (int)(itemsBound.x / (ItemSize.x+Margin)) + 1;
        for (int i = 0; i < hotbar.Count; i++)
        {
            float verticalShift = (ItemSize.y+Margin)*(int)((float)i/(horizontalShift));
            Vector2 itemLocation = new Vector2(itemsTopLeft.x + (i%horizontalShift)*(ItemSize.x+Margin), itemsTopLeft.y + verticalShift);
            HotbarInBoxes.Add(GuiClass.GetCenteredRect(itemLocation, ItemSize));
        }
        return HotbarInBoxes;
    }

    /// <summary>
    /// Get the Rects for the Hotbar items when the inventory menu is closed
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public List<Rect> GetHotbarOutBoxes(bool update) {
        if (HotbarOutBoxes != null && !update)
        {
            return HotbarOutBoxes;
        }
        if (GuiBoxes == null)
        {
            GetGuiBoxes(true);
        }
        HotbarOutBoxes = new List<Rect>();

        Vector2 itemsBound = new Vector2(GuiBoxes[3].size.x - (Margin*2) - ItemSize.x, GuiBoxes[3].size.y - (Margin*2) - ItemSize.y);
        Vector2 itemsTopLeft = new Vector2(GuiBoxes[3].center.x - ((ItemSize.x + Margin) * HotbarSize/2f) + (ItemSize.x + Margin)/2, GuiBoxes[3].y + GuiBoxes[3].size.y - Margin - ItemSize.y/2);
        int horizontalShift = (int)(itemsBound.x / (ItemSize.x+Margin)) + 1;
        for (int i = 0; i < hotbar.Count; i++)
        {
            float verticalShift = (ItemSize.y+Margin)*(int)((float)i/(horizontalShift));
            Vector2 itemLocation = new Vector2(itemsTopLeft.x + (i%horizontalShift)*(ItemSize.x+Margin), itemsTopLeft.y + verticalShift);
            HotbarOutBoxes.Add(GuiClass.GetCenteredRect(itemLocation, ItemSize));
        }
        return HotbarOutBoxes;
    }

    /// <summary>
    /// Gets the location of the item sprites
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public List<Vector2> GetItemSpritePos(bool update) {
        if (ItemSpritePos != null && !update)
        {
            return ItemSpritePos;
        }
        if (ItemBoxes == null)
        {
            GetItemBoxes(true);
        }
        ItemSpritePos = new List<Vector2>();
        
        // update the default sprite positions
        // overide these values for drag function
        for (int i = 0; i < inventory.Count; i++)
        {
            ItemSpritePos.Add(GetItemBoxes(false)[i].center);
        }
        return ItemSpritePos;
    }

    /// <summary>
    /// Gets the location of the hotbar sprites
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public List<Vector2> GetHotbarSpritePos(bool update) {
        if (HotbarSpritePos != null && !update)
        {
            return HotbarSpritePos;
        }
        if (HotbarInBoxes == null)
        {
            GetHotbarInBoxes(true);
        }
        if (HotbarOutBoxes == null)
        {
            GetHotbarOutBoxes(true);
        }
        HotbarSpritePos = new List<Vector2>();

        if (OpenInventoryToggle)
        {
            for (int i = 0; i < hotbar.Count; i++)
            {
                HotbarSpritePos.Add(GetHotbarInBoxes(false)[i].center);
            }
            return HotbarSpritePos;
        }
        else
        {
            for (int i = 0; i < hotbar.Count; i++)
            {
                HotbarSpritePos.Add(GetHotbarOutBoxes(false)[i].center);
            }
            return HotbarSpritePos;
        }
    }

    private Rect GetItemTextRect(Rect item) {
        Vector2 textPos = new Vector2(item.xMax - ItemTextSize.x, item.yMax - ItemTextSize.y);
        return new Rect(textPos, ItemTextSize);
    }
}

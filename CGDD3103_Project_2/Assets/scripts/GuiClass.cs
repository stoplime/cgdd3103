using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiClass : MonoBehaviour {

    /// <summary>
    /// Toggle displaying the controls menu
    /// </summary>
    /// <returns></returns>
    public bool Controls{ get; set;}

    /// <summary>
    /// size of the controls menu
    /// </summary>
    private Vector2 controlsMenuSize;

    /// <summary>
    /// location of the center of the screen
    /// </summary>
    private Vector2 screenCenter;

    /// <summary>
    /// returns a Rect based off the center and size
    /// </summary>
    /// <param name="center">center point of the rect</param>
    /// <param name="size">size of the rect</param>
    /// <returns>a Rect</returns>
    public static Rect GetCenteredRect(Vector2 center, Vector2 size)
    {
        return new Rect(center.x - size.x/2, center.y - size.y/2, size.x, size.y);
    }

    /// <summary>
    /// accessing the main character variables
    /// </summary>
    private CharacterMovement mainCharacterScript;

    /// <summary>
    /// list of available controls for keys
    /// </summary>
    private List<string> keyTable;

    /// <summary>
    /// list of keys to be displayed in the controls menu
    /// </summary>
    private List< List<string> > keyDisplays;

    /// <summary>
    /// flag for when a key change is required, {-1, -1} is the default false state.
    /// </summary>
    private int[] keyChangeFlag;

    void Start()
    {
        keyChangeFlag = new int[2] {-1, -1};
        Controls = false;
        screenCenter = new Vector2(Screen.width/2, Screen.height/2);
        controlsMenuSize = new Vector2(Screen.width * 4/5, Screen.height * 4/5);
        
        // The list of keys for the different types of controls
        keyTable = new List<string>();
        keyTable.Add("forward");
        keyTable.Add("backward");
        keyTable.Add("left");
        keyTable.Add("right");
        // keyTable.Add("turn");
        keyTable.Add("fire");

        // creates a list of key variables being used as controls
        // index setup as [profile][key]
        keyDisplays = new List< List<string> >();
        List<string> dispProflie1 = new List<string>();
        List<string> dispProflie2 = new List<string>();
        for (int i = 0; i < keyTable.Count; i++)
        {
            dispProflie1.Add(GameManager.DefaultKeyConfig1[keyTable[i]].ToString());
            dispProflie2.Add(GameManager.DefaultKeyConfig2[keyTable[i]].ToString());
        }
        keyDisplays.Add(dispProflie1);
        keyDisplays.Add(dispProflie2);
    }

    void Update()
    {
        // Update the GUI sizes if the screen were to resize
        screenCenter = new Vector2(Screen.width/2, Screen.height/2);
        controlsMenuSize = new Vector2(Screen.width * 4/5, Screen.height * 4/5);

        // toggle the controls menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Controls = !Controls;
        }

        // accessing variables from the main character
        mainCharacterScript = gameObject.GetComponentInParent<CharacterMovement>();
        Help.isPause = Controls;

        // flag checking to see if a change of key is required.
        if (keyChangeFlag[0] != -1 && keyChangeFlag[1] != -1)
        {
            KeyCode keyPressed = GetKeyCode();
            if (keyPressed != KeyCode.None)
            {
				// print(keyPressed.ToString());
				mainCharacterScript.setKeyProfile(keyChangeFlag[0], keyTable[keyChangeFlag[1]], keyPressed);
				keyDisplays[keyChangeFlag[0]][keyChangeFlag[1]] = keyPressed.ToString();
				keyChangeFlag = new int[2] {-1, -1};
            }
        }
    }

    /// <summary>
    /// Finds the key being pressed
    /// </summary>
    /// <returns>KeyCode object of pressed key, KeyCode.None if not pressed.</returns>
    public KeyCode GetKeyCode()
    {
        foreach (KeyCode testKey in System.Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown(testKey))
			{
				return testKey;
			}
		}
        return KeyCode.None;
    }

	void OnGUI () {
        
        // Top left Health and menu
        GUI.Box(new Rect(10,10,100,90), "Health");
        GUI.Label(new Rect(20,40,80,20), "HP: " + mainCharacterScript.health.ToString() + "/" + mainCharacterScript.maxHealth.ToString());

        // toggle controls menu
        if(GUI.Button(new Rect(20,70,80,20), "Controls")) {
            Controls = !Controls;
        }

        if (Controls)
        {
			// helper rect
            Rect controlsBox = GetCenteredRect(screenCenter, controlsMenuSize);
			// draws the main controls box
            GUI.Box(controlsBox, "Controls");

			// draws selected profile box
			if (mainCharacterScript.Profile == 0)
			{
				GUI.Box(GetCenteredRect(new Vector2(controlsBox.x + controlsMenuSize.x/3, screenCenter.y), new Vector2(controlsMenuSize.x/4, controlsMenuSize.y*7/8)), "Selected Profile");
			}
			else if (mainCharacterScript.Profile == 1)
			{
				GUI.Box(GetCenteredRect(new Vector2(controlsBox.x + controlsMenuSize.x*2/3, screenCenter.y), new Vector2(controlsMenuSize.x/4, controlsMenuSize.y*7/8)), "Selected Profile");
			}

			// profile buttons
            if (GUI.Button(GetCenteredRect(new Vector2(controlsBox.x + controlsMenuSize.x/3, controlsBox.y + controlsMenuSize.y/6), new Vector2(80, 20)), "Profile1"))
            {
                mainCharacterScript.Profile = 0;
            }
            if (GUI.Button(GetCenteredRect(new Vector2(controlsBox.x + controlsMenuSize.x*2/3, controlsBox.y + controlsMenuSize.y/6), new Vector2(80, 20)), "Profile2"))
            {
                mainCharacterScript.Profile = 1;               
            }

			// creates the labels for each type of key
            for (int i = 0; i < keyTable.Count; i++)
            {
                GUI.Label(GetCenteredRect(new Vector2(controlsBox.x + controlsMenuSize.x/6, controlsBox.y + controlsMenuSize.y/6 + (i+1)*40), new Vector2(80, 20)), keyTable[i]);
            }
			
			// draws the buttons for each key for two profiles
            for (int i = 0; i < keyDisplays.Count; i++) // i == 0 is profile1; i == 1 is profile2
            {
                for (int j = 0; j < keyDisplays[i].Count; j++)
                {
                    if (GUI.Button(GetCenteredRect(new Vector2(controlsBox.x + controlsMenuSize.x * (i+1)/3, controlsBox.y + controlsMenuSize.y/6 + 40*(j+1)), new Vector2(80, 20)), keyDisplays[i][j]))
                    {
                        keyDisplays[i][j] = " ";
                        keyChangeFlag = new int[2] {i, j};
                    }
                }
            }
            
        }
    }
}

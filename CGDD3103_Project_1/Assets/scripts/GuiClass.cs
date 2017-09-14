using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiClass : MonoBehaviour {

    public bool Controls{ get; set;}
    private Vector2 controlsMenuSize;

    private Vector2 screenCenter;

    public static Rect GetCenteredRect(Vector2 center, Vector2 size)
    {
        return new Rect(center.x - size.x/2, center.y - size.y/2, size.x, size.y);
    }

    // private GameManager gmScript;

    private CharacterMovement mainCharacterScript;

    private List<string> keyTable;

    private List< List<string> > keyDisplays;

    private int[] keyChangeFlag;

    private string keyChangeKey;

    private KeyCode changedKeyCode;

    void Start()
    {
        changedKeyCode = KeyCode.None;
        keyChangeFlag = new int[2] {-1, -1};
        keyChangeKey = " ";
        Controls = false;
        screenCenter = new Vector2(Screen.width/2, Screen.height/2);
        controlsMenuSize = new Vector2(Screen.width * 4/5, Screen.height * 4/5);

        // gmScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        keyTable = new List<string>();
        keyTable.Add("forward");
        keyTable.Add("backward");
        keyTable.Add("left");
        keyTable.Add("right");
        // keyTable.Add("turn");
        keyTable.Add("fire");

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

        mainCharacterScript = gameObject.GetComponentInParent<CharacterMovement>();
        mainCharacterScript.MovementLock = Controls;

        if (keyChangeFlag[0] != -1 && keyChangeFlag[1] != -1)
        {
            KeyCode keyPressed = GetKeyCode(Event.current);
            if (keyPressed != KeyCode.None)
            {
                changedKeyCode = keyPressed;
            }
        }
    }

    public KeyCode GetKeyCode(Event e)
    {
        if (e == null)
        {
            return KeyCode.None;
        }
        if (e.isKey && e.keyCode != KeyCode.None) { return e.keyCode; }
        if (e.isMouse) { return KeyCode.Mouse0 + e.button; }
        if (e.shift)
        {
            if (Input.GetKey(KeyCode.RightShift)) { return KeyCode.RightShift; }
            else { return KeyCode.LeftShift; }
        }
        if (e.alt)
        {
            if (Input.GetKey(KeyCode.RightAlt)) { return KeyCode.RightAlt; }
            else if (Input.GetKey(KeyCode.LeftAlt)) { return KeyCode.LeftAlt; }
            else { return KeyCode.AltGr; }
        }
        if (e.control)
        {
            if (Input.GetKey(KeyCode.RightControl)) { return KeyCode.RightControl; }
            else { return KeyCode.LeftControl; }
        }
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (j > 0)
                {
                    if (Input.GetKey("joystick " + j + " button " + i))
                    { return KeyCode.Joystick1Button0 + i + (j * 20); }
                }
                else
                {
                    if (Input.GetKey("joystick button " + i))
                    { return KeyCode.Joystick1Button0 + i + (j * 20); }
                }
            }
        }
        return KeyCode.None;
    }

	void OnGUI () {
        
        GUI.Box(new Rect(10,10,100,90), "Health");
        GUI.Label(new Rect(20,40,80,20), "HP: " + mainCharacterScript.health.ToString() + "/" + mainCharacterScript.maxHealth.ToString());

        // toggle controls menu
        if(GUI.Button(new Rect(20,70,80,20), "Controls")) {
            Controls = !Controls;
        }

        if (Controls)
        {
            Rect controlsBox = GetCenteredRect(screenCenter, controlsMenuSize);
            GUI.Box(controlsBox, "Controls");
            if (GUI.Button(GetCenteredRect(new Vector2(controlsBox.x + controlsMenuSize.x/3, controlsBox.y + controlsMenuSize.y/6), new Vector2(80, 20)), "Profile1"))
            {
                mainCharacterScript.Profile = 0;
            }
            if (GUI.Button(GetCenteredRect(new Vector2(controlsBox.x + controlsMenuSize.x*2/3, controlsBox.y + controlsMenuSize.y/6), new Vector2(80, 20)), "Profile2"))
            {
                mainCharacterScript.Profile = 1;               
            }
            for (int i = 0; i < keyTable.Count; i++)
            {
                GUI.Label(GetCenteredRect(new Vector2(controlsBox.x + controlsMenuSize.x/6, controlsBox.y + controlsMenuSize.y/6 + (i+1)*40), new Vector2(80, 20)), keyTable[i]);
            }

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

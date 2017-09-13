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

    private CharacterMovement mainCharacterScript;

    void Start()
    {
        Controls = false;
        screenCenter = new Vector2(Screen.width/2, Screen.height/2);
        controlsMenuSize = new Vector2(Screen.width * 4/5, Screen.height * 4/5);
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
        }
    }
}

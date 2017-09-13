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

    private GameObject gameManager;
    private GameObject mainCharacter;

    private CharacterMovement mainCharacterScript;

    void Start()
    {
        Controls = false;
        screenCenter = new Vector2(Screen.width/2, Screen.height/2);
        controlsMenuSize = new Vector2(Screen.width * 4/5, Screen.height * 4/5);
        gameManager = GameObject.Find("GameManager");
        GameManager g = gameManager.GetComponent(typeof(GameManager)) as GameManager;
        mainCharacter = g.mainCharacter;
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

        mainCharacterScript = mainCharacter.GetComponent(typeof(CharacterMovement)) as CharacterMovement;
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
            GUI.Box(GetCenteredRect(screenCenter, controlsMenuSize), "Controls");
            // GUI.Label();
        }
    }
}

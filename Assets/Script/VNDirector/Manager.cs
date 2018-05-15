using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class Manager : MonoBehaviour {

    public View view;
    public AudioSource musicPlayer;

    const int ScreenWidth = 1920;
    const int ScreenHeight = 1080;

    public XmlDocument vnScript = new XmlDocument();
    public string route = "main";
    public int step = 1;
    
    public static JSEngine jsEngine = new JSEngine();

    private HoldState hold = HoldState.Clear; // when held, the VN waits for user click to go next.

    void Start () {
        vnScript.LoadXml(Resources.Load<TextAsset>("VNScripts/part2").text); // TODO: Add options for multiple scripts, FIX HARDCODING
	}
	
	void Update () {
        ResourceController.Update();
        if (Input.GetKeyDown("space") || Input.GetKeyDown("return")) {
            NextStep();  
        };
        ProcessLoop();

	}

    private void ProcessLoop() {
        while (true) {
            // If held, break as not to cause infinite loop :3
            if (hold == HoldState.Held) { break; }

            // Using XPath, finds current label, finds the step (any type) with the current step no (Note: XPath order selector starts at 1, not 0)
            var command = vnScript.SelectSingleNode($"/Script/Label[@name='{route}']/*[{step}]");

            switch (command.Name) { // wew thats a large switch
                case "Text": // A plain VN Dialogue, nothing hard
                    view.SetDialogue(command.InnerText);
                    break;
                case "Element-Create":
                    XMLElementAction.Create(command);
                    break;
                case "Element-Destroy":
                    Element.Destroy(command.Attributes["name"].Value);
                    break;
                case "Music-Play": // TODO: Not sure what this does, replace!
                    var clip = ResourceController.Get<AudioClip>(command.InnerText);
                    musicPlayer.PlayOneShot(clip);
                    break;
                case "Load":
                    if (command.Attributes?["type"]?.Value == "sprite") {
                        ResourceController.Load<Sprite>(command.InnerText);
                    } else {
                        ResourceController.Load(command.InnerText);
                    }
                    break;
                case "Unload":
                    ResourceController.Unload(command.InnerText);
                    break;
                case "Jump":
                    route = command.Attributes["label"].Value;
                    step = 0; // somewhat hacky way to handle the increment at the end, will fix if this errs or something
                    break;
                case "JS": // JS Exec
                    jsEngine.Execute(command.InnerText);
                    break;
                default:
                    throw new Exception($"{command.Name} is not a valid tag");
            }
            hold = Hold.GetHoldState(command);
            if (hold == HoldState.Clear) step++; // Only progress to next step when HoldState is clear.
        }
    }

    void NextStep() {
        if (view.isTextScrolling) {
            view.EndScroll();
        } else if (hold == HoldState.Held) {
            step++;
            hold = HoldState.Clear;
        }
    }

    public static Vector3 ScreenToWorld(float x, float y, float z = 0) {
        float bx = x / ScreenWidth * Screen.width;
        float by = Screen.height - y / ScreenHeight * Screen.height;

        var worldVector = Camera.main.ScreenToWorldPoint(new Vector3(bx, by, 0f));
        return new Vector3(worldVector.x, worldVector.y, z);
    }
}

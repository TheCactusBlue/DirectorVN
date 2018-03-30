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

    public XmlDocument vnScript;
    public string route = "main";
    public int step = 1;
    
    public static JSEngine jsEngine = new JSEngine();

    private HoldState hold = HoldState.Clear; // when held, the VN waits for user click to go next.

    void Start () {
        vnScript = new XmlDocument();
        vnScript.LoadXml(Resources.Load<TextAsset>("VNScripts/part2").text);
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
            if (hold == HoldState.Held) { break; }

            var command = vnScript.SelectSingleNode($"/Script/Label[@name='{route}']/*[{step}]"); // Using XPath, Array starts at 1 b*tches
            switch (command.Name) { // wew thats a large switch
                case "Text":
                    view.SetDialogue(command.InnerText);
                    break;
                case "Element-Create":
                    Element.CreateElementFromXML(command);
                    break;
                case "Element-Destroy":
                    Element.DestroyElement(command.Attributes["name"].Value);
                    break;
                case "Music-Play":
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
                    step = 0; // Not a bug, just a hacky way to handle the increment at the end
                    break;
                case "JS":
                    jsEngine.engine.Execute(command.InnerText);
                    break;
                default:
                    break;
            }
            hold = Hold.GetHoldState(command);
            if (hold == HoldState.Clear) step++;
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

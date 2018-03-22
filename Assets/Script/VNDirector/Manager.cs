using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class Manager : MonoBehaviour {

    public bool isDebug = true;

    const int ScreenWidth = 1920;
    const int ScreenHeight = 1080;

    private JObject currentScript;
    public string route = "root";
    public int step = 0;

    public Text nameBox;
    public Text dialogueBox;
    
    private JSEngine jsEngine = new JSEngine();

    private HoldState hold = HoldState.Clear; // when held, the VN waits for user click to go next.

    // Use this for initialization
    void Start () {
        currentScript = LoadScript("VNScripts/part1");
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space")) {
            NextStep();  
        };
        ScriptProcessingLoop();

	}

    private void ScriptProcessingLoop() {
        while (true) {
            if (hold == HoldState.Held) { break; }

            // Get Current Command Token
            JToken command = currentScript[route][step];
            Debug.Log(command);

            if (command.Type == JTokenType.String) {
                SetDialogue(command.Value<string>());

            } else if (command["CreateElement"] != null) {
                Element.CreateElementFromCommand(command);

            } else if (command["DestroyElement"] != null) {
                Element.DestroyElement(command["DestroyElement"].Value<string>());
            }
            hold = Hold.GetHoldState(command);
            if (hold == HoldState.Clear) step++;
        }
    }

    void NextStep() {
        if (hold == HoldState.Held) {
            step++;
            hold = HoldState.Clear;
        }
    }

    void SetDialogue(string text, bool isScrolled=true) {
        dialogueBox.text = jsEngine.FormatString(text);
    }

    // Note: drop the file extension.
    private static JObject LoadScript(string path) {
        var scriptFile = Resources.Load<TextAsset>(path);
        return JObject.Parse(scriptFile.text);
    }

    public static Vector3 ScreenToWorld(float x, float y, float z = 0) {
        float bx = x / ScreenWidth * Screen.width;
        float by = Screen.height - y / ScreenHeight * Screen.height;

        var worldVector = Camera.main.ScreenToWorldPoint(new Vector3(bx, by, 0f));
        return new Vector3(worldVector.x, worldVector.y, z);
    }
}

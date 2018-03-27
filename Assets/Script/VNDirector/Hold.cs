using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Xml;
/***
* [Clear] means that commands are passed automatically, allowing execution of multiple commands in one cycle
* [Held] holds the VN execution to the point, executing when Nexted.
*/
public enum HoldState { Clear, Held, Lock, Await, Skippable }

public class Hold {
    // Use this for initialization
    public static HoldState DefTypes(string hold) {
        switch (hold) {
            case "h":
                return HoldState.Held;
            case "l":
                return HoldState.Lock;
            default:
                return HoldState.Clear;
        }
    }

    public static HoldState GetHoldState(XmlNode command) {
        //Debug.Log(command["_h"].Type);
        if (command.Attributes?["_h"] == null) {
            switch (command.Name) {
                case "Text":
                    return HoldState.Held;
                default:
                    break;
            }
        } else { //_h tag exists
            switch (command.Attributes["_h"].Value) {
                case "c":
                    return HoldState.Clear;
                case "h":
                    return HoldState.Held;
                default:
                    break;
            }
        }

        return HoldState.Clear;
    }
}

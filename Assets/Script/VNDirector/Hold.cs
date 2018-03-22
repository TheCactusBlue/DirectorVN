using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;
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

    public static HoldState GetHoldState(JToken command) {
        //Debug.Log(command["_h"].Type);
        if (command.Type == JTokenType.String) {
            return HoldState.Held;
        } else if (command["_h"] == null) {
            return HoldState.Clear;
        } else if (command["_h"].Type == JTokenType.String) { //_h tag exists

            switch (command["_h"].Value<string>()) {
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

using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;

public class SaveLoad : MonoBehaviour {

    public Manager manager;
    // Use this for initialization
    static void Load() {

    }

    // Update is called once per frame
    public void Save() {
        JObject s = new JObject {
            ["progress"] = new JObject {
                ["route"] = manager.route,
                ["step"] = manager.step
            },
            ["jsData"] = Manager.jsEngine.SerializeToJSON(),
        };

        Debug.Log(s.ToString());
    }
}

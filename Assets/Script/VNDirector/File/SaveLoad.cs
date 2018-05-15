using UnityEngine;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;

public class SaveLoad : MonoBehaviour {

    public Manager manager;

    // Use this for initialization
    static void Load() {

    }

    // Update is called once per frame
    public void Save(int saveSlot) {
        JObject s = new JObject {
            ["progress"] = new JObject {
                ["route"] = manager.route,
                ["step"] = manager.step
            },
            ["jsData"] = Manager.jsEngine.SerializeToJSON(),
        };
        File.WriteAllText(Application.dataPath + "/saves/save1.json", s.ToString());
        Debug.Log(s.ToString());
    }
}

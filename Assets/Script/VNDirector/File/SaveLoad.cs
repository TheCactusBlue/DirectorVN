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
        JObject s = new JObject();
        
        Debug.Log(manager.route+" "+manager.step);
    }
}

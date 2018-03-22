using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Jint;
using Newtonsoft.Json;
using UnityEngine;

public class JSEngine {

    public Engine engine;
    private static Regex formatRegex = new Regex(@"\#{(.+?)\}");

    public JSEngine() {
        engine = new Engine();
        engine.SetValue("log", new System.Action<object>(Debug.Log));
        engine.SetValue("name", "CactusBlue");
	}

    public Jint.Native.Object.ObjectInstance Serialize() {
        return engine.GetValue("data").AsObject();
    }
    
    public string SerializeToJSON() {
        return JsonConvert.SerializeObject(Serialize());
    }

    public string FormatString(string template) {

        var formatDict = new Dictionary<string, string>();

        foreach (Match match in formatRegex.Matches(template)) {
            formatDict[match.Value] = engine.Execute(match.Value.Substring(2, match.Value.Length - 3)).GetCompletionValue().AsString();
        }

        foreach (var entry in formatDict) {
            template = template.Replace(entry.Key, entry.Value);
        }
        return template;
    }
}
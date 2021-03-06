﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Jint;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// A wrapper around JINT
/// </summary>
public class JSEngine : Engine {
    // use like #{whateverTemplate}
    private static readonly Regex formatRegex = new Regex(@"\#{(.+?)\}");

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataJSON">If filled, loads variable "data" from above</param>
    public JSEngine(string dataJSON="{}") : base() {
        SetValue("log", new System.Action<object>(Debug.Log));
        SetValue("name", "CactusBlue");
        Execute($"var data = {dataJSON}");
	}

    /// <summary>
    /// Returns contents of object "data" from JINT namespace.
    /// </summary>
    public Jint.Native.Object.ObjectInstance Serialize() {
        return GetValue("data").AsObject();
    }

    /// <summary>
    /// Returns contents of object "data" from JINT namespace as JSON.
    /// </summary>
    /// <remarks>
    /// Only JSON approved types are allowed. (Obviously)
    /// </remarks>
    public string SerializeToJSON() {
        return Execute("JSON.stringify(data)").GetCompletionValue().AsString();
    }

    /// <summary>
    /// Formats string with JINT expressions. You can't escape the format so far, so f**k you.
    /// </summary>
    /// <param name="template">A string to use as template</param>
    /// <example>
    /// JSEngine.FormatString("Some testing with #{data.engineName}");
    /// </example>
    public string FormatString(string template) {

        var formatDict = new Dictionary<string, string>();

        foreach (Match match in formatRegex.Matches(template)) {
            formatDict[match.Value] = Execute(match.Value.Substring(2, match.Value.Length - 3)).GetCompletionValue().AsString();
        }
        foreach (var entry in formatDict) {
            template = template.Replace(entry.Key, entry.Value);
        }
        return template;
    }
}
using UnityEngine;
using System.Collections;
using System.Xml;

/// <summary>
/// A helper class for executing VNX steps as Element methods
/// </summary>
public static class XMLElementAction {

    public static void Create(XmlNode command) {
        Element.Create(
            command.Attributes["name"].Value,
            command.Attributes["path"].Value,
            new Vector2( // Position
                float.Parse(command.Attributes["x"].Value),
                float.Parse(command.Attributes["y"].Value)
            ),
            new Vector2( // Scale
                float.Parse(command.Attributes?["s"]?.Value ?? "1"),
                float.Parse(command.Attributes?["s"]?.Value ?? "1")
            ),
            float.Parse(command.Attributes?["r"]?.Value ?? "0"), // Rotation
            int.Parse(command.Attributes?["z"]?.Value ?? "0") // Z-Order
        );
    }

    public static void Move(XmlNode command) {
        
    }
}

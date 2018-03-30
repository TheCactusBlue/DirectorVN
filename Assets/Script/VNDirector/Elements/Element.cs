using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[Serializable]
public class Element : MonoBehaviour {

    public static Dictionary<string, GameObject> collection = new Dictionary<string, GameObject>();
    public static GameObject stage = GameObject.Find("Stage");

    public float zOrder;

    private Vector2 _screenPos;
    public float _rotation;

    public Vector2 Position {
        get {
            return _screenPos;
        }

        set {
            _screenPos = value;
            gameObject.transform.position = Manager.ScreenToWorld(value.x, value.y, 0);
        }
    }

    public Vector2 Scale {
        get {
            return gameObject.transform.localScale;
        }

        set {
            gameObject.transform.localScale = value;
        }
    }

    public float Rotation {
        get {
            return _rotation;
        }

        set {
            _rotation = value;
            transform.rotation = Quaternion.AngleAxis(value, Vector3.back);
        }
    }

    public static void CreateElementFromXML(XmlNode command) {
        CreateElement(
            command.Attributes["name"].Value,
            command.Attributes["path"].Value,
            new Vector2(
                float.Parse(command.Attributes["x"].Value),
                float.Parse(command.Attributes["y"].Value)
            ),
            new Vector2(
                float.Parse(command.Attributes?["s"]?.Value ?? "1"),
                float.Parse(command.Attributes?["s"]?.Value ?? "1")
            ),
            float.Parse(command.Attributes?["r"]?.Value ?? "0"),
            int.Parse(command.Attributes?["o"]?.Value ?? "0")
        );
    }

    public void MoveFromXML(XmlNode command) {
    }

    public static void CreateElement(string name, string spritePath, Vector2 position, Vector2? scale = null, float rotation = 0, int order = 0) {

        var gameElement = new GameObject(name);
        var spriteComponent = gameElement.AddComponent<SpriteRenderer>();
        var element = gameElement.AddComponent<Element>();

        spriteComponent.sortingOrder = order;

        spriteComponent.sprite = ResourceController.Get<Sprite>(spritePath);


        element.Position = position;
        element.Scale = scale ?? Vector2.one;
        element.Rotation = rotation;

        element.transform.SetParent(stage.transform);

        collection[name] = gameElement;
    }

    public static void DestroyElement(string name) {
        Destroy(collection[name]);
        collection.Remove(name);
    }

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Element : MonoBehaviour
{
    public static Dictionary<string, GameObject> collection = new Dictionary<string, GameObject>();

    public float zOrder;

    public float rotation;

    private Vector2 _screenPos;

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

    public static void CreateElementFromCommand(JToken command) {
        Vector2 scaleVector = Vector2.one;
        float rotation = 0;
        int order = 0;

        if (command["s"] != null) {
            scaleVector = new Vector2(command["s"].Value<float>(), command["s"].Value<float>());
        }
        if (command["r"] != null) {
            rotation = command["r"].Value<float>();
        }
        if (command["o"] != null) {
            order = command["o"].Value<int>();
        }

        CreateElement(
            command["CreateElement"].Value<string>(),
            command["path"].Value<string>(),
            new Vector2(command["x"].Value<float>(), command["y"].Value<float>()),
            scaleVector,
            rotation,
            order
        );
    }

    public static void CreateElement(string name, string spritePath, Vector2 position, Vector2? scale = null, float rotation = 0, int order = 0) {

        var gameElement = new GameObject(name);
        var spriteComponent = gameElement.AddComponent<SpriteRenderer>();
        var element = gameElement.AddComponent<Element>();

        spriteComponent.sortingOrder = order;
        spriteComponent.sprite = Resources.Load<Sprite>(spritePath);

        element.Position = position;
        element.Scale = scale ?? Vector2.one;

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

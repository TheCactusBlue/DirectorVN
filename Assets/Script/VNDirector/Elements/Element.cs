using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Element : MonoBehaviour {

    public static Dictionary<string, GameObject> instances = new Dictionary<string, GameObject>();
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

    public static void Create(string name, string spritePath, Vector2 position, Vector2? scale = null, float rotation = 0, int order = 0) {

        var gameElement = new GameObject(name);
        var spriteComponent = gameElement.AddComponent<SpriteRenderer>();
        var element = gameElement.AddComponent<Element>();

        spriteComponent.sortingOrder = order;

        spriteComponent.sprite = ResourceController.Get<Sprite>(spritePath);


        element.Position = position;
        element.Scale = scale ?? Vector2.one;
        element.Rotation = rotation;

        element.transform.SetParent(stage.transform);

        instances[name] = gameElement;
    }

    public static void Destroy(string name) {
        Destroy(instances[name]);
        instances.Remove(name);
    }

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }
}

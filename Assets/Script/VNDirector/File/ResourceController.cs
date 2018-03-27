using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceController {

    private static Dictionary<string, Object> resources = new Dictionary<string, Object>();
    private static Dictionary<string, ResourceRequest> resourceRequests = new Dictionary<string, ResourceRequest>();

    public static List<string> toRemove = new List<string>();

    public static void Update() {
        foreach (KeyValuePair<string, ResourceRequest> resource in resourceRequests) {
            if (resource.Value.isDone) {
                resources[resource.Key] = resource.Value.asset;
                toRemove.Add(resource.Key);
            }
        }

        foreach (string key in toRemove) {
            resourceRequests.Remove(key);
        }
    }

    // Preloads a resource into memory.
    public static void Load(string path) {
        resourceRequests[path] = Resources.LoadAsync(path);
    }

    public static void Load<T>(string path) where T : Object {
        resourceRequests[path] = Resources.LoadAsync<T>(path);
    }

    public static void Unload(string path) {

    }

    public static T Get<T>(string path) where T : Object {
        if (resources.ContainsKey(path)) {
            // Debug.Log(resources[path]);
            return (T) resources[path];
        }
        return Resources.Load<T>(path);
    }
}

using System.Collections.Generic;
using Cacophony;
using UnityEngine;

public class CacophonousGestureConsumer : MonoBehaviour
{
    public List<GameObject> prefabList = new List<GameObject>();
    public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    public List<HandGestureManager> gestureManagers = new();
    public GameObject gestureParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        prefabs = new Dictionary<string, GameObject>();
        gestureManagers = new List<HandGestureManager>();
        // Find all HandGestureManager components in children
        var gestures = gestureParent.GetComponentsInChildren<HandGestureManager>();
        foreach (var gesture in gestures)
        {
            string name = gesture.name;
            gestureManagers.Add(gesture);
            prefabs.Add(name, prefabList.Find(x => x.name == name));
            gesture.actionProcessor.OnStart.AddListener(
                (gesture) =>
                {
                    Debug.Log("Gesture started: " + gesture);
                    SpawnPrefab(name);
                }
            );
        }

        Debug.Log(prefabs.Count + " prefabs loaded.");
    }


    private void SpawnPrefab(string name)
    {
        Debug.Log("Spawning prefab: " + name);
        if (prefabs.ContainsKey(name))
        {
            GameObject prefab = prefabs[name];
            GameObject instance = Instantiate(prefab, transform.position + Random.insideUnitSphere, Random.rotation);
            instance.transform.SetParent(transform);
            instance.name = prefab.name;
        }
        else
        {
            Debug.LogWarning($"Prefab with name {name} not found.");
        }
    }
}

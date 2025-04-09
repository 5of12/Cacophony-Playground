using System.Collections.Generic;
using Cacophony;
using UnityEngine;

public class CacophonousGestureConsumer : MonoBehaviour
{
    public enum EmitMode
    {
        Immediate,
        AfterAction
    }

    public List<GameObject> prefabList = new List<GameObject>();
    public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    public List<HandGestureManager> gestureManagers = new();
    public GameObject gestureParent;
    public Gradient colours;
    public ParticleSystem confetti;

    [Header("Reduce Cacophony")]
    [Tooltip("Select whether to react as soon as the gesture is detected or after the action is completed")]
    public EmitMode emitMode = EmitMode.Immediate;

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
                (e) =>
                {
                    if (emitMode == EmitMode.Immediate)
                    {
                        SpawnPrefab(name);
                    }
                }
            );
            gesture.actionProcessor.OnEnd.AddListener(
                (e) =>
                {
                    if (emitMode == EmitMode.AfterAction)
                    {
                        SpawnPrefab(name);
                    }
                }
            );
        }
    }


    private void SpawnPrefab(string name)
    {
        if (prefabs.ContainsKey(name))
        {
            GameObject prefab = prefabs[name];
            GameObject instance = Instantiate(prefab, transform.position + (Random.insideUnitSphere * 0.25f), Random.rotation);
            instance.transform.SetParent(transform);
            instance.name = prefab.name;
            instance.GetComponent<Renderer>().material.color = colours.Evaluate(Random.Range(0f, 1f));
            confetti.Emit(50);
        }
        else
        {
            Debug.LogWarning($"Prefab with name {name} not found.");
        }
    }
}

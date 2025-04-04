using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSelectorShapeGenerator : MonoBehaviour
{
    public Transform targetObject;
    public List<Transform> subTargets;
    public GameObject shapePrefab;
    public Vector3 numShapes = new Vector3(20, 5, 10);
    public float minSize = 0.1f;
    public float maxSize = 0.5f;
    public float forceUpdateInterval = 4f;
    private List<Rigidbody> shapes = new List<Rigidbody>();
    private Vector3 randomTarget;
    private float maxForce = 1f;
    private bool underInfluence;
    private int numTargets = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // Instantiate shapes using the mesh attached to this object as a bounding box
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        var boundingBox = mesh.bounds;
        GameObject shapeParent = new GameObject("ShapeParent");
        for (int i = 0; i < numShapes.x; i++)
        {
            for (int j = 0; j < numShapes.y; j++)
            {
                for (int k = 0; k < numShapes.z; k++)
                {
                    Vector3 position = new Vector3(
                        (Random.Range(boundingBox.min.x, boundingBox.max.x) * transform.localScale.x) + transform.position.x,
                        (Random.Range(boundingBox.min.y, boundingBox.max.y) * transform.localScale.y) + transform.position.y,
                        (Random.Range(boundingBox.min.z, boundingBox.max.z) * transform.localScale.z) + transform.position.z
                    );

                    GameObject shape = Instantiate(shapePrefab, position, Quaternion.identity);
                    float sizeFactor = Random.Range(minSize, maxSize);
                    shape.transform.localScale = new Vector3(sizeFactor, sizeFactor, sizeFactor);
                    shape.transform.parent = shapeParent.transform; // Set the parent to this object

                    shapes.Add(shape.GetComponent<Rigidbody>());
                    // shape.GetComponentInChildren<Renderer>().material.color = gradient.Evaluate(Random.Range(0f, 1f));
                }
            }
        }

        AddForceToShapes();
        maxForce = 0.5f;
        StartCoroutine(WaitForSeconds(forceUpdateInterval));
    }

    // Update is called once per frame
    void Update()
    {
        // targetObject.position = Vector3.Lerp(targetObject.position, randomTarget, Time.deltaTime / forceUpdateInterval);
        targetObject.position = Vector3.MoveTowards(targetObject.position, randomTarget, 0.0001f);
    }

    private IEnumerator WaitForSeconds(float seconds)
    {
        while(gameObject.activeSelf)
        {
            yield return new WaitForSeconds(seconds);
            if (!underInfluence)
            {
                AddForceToShapes();
            }
            else
            {
                AttractShapesToTarget(subTargets);
            }
        }
    }

    private void AddForceToShapes()
    {
        foreach(var shape in shapes)
        {
            AddForceToShape(shape, targetObject, 1, ForceMode.Impulse);
        }
        randomTarget = Random.insideUnitSphere;;
    }

    public void AddForceToShape(Rigidbody shape, Transform targetObject, float noiseStrength, ForceMode forceMode)
    {
        // Add a force pulling the shape towards the targetObject
        Vector3 direction = (targetObject.position - shape.transform.position).normalized;
        float distance = Vector3.Distance(targetObject.position, shape.transform.position);
        
        float forceMagnitude = Mathf.Clamp(distance - targetObject.localScale.x, 0, maxForce);

        Vector3 randomForce = Random.onUnitSphere * Random.Range(0.05f, 0.2f) * noiseStrength;
        Vector3 force = randomForce + direction * forceMagnitude;
        shape.AddForce(force, forceMode);
    }

    // Public function to attract the shapes towards one of a list of targets
    public void AttractShapesToTarget(List<Transform> targets)
    {
        int index = 0;
        int num = Mathf.Min(numTargets, targets.Count);

        foreach (var shape in shapes)
        {
            AddForceToShape(shape, targets[index], 0, ForceMode.Impulse);
            index = index + 1 < num ? index + 1: 0;
        }
    }

    public void SetTargetCount(int targetId, float progress)
    {
        if (!underInfluence)
        {
            underInfluence = true;
            numTargets = targetId;
            maxForce = 4f;
            // Attract shapes to the targets
            AttractShapesToTarget(subTargets);
        }
    }

    public void ReleaseInfluence(int targetId)
    {
        if (underInfluence && numTargets == targetId)
        {
            underInfluence = false;
            numTargets = 0;
            maxForce = 0.5f;
        }
    }
}

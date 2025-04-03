using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSelectorShapeGenerator : MonoBehaviour
{
    public Transform targetObject;
    public GameObject shapePrefab;

    public Vector3 numShapes = new Vector3(20, 5, 10);
    public float minSize = 0.1f;
    public float maxSize = 0.5f;

    public List<Rigidbody> shapes = new List<Rigidbody>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity = new Vector3(0, -2f, 0);

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
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        AddForceToShapes();
        StartCoroutine(WaitForSeconds(1f));
    }

    private IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        AddForceToShapes();
    }

    private void AddForceToShapes()
    {
        foreach(var shape in shapes)
        {
            // Add a force pulling the shape towards the targetObject
            Vector3 direction = (targetObject.position + Random.onUnitSphere  - shape.transform.position).normalized;
            float distance = Vector3.Distance(targetObject.position, shape.transform.position);
            float forceMagnitude = Mathf.Clamp(distance, 0, 0.2f);

            shape.AddForce(direction * forceMagnitude * Time.deltaTime, ForceMode.Impulse);
        }
    }
}

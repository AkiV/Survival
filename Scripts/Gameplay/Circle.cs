using UnityEngine;
using System.Collections;

public class Circle : MonoBehaviour 
{
    public Color color = Color.red;
    public float radius = 15.0f;

    private LineRenderer lineRenderer;
    private float offsetY = 0.1f;

    void OnDrawGizmos()
    {
        Gizmos.color = color;

        Vector3[] circleVertices = ConstructCircle(100);

        for (int i = 0; i < circleVertices.Length - 1; i++)
            Gizmos.DrawLine(circleVertices[i], circleVertices[i + 1]);

        Gizmos.DrawLine(circleVertices[0], circleVertices[circleVertices.Length - 1]);
    }

    Vector3[] ConstructCircle(int verticesCount)
    {
        Vector3[] vertices = new Vector3[verticesCount];

        for (int i = 0; i < verticesCount; i++)
        {
            float slice = Mathf.Deg2Rad * (360.0f / verticesCount) * (i - 1);

            Vector3 vertex = new Vector3(transform.position.x + Mathf.Cos(slice) * radius,
                                         transform.position.y + offsetY,
                                         transform.position.z + Mathf.Sin(slice) * radius);

            vertices[i] = vertex;
        }

        return vertices;
    }

    public bool InsideCircle(Vector3 point)
    {
        return (Vector3.Distance(point, transform.position) < radius);
    }

    public Vector3 RandomOnCircle()
    {
        float angle = Random.Range(-Mathf.PI, Mathf.PI);
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);
        return (new Vector3(x, 0, y) * radius);
    }
}

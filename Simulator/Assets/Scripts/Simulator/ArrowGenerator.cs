using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ArrowGenerator : MonoBehaviour
{
    [SerializeField]
    private const float defaultStemLength = 4f;
    [SerializeField]
    private const float defaultStemWidth = 0.3f;
    [SerializeField]
    private const int defaultSegments = 26;
    [SerializeField]
    private const float defaultTipLength = 1;
    [SerializeField]
    private const float defaultTipWidth = 1;
    [SerializeField]
    private const float colliderScale = 1.2f;

    public float stemLength = defaultStemLength;
    public float stemWidth = defaultStemWidth;
    public int segments = defaultSegments;

    public float tipLength = defaultTipLength;
    public float tipWidth = defaultTipWidth;

    public Color color = Color.white;
    public Material material;

    [System.NonSerialized]
    public List<Vector3> vertices;
    [System.NonSerialized]
    public List<int> triangles;

    Mesh mesh;
    CapsuleCollider collider;

    void Awake()
    {
        //make sure Mesh Renderer has a material
        mesh = new Mesh();
        if (!collider) {
            collider = gameObject.AddComponent<CapsuleCollider>();
            collider.isTrigger = true;
        }
        this.GetComponent<MeshFilter>().mesh = mesh;
        setSettings(1, color);
        GenerateArrow();
    }

    public void setSettings(float scale, Color col) {
        stemLength = defaultStemLength * scale;
        stemWidth = defaultStemWidth * scale;
        segments = defaultSegments;
        tipLength = defaultTipLength * scale;
        tipWidth = defaultTipWidth * scale;
        color = col;

        material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.color = color;
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", color * 1.5f);

        GetComponent<MeshRenderer>().material = material;
    }
    //void Update()
    //{
    //    GenerateArrow();
    //}

    void OnValidate() {
        GenerateArrow();
    }

    //arrow is generated starting at Vector3.zero
    //arrow is generated facing right, towards radian 0.
    public void GenerateArrow()
    {
        transform.localPosition = Vector3.zero;
        //setup
        vertices = new List<Vector3>();
        triangles = new List<int>();

        //stem setup
        Vector3 stemOrigin = Vector3.zero;
        float stemHalfWidth = stemWidth/2f;

        //Stem vertices
        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            float x = Mathf.Cos(angle) * stemHalfWidth;
            float z = Mathf.Sin(angle) * stemHalfWidth;

            vertices.Add(new Vector3(x, 0, z));
            vertices.Add(new Vector3(x, stemLength, z));
        }

        //Stem triangles
        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;

            int b1 = i * 2;
            int t1 = i * 2 + 1;
            int b2 = next * 2;
            int t2 = next * 2 + 1;

            triangles.Add(b1);
            triangles.Add(t1);
            triangles.Add(b2);

            triangles.Add(t1);
            triangles.Add(t2);
            triangles.Add(b2);
        }
                
        //tip setup
        Vector3 tipOrigin = stemLength*Vector3.up;
        float tipHalfWidth = tipWidth/2;

        //tip circle points
        int tipStartIndex = vertices.Count;
        for (int i = 0; i < segments; i++) {
            float angle = 2 * Mathf.PI * i / segments;
            float x = Mathf.Cos(angle) * tipHalfWidth;
            float z = Mathf.Sin(angle) * tipHalfWidth;

            vertices.Add(new Vector3(x, stemLength, z));
        }

        //Cone
        vertices.Add(new Vector3(0, stemLength + tipLength, 0));
        int topConeIndex = vertices.Count - 1;

        //Cone triangles;
        for (int i = 0; i < segments; i++) {
            int current = tipStartIndex + i;
            int next = tipStartIndex + (i + 1) % segments;

            triangles.Add(topConeIndex);
            triangles.Add(next);
            triangles.Add(current);
        }
        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;

            int cylTop1 = i * 2 + 1;
            int cylTop2 = next * 2 + 1;

            int coneBase1 = tipStartIndex + i;
            int coneBase2 = tipStartIndex + next;

            triangles.Add(cylTop1);
            triangles.Add(coneBase1);
            triangles.Add(cylTop2);

            triangles.Add(cylTop2);
            triangles.Add(coneBase1);
            triangles.Add(coneBase2);
        }

        //assign lists to mesh.
        if (mesh) {
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();

            collider.height = (stemLength + tipLength) * colliderScale;      
            collider.radius = (stemWidth / 2) * colliderScale;
            collider.center = Vector3.zero + Vector3.up * ((stemLength + tipLength) / 2); 
            collider.direction = 1; 
        }
    }

    public void Glow() {
        material.SetColor("_EmissionColor", color * 2f);
    }
    public void ResetColor() {
        material.color = color;
        material.SetColor("_EmissionColor", color * 1.5f);
    }
    public void ChangeColorTemporaly(Color col) {
        material.color = col;
        material.SetColor("_EmissionColor", col);
    }
}
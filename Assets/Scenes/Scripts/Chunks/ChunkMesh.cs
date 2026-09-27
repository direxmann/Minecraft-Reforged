using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class ChunkMesh : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private Chunk chunk;

    [SerializeField] private BlockDatabase blockDatabase;

    [Header("Materials")]
    [SerializeField] private Material stoneMaterial;
    [SerializeField] private Material dirtMaterial;
    [SerializeField] private Material grassSideMaterial;
    [SerializeField] private Material grassTopMaterial;

    private readonly List<Vector3> vertices = new();
    private readonly List<int> triangles = new();
    private readonly List<Vector2> uvs = new();

    private readonly List<int> stoneTriangles = new();
    private readonly List<int> dirtTriangles = new();
    private readonly List<int> grassSideTriangles = new();
    private readonly List<int> grassTopTriangles = new();

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        chunk = GetComponent<Chunk>();
    }

    private void Start()
    {
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();

        stoneTriangles.Clear();
        dirtTriangles.Clear();
        grassSideTriangles.Clear();
        grassTopTriangles.Clear();

        for (int x = 0; x < Chunk.Size; x++)
        {
            for (int y = 0; y < Chunk.Size; y++)
            {
                for (int z = 0; z < Chunk.Size; z++)
                {
                    BlockType blockType = chunk.GetBlock(x, y, z);

                    if (blockType == BlockType.Air)
                        continue;

                    AddBlock(x, y, z, blockType);
                }
            }
        }

        Mesh mesh = new Mesh();

        mesh.indexFormat = IndexFormat.UInt32;

        mesh.vertices = vertices.ToArray();

        mesh.subMeshCount = 4;

        mesh.SetTriangles(stoneTriangles, 0);
        mesh.SetTriangles(dirtTriangles, 1);
        mesh.SetTriangles(grassSideTriangles, 2);
        mesh.SetTriangles(grassTopTriangles, 3);

        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        meshRenderer.materials = new Material[]
        {
            stoneMaterial,
            dirtMaterial,
            grassSideMaterial,
            grassTopMaterial
        };

        meshCollider.sharedMesh = mesh;
    }

    private void AddBlock(int x, int y, int z, BlockType blockType)
    {
        Vector3 position = new Vector3(x, y, z);

        switch (blockType)
        {
            case BlockType.Stone:

                if (chunk.GetBlock(x, y + 1, z) == BlockType.Air)
                    AddTopFace(position, stoneTriangles);

                if (chunk.GetBlock(x, y - 1, z) == BlockType.Air)
                    AddBottomFace(position, stoneTriangles);

                if (chunk.GetBlock(x - 1, y, z) == BlockType.Air)
                    AddLeftFace(position, stoneTriangles);

                if (chunk.GetBlock(x + 1, y, z) == BlockType.Air)
                    AddRightFace(position, stoneTriangles);

                if (chunk.GetBlock(x, y, z - 1) == BlockType.Air)
                    AddFrontFace(position, stoneTriangles);

                if (chunk.GetBlock(x, y, z + 1) == BlockType.Air)
                    AddBackFace(position, stoneTriangles);

                break;

            case BlockType.Dirt:

                if (chunk.GetBlock(x, y + 1, z) == BlockType.Air)
                    AddTopFace(position, dirtTriangles);

                if (chunk.GetBlock(x, y - 1, z) == BlockType.Air)
                    AddBottomFace(position, dirtTriangles);

                if (chunk.GetBlock(x - 1, y, z) == BlockType.Air)
                    AddLeftFace(position, dirtTriangles);

                if (chunk.GetBlock(x + 1, y, z) == BlockType.Air)
                    AddRightFace(position, dirtTriangles);

                if (chunk.GetBlock(x, y, z - 1) == BlockType.Air)
                    AddFrontFace(position, dirtTriangles);

                if (chunk.GetBlock(x, y, z + 1) == BlockType.Air)
                    AddBackFace(position, dirtTriangles);

                break;

            case BlockType.Grass:

                if (chunk.GetBlock(x, y + 1, z) == BlockType.Air)
                    AddTopFace(position, grassTopTriangles);

                if (chunk.GetBlock(x, y - 1, z) == BlockType.Air)
                    AddBottomFace(position, dirtTriangles);

                if (chunk.GetBlock(x - 1, y, z) == BlockType.Air)
                    AddLeftFace(position, grassSideTriangles);

                if (chunk.GetBlock(x + 1, y, z) == BlockType.Air)
                    AddRightFace(position, grassSideTriangles);

                if (chunk.GetBlock(x, y, z - 1) == BlockType.Air)
                    AddFrontFace(position, grassSideTriangles);

                if (chunk.GetBlock(x, y, z + 1) == BlockType.Air)
                    AddBackFace(position, grassSideTriangles);

                break;
        }
    }

    private void AddTopFace(Vector3 position, List<int> targetTriangles)
    {
        int vertexIndex = vertices.Count;

        vertices.Add(position + new Vector3(0, 1, 0));
        vertices.Add(position + new Vector3(0, 1, 1));
        vertices.Add(position + new Vector3(1, 1, 1));
        vertices.Add(position + new Vector3(1, 1, 0));

        AddFace(
            vertexIndex,
            targetTriangles,
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0)
        );
    }

    private void AddBottomFace(Vector3 position, List<int> targetTriangles)
    {
        int vertexIndex = vertices.Count;

        vertices.Add(position + new Vector3(0, 0, 0));
        vertices.Add(position + new Vector3(1, 0, 0));
        vertices.Add(position + new Vector3(1, 0, 1));
        vertices.Add(position + new Vector3(0, 0, 1));

        AddFace(
            vertexIndex,
            targetTriangles,
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        );
    }

    private void AddFrontFace(Vector3 position, List<int> targetTriangles)
    {
        int vertexIndex = vertices.Count;

        vertices.Add(position + new Vector3(0, 0, 0));
        vertices.Add(position + new Vector3(0, 1, 0));
        vertices.Add(position + new Vector3(1, 1, 0));
        vertices.Add(position + new Vector3(1, 0, 0));

        AddFace(
            vertexIndex,
            targetTriangles,
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0)
        );
    }

    private void AddBackFace(Vector3 position, List<int> targetTriangles)
    {
        int vertexIndex = vertices.Count;

        vertices.Add(position + new Vector3(0, 0, 1));
        vertices.Add(position + new Vector3(1, 0, 1));
        vertices.Add(position + new Vector3(1, 1, 1));
        vertices.Add(position + new Vector3(0, 1, 1));

        AddFace(
            vertexIndex,
            targetTriangles,
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        );
    }

    private void AddLeftFace(Vector3 position, List<int> targetTriangles)
    {
        int vertexIndex = vertices.Count;

        vertices.Add(position + new Vector3(0, 0, 0));
        vertices.Add(position + new Vector3(0, 0, 1));
        vertices.Add(position + new Vector3(0, 1, 1));
        vertices.Add(position + new Vector3(0, 1, 0));

        AddFace(
            vertexIndex,
            targetTriangles,
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        );
    }

    private void AddRightFace(Vector3 position, List<int> targetTriangles)
    {
        int vertexIndex = vertices.Count;

        vertices.Add(position + new Vector3(1, 0, 0));
        vertices.Add(position + new Vector3(1, 1, 0));
        vertices.Add(position + new Vector3(1, 1, 1));
        vertices.Add(position + new Vector3(1, 0, 1));

        AddFace(
            vertexIndex,
            targetTriangles,
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0)
        );
    }

    private void AddFace(int vertexIndex, List<int> targetTriangles, Vector2 uv0, Vector2 uv1, Vector2 uv2, Vector2 uv3)
    {
        targetTriangles.Add(vertexIndex);
        targetTriangles.Add(vertexIndex + 1);
        targetTriangles.Add(vertexIndex + 2);

        targetTriangles.Add(vertexIndex);
        targetTriangles.Add(vertexIndex + 2);
        targetTriangles.Add(vertexIndex + 3);

        uvs.Add(uv0);
        uvs.Add(uv1);
        uvs.Add(uv2);
        uvs.Add(uv3);
    }
}
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private int worldSize = 3;

    [SerializeField] private GameObject chunkPrefab;

    private void Start()
    {
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int z = 0; z < worldSize; z++)
            {
                CreateChunk(x, z);
            }
        }
    }

    private void CreateChunk(int x, int z)
    {
        Vector3 position = new Vector3(
            x * Chunk.Size,
            0,
            z * Chunk.Size
        );

        Instantiate(
            chunkPrefab,
            position,
            Quaternion.identity,
            transform
        );
    }
}
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public const int Size = 16;

    [SerializeField] private float terrainScale = 0.1f;
    [SerializeField] private int terrainHeight = 8;

    private BlockType[,,] blocks;
    private Vector2 worldPosition;

    public void Initialize()
    {
        blocks = new BlockType[Size, Size, Size];

        worldPosition = new Vector2(
            transform.position.x,
            transform.position.z
        );

        Generate();
    }

    private void Awake()
    {
        Initialize();
    }

    private void Generate()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int z = 0; z < Size; z++)
            {
                float noise = Mathf.PerlinNoise(
                    (worldPosition.x + x) * terrainScale,
                    (worldPosition.y + z) * terrainScale
                );

                int height = Mathf.FloorToInt(noise * terrainHeight);

                for (int y = 0; y < Size; y++)
                {
                    if (y > height)
                    {
                        blocks[x, y, z] = BlockType.Air;
                    }
                    else if (y == height)
                    {
                        blocks[x, y, z] = BlockType.Grass;
                    }
                    else if (y >= height - 2)
                    {
                        blocks[x, y, z] = BlockType.Dirt;
                    }
                    else
                    {
                        blocks[x, y, z] = BlockType.Stone;
                    }
                }
            }
        }
    }

    public BlockType GetBlock(int x, int y, int z)
    {
        if (x < 0 || x >= Size ||
            y < 0 || y >= Size ||
            z < 0 || z >= Size)
        {
            return BlockType.Air;
        }

        return blocks[x, y, z];
    }
}
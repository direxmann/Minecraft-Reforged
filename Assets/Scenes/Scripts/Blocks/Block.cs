using UnityEngine;

public class Block
{
    public BlockType Type { get; }

    public Block(BlockType type)
    {
        Type = type;
    }
}
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "BlockDatabase",
    menuName = "Minecraft Reforged/Block Database"
)]
public class BlockDatabase : ScriptableObject
{
    [SerializeField] private List<BlockDefinition> definitions = new();
    [SerializeField] private Texture2D textureAtlas;
    [SerializeField] private int textureSize = 16;

    private Dictionary<BlockType, BlockDefinition> dictionary;

    private void OnEnable()
    {
        BuildDictionary();
    }

    private void BuildDictionary()
    {
        dictionary = new Dictionary<BlockType, BlockDefinition>();

        foreach (BlockDefinition definition in definitions)
        {
            if (definition == null)
                continue;

            dictionary[definition.type] = definition;
        }
    }

    public BlockDefinition Get(BlockType type)
    {
        if (type == BlockType.Air)
            return null;

        if (dictionary.TryGetValue(type, out BlockDefinition definition))
            return definition;

        return null;
    }

    public Texture2D TextureAtlas => textureAtlas;
    public int TextureSize => textureSize;

}
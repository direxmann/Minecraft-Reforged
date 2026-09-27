using UnityEngine;

[CreateAssetMenu(
    fileName = "BlockDefinition",
    menuName = "Minecraft Reforged/Block Definition"
)]
public class BlockDefinition : ScriptableObject
{
    [Header("Identification")]
    public BlockType type;
    public string blockName;

    [Header("Properties")]
    public bool solid = true;
    public bool transparent = false;
    public float hardness = 1f;

    [Header("Texture Atlas")]
    public Texture2D textureTop;
    public Texture2D textureBottom;
    public Texture2D textureSide;
}
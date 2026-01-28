using Arch.Core;
using Arch.Core.Extensions;
using Escalon;
using UnityEngine;

public class TileView : MonoBehaviour
{
    public GameObject View;
    public MeshRenderer Renderer;
    public SpriteRenderer IconRenderer;
    public Entity Entity;
    [SerializeField]
    private SerializableDictionary<ResourceType, Sprite> _resourceIcons;


    public void SetResource()
    {
        ResourceType resourceType = Entity.Get<ResourceData>().ResourceType;
        IconRenderer.sprite = _resourceIcons[resourceType];
    }
}

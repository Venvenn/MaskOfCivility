using Arch.Core;
using Arch.Core.Extensions;
using Escalon;
using MoreMountains.Feedbacks;
using UnityEngine;

public class TileView : MonoBehaviour
{
    public GameObject View;
    public GameObject Target;
    public MeshRenderer Renderer;
    public SpriteRenderer IconRenderer;
    public Entity Entity;

    [SerializeField]
    private MMF_Player _moveTo;
    
    [SerializeField]
    private SerializableDictionary<ResourceTypes, Sprite> _resourceIcons;

    public void SetResource()
    {
        ResourceTypes resourceTypes = Entity.Get<ResourceData>().ResourceTypes;
        IconRenderer.sprite = _resourceIcons[resourceTypes];
    }

    public void SetDestination(Vector3 position, float duration)
    {
        if (!_moveTo.IsPlaying && View.transform.localPosition != position)
        {
            _moveTo.GetFeedbackOfType<MMF_DestinationTransform>().Duration = duration;
            Target.transform.localPosition = position;
            _moveTo.PlayFeedbacks();
        }
    }
}

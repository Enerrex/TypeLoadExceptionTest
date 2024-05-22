using FlatTop;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Entities;


public struct RequestPathMessage : IComponentData, IEnableableComponent
{
    public float TimeStamp;
    public Entity RequestingEntity;
    public Entity ContainingRegion;
    public OffsetHexCoordinate Start;
    public OffsetHexCoordinate End;
}


public partial struct SelectTilesOnGridJob : IJobEntity
{
    [ReadOnly] public ComponentLookup<LocalTransform> TransformLookup;


    public EntityCommandBuffer CommandBuffer;

    void Execute
    (
        in Entity entity
    )
    {
        CommandBuffer.AddComponent
        (
            entity,
            new RequestPathMessage
            {
                RequestingEntity = new Entity(),
                ContainingRegion = entity,
                Start = new OffsetHexCoordinate(),
                End = new OffsetHexCoordinate(),
            }
        );
    }
}
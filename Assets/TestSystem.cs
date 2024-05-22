using Unity.Entities;
using Unity.Transforms;
using UnityEditor.PackageManager.Requests;
public partial class SelectTileOnGridSystem : SystemBase
{
    private EntityQuery _clickableRegions;
    private EntityCommandBufferSystem _postPlayerInputEcb;

    protected override void OnCreate()
    {
        _postPlayerInputEcb = World.GetOrCreateSystemManaged<EntityCommandBufferSystem>();
        _clickableRegions = SystemAPI.QueryBuilder().WithNone<RequestPathMessage>().Build();
    }

    protected override void OnUpdate()
    {
        var transform_lookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
        var command_buffer = _postPlayerInputEcb.CreateCommandBuffer();
            
        Dependency = new SelectTilesOnGridJob
        {
            TransformLookup = transform_lookup,
            CommandBuffer = command_buffer,
        }.Schedule(_clickableRegions, Dependency);
        _postPlayerInputEcb.AddJobHandleForProducer(Dependency);
    }
}
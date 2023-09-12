using Components;
using Leopotam.EcsLite;

namespace Systems
{
  public class FollowSystem : IEcsRunSystem, IEcsInitSystem
  {
    private EcsWorld _world;
    private EcsPool<PointerFollowableComponent> _mouseFollowablePool;
    private EcsFilter _mouseFollowableFilter;
    private EcsPool<InputPointerPositionComponent> _inputPositionPool;
    private EcsPool<TransformComponent> _transformPool;
    private EcsFilter _mouseInputFilter;
    
    public void Init(IEcsSystems systems)
    {
      _world = systems.GetWorld();
      _mouseFollowablePool = _world.GetPool<PointerFollowableComponent>();
      _inputPositionPool = _world.GetPool<InputPointerPositionComponent>();
      _transformPool = _world.GetPool<TransformComponent>();
      _mouseFollowableFilter = _world.Filter<PointerFollowableComponent>().Inc<TransformComponent>().End();
      _mouseInputFilter = _world.Filter<InputPointerPositionComponent>().End();
    }
    
    public void Run(IEcsSystems systems)
    {
      int mouseInput = _mouseInputFilter.GetRawEntities()[0];
      ref var inputPositionComponent = ref _inputPositionPool.Get(mouseInput);
      foreach (var followable in _mouseFollowableFilter)
      {
        ref var followableComponent = ref _mouseFollowablePool.Get(followable);
        ref var transformComponent = ref _transformPool.Get(followable);
        if(followableComponent.IsFollowable)
          transformComponent.Transform.position = inputPositionComponent.CurrentPosition;
      }
    }

  }
}
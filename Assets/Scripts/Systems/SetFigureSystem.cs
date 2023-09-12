using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
  public class SetFigureSystem : IEcsInitSystem, IEcsRunSystem
  {
    private EcsWorld _world;
    private GameData _gameData;
    private Camera _camera;
    private EcsFilter _pointerClickFilter;
    private EcsPool<PointerComponent> _pointerPool;
    private EcsPool<PointerFollowableComponent> _pointerFollowablePool;
    private EcsPool<InputMouseLeftClickComponent> _inputMouseClickPool;
    private EcsPool<CellComponent> _cellPool;
    private EcsPool<TransformComponent> _transformPool;
    
    public void Init(IEcsSystems systems)
    {
      _world = systems.GetWorld();
      _gameData = systems.GetShared<GameData>();
      _camera = _gameData.Camera;
      _pointerClickFilter = _world.Filter<PointerComponent>().Inc<InputMouseLeftClickComponent>().End();
      _pointerPool = _world.GetPool<PointerComponent>();
      _pointerFollowablePool = _world.GetPool<PointerFollowableComponent>();
      _inputMouseClickPool = _world.GetPool<InputMouseLeftClickComponent>();
      _cellPool = _world.GetPool<CellComponent>();
      _transformPool = _world.GetPool<TransformComponent>();
    }
    
    public void Run(IEcsSystems systems)
    {
      foreach (var pointer in _pointerClickFilter)
      {
        ref var pointerComponent = ref _pointerPool.Get(pointer);
        ref var inputMouseLeftClickComponent = ref _inputMouseClickPool.Get(pointer);
        Ray ray = _camera.ScreenPointToRay(inputMouseLeftClickComponent.Position);
        if (Physics.Raycast(ray.origin, ray.direction * 10f, out var hitInfo))
        {
          if (hitInfo.collider.TryGetComponent(out CellView cellView))
          {
            ref var cellComponent = ref _cellPool.Get(cellView.Entity);
            if (cellComponent.IsFill)
            {
              _inputMouseClickPool.Del(pointer);
              continue;
            }

            ref var pointerFollowableComponent = ref _pointerFollowablePool.Get(pointerComponent.FigureEntity);
            ref var figureTransformComponent = ref _transformPool.Get(pointerComponent.FigureEntity);
            figureTransformComponent.Transform.position = cellView.transform.position;
            cellComponent.FigureEntity = pointerComponent.FigureEntity;
            cellComponent.IsFill = true;
            pointerComponent.IsEmpty = true;
            pointerFollowableComponent.IsFollowable = false;
            _inputMouseClickPool.Del(pointer);
          }
        }
        else
        {
          _inputMouseClickPool.Del(pointer);
        }
      }
    }
  }
}
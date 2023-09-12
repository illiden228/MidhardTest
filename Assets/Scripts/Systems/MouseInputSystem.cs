using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
  public class MouseInputSystem : IEcsRunSystem, IEcsInitSystem
  {
    private EcsWorld _world;
    private GameData _gameData;
    private EcsPool<InputPointerPositionComponent> _inputPositionPool;
    private EcsPool<PointerComponent> _pointerPool;
    private EcsPool<InputMouseLeftClickComponent> _mouseLeftClickPool;
    private EcsFilter _inputPositionFilter;
    private Camera _camera;
    private int _pointerEntity;
    
    public void Init(IEcsSystems systems)
    {
      _world = systems.GetWorld();
      _gameData = systems.GetShared<GameData>();
      _inputPositionPool = _world.GetPool<InputPointerPositionComponent>();
      _pointerPool = _world.GetPool<PointerComponent>();
      _mouseLeftClickPool = _world.GetPool<InputMouseLeftClickComponent>();
      _inputPositionFilter = _world.Filter<InputPointerPositionComponent>().End();
      _camera = _gameData.Camera;

      _pointerEntity = _world.NewEntity();
      _inputPositionPool.Add(_pointerEntity);
      _pointerPool.Add(_pointerEntity);
    }
    
    public void Run(IEcsSystems systems)
    {
      foreach (var inputPosition in _inputPositionFilter)
      {
        ref var inputPositionComponent = ref _inputPositionPool.Get(inputPosition);
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Vector3 normal = Vector3.up;
        float distance = -Vector3.Dot (normal, ray.origin) / Vector3.Dot (normal, ray.direction);
        Vector3 position = ray.origin + ray.direction * distance;

        inputPositionComponent.CurrentPosition = position;
      }

      if (Input.GetMouseButtonDown(0))
      {
        ref InputMouseLeftClickComponent inputMouseLeftClickComponent = ref _mouseLeftClickPool.Add(_pointerEntity);
        inputMouseLeftClickComponent.Position = Input.mousePosition;
      }
    }
  }
}
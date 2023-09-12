using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class FigureInitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private GameData _gameData;
        private EcsPool<PointerFollowableComponent> _pointerFollowablePool;
        private EcsPool<TransformComponent> _transformPool;
        private EcsPool<PointerComponent> _pointerPool;
        private EcsFilter _pointerFilter;
        private int _pointerEntity;
    
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _pointerFollowablePool = _world.GetPool<PointerFollowableComponent>();
            _transformPool = _world.GetPool<TransformComponent>();
            _pointerPool = _world.GetPool<PointerComponent>();
            _pointerFilter = _world.Filter<PointerComponent>().End();
            _gameData = systems.GetShared<GameData>();

            _pointerEntity = _pointerFilter.GetRawEntities()[0];
            ref var pointerComponent = ref _pointerPool.Get(_pointerEntity);
        
            CreateFigure(ref pointerComponent);
        }
    
        public void Run(IEcsSystems systems)
        {
            ref var pointerComponent = ref _pointerPool.Get(_pointerEntity);
            if(pointerComponent.IsEmpty)
                CreateFigure(ref pointerComponent);
        }
    
        private void CreateFigure(ref PointerComponent pointerComponent)
        {
            int newFollowPointerEntity = _world.NewEntity();
            ref var pointerFollowableComponent = ref _pointerFollowablePool.Add(newFollowPointerEntity);
            ref var transformComponent = ref _transformPool.Add(newFollowPointerEntity);

            pointerComponent.FigureEntity = newFollowPointerEntity;
            pointerComponent.IsEmpty = false;
        
            pointerFollowableComponent.IsFollowable = true;
            transformComponent.Transform = GameObject.Instantiate(_gameData.FigurePrefab).transform;
        }
    }
}

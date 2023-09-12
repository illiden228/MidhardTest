using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
  public class GridnitSystem : IEcsInitSystem
  {
    private EcsWorld _world;
    private GameData _gameData;
    private EcsPool<TransformComponent> _transformPool;
    private EcsPool<GridComponent> _gridPool;
    private EcsPool<CellComponent> _cellPool;

    public void Init(IEcsSystems systems)
    {
      _world = systems.GetWorld();
      _gameData = systems.GetShared<GameData>();
      _transformPool = _world.GetPool<TransformComponent>();
      _gridPool = _world.GetPool<GridComponent>();
      _cellPool = _world.GetPool<CellComponent>();

      CreateGrid();
    }

    private void CreateGrid()
    {
      int size = _gameData.GridSize;
      Vector3 startPosition = _gameData.StartGridPosition.position;
      Vector3 currentPosition = startPosition;

      int newGridEntity = _world.NewEntity();
      ref var gridComponent = ref _gridPool.Add(newGridEntity);
      gridComponent.Cells = new int[size, size];

      for (int i = 0; i < size; i++)
      {
        for (int j = 0; j < size; j++)
        {
          CreateCell(_gameData.CellPrefab, currentPosition, j, i, ref gridComponent);
          currentPosition.x += _gameData.CellPrefab.transform.localScale.x;
        }

        currentPosition.z += _gameData.CellPrefab.transform.localScale.z;
        currentPosition.x = startPosition.x;
      }
    }

    private void CreateCell(GameObject prefab, Vector3 position, int x, int y, ref GridComponent gridComponent)
    {
      int newCellEntity = _world.NewEntity();
      ref var transformComponent = ref _transformPool.Add(newCellEntity);
      ref var cellComponent = ref _cellPool.Add(newCellEntity);
      GameObject newCell = GameObject.Instantiate(prefab);
      transformComponent.Transform = newCell.transform;
      transformComponent.Transform.position = position;
      cellComponent.IsFill = false;
      newCell.GetComponent<CellView>().Entity = newCellEntity;
      gridComponent.Cells[x, y] = newCellEntity;
    }
  }
}
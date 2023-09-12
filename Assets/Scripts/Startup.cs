using Leopotam.EcsLite;
using Systems;

public class Startup
{
  private EcsWorld _world;
  private EcsSystems _systems;

  public void Init(GameData gameData)
  {
    _world = new EcsWorld();
    _systems = new EcsSystems(_world, gameData);

    _systems.Add(new MouseInputSystem());
    _systems.Add(new GridnitSystem());
    _systems.Add(new FigureInitSystem());
    _systems.Add(new FollowSystem());
    _systems.Add(new SetFigureSystem());
    _systems.Init();
  }

  public void Run()
  {
    _systems.Run();
  }

  public void Destroy()
  {
    _systems?.Destroy();
    _world?.Destroy();
  }
}
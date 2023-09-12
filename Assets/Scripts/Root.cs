using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private Transform _mainCanvas;
    [SerializeField] private Camera _camera;
    [SerializeField] private LoadingView _loadingViewPrefab;
    [SerializeField] private GameObject _figurePrefab;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private Transform _gridStartPosition;
    [SerializeField] private int _gridSize;
    private Startup _startup;

    private async void Start()
    {
        LoadingModel loading = new LoadingModel(_loadingViewPrefab.gameObject, _mainCanvas);
        loading.Init();
        var asyncOperation = UniTask.Delay(5000);
        await asyncOperation;
        loading.Destroy();

        _startup = new Startup();
        GameData gameData = new GameData();
        gameData.FigurePrefab = _figurePrefab;
        gameData.CellPrefab = _cellPrefab;
        gameData.Camera = _camera;
        gameData.StartGridPosition = _gridStartPosition;
        gameData.GridSize = _gridSize;
        _startup.Init(gameData);
    }

    private void Update()
    {
        _startup?.Run();
    }

    private void OnDestroy()
    {
        _startup.Destroy();
    }
}

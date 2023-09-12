using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DefaultNamespace
{
  public class LoadingModel
  {
    private LoadingView _loadingView;
    private TweenerCore<Quaternion, Vector3, QuaternionOptions> _loadingTween;
    
    public LoadingModel(GameObject loadingView, Transform uiPlace)
    {
      _loadingView = GameObject.Instantiate(loadingView, uiPlace).GetComponent<LoadingView>();
    }
    
    public void Init()
    {
      _loadingTween =_loadingView.LoadingImage.transform
        .DORotate(new Vector3(0, 0, -360f), 2f)
        .SetLoops(-1, LoopType.Restart)
        .SetRelative()
        .SetEase(Ease.Linear);
    }

    public void Destroy()
    {
      _loadingTween.Kill();
      _loadingView.gameObject.SetActive(false);
    }
  }
}
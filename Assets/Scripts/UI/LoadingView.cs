using UnityEngine;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour
{
    [SerializeField] private Image _loadingImage;

    public Image LoadingImage => _loadingImage;
}

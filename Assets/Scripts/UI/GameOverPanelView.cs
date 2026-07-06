using DG.Tweening;
using UnityEngine;

public class GameOverPanelView : MonoBehaviour
{
    void OnEnable()
    {
        transform.localScale = Vector3.one * 0.5f;

        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutCubic);
    }
}

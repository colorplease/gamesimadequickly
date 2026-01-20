using UnityEngine;
using TMPro;
using DG.Tweening;

public class ThoughtTextObject : MonoBehaviour
{
    public TMP_Text textComponent;
    [SerializeField] float fadeOpacity;

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    public void FadeTextIn()
    {
        textComponent.DOFade(fadeOpacity, 0.25f).SetEase(Ease.OutExpo);
    }

    public void FadeTextOut()
    {
        textComponent.DOFade(0, 0.25f).SetEase(Ease.InExpo);
    }
}

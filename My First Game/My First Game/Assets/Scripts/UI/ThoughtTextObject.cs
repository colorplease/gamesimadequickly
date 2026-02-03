using UnityEngine;
using TMPro;
using DG.Tweening;

public class ThoughtTextObject : MonoBehaviour
{
    [Header("Generic ThoughtTextObject Stuff")]
    public TMP_Text textComponent;
    [SerializeField] float fadeOpacity;

    [Header("PastThoughtTextObject Stuff")]
    [SerializeField] bool isPast;
    [SerializeField] Eyechecker eyechecker;
    Tween typeWriterTween;
    bool isPlaying = false;
    public bool hasBeenRead = false;

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        eyechecker = Eyechecker.instance;
    }

    void Update()
    {
        if(isPast && !hasBeenRead)
        {
            if(eyechecker.lookingAtPastPortal)
            {
                if(!isPlaying)
                {
                    typeWriterTween.Play();
                    isPlaying = true;
                }
            }
            else
            {
                if(isPlaying)
                {
                    typeWriterTween.Pause();
                    isPlaying = false;
                }
                
            }
        }
    }

    public void FadeTextIn()
    {
        textComponent.DOFade(fadeOpacity, 0.25f).SetEase(Ease.OutExpo);
    }

    public void FadeTextOut()
    {
        textComponent.DOFade(0, 0.25f).SetEase(Ease.InExpo).OnComplete(() => {
            textComponent.SetText("");
        });
        
    }

    public void SetTextTypeWriter(string text)
    {
        hasBeenRead = false;
        string typeWriterText = "";
        typeWriterTween = DOTween.To(() => typeWriterText, x => typeWriterText = x, text, text.Length * 0.15f).OnUpdate(() => {
            // print("Setting text: " + text);
            textComponent.text = typeWriterText;
        });
        typeWriterTween.Pause();
        typeWriterTween.OnComplete(() => {
            hasBeenRead = true;
            ThoughtManager.instance.PresentPastThought();
            ThoughtManager.instance.RestorePlayerMovement();
        });
    }
}

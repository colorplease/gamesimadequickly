using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;

    public bool enableBeginningCutscene = true;
    public Animator beginningCutsceneHolder;
    public bool isDoneWithBeginningCutscene = false;
    [Header("Beginning Cutscene")]
    public GameObject godTextPrefab;
    public GameObject meTextPrefab;
    public Transform cutsceneTextHolder;
    public Image backgroundImage;

    public TextMeshProUGUI currentText;
    public float timeBeforeFadeOnComplete;
    public float fadingTimeBeforeDisappearance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if(enableBeginningCutscene)
        {
            beginningCutsceneHolder.SetTrigger("Start");
            backgroundImage.enabled = true;
            ThoughtManager.instance.RestrictPlayerMovement();
            PlayerMovement.instance.footstepSoundEnabled = false;
            PlayerLook.instance.inControl = false;
        }
        else
        {
            StartFirstChapter();
        }
    }
    
    public void SummonGodText(string text)
    {
        TextMeshProUGUI textObject;
        textObject = Instantiate(godTextPrefab, cutsceneTextHolder).GetComponent<TextMeshProUGUI>();
        textObject.SetText(text);
        currentText = textObject;

    }
    public void SummonMeText(string text)
    {
        TextMeshProUGUI textObject;
        textObject = Instantiate(meTextPrefab, cutsceneTextHolder).GetComponent<TextMeshProUGUI>();
        textObject.SetText(text);
        currentText = textObject;
    }
    
    public void EditCurrentText(string text)
    {
        currentText.SetText(text);
    }

    public void MarkCurrentTextComplete()
    {
        TextMeshProUGUI actualCurrentText = currentText;
        actualCurrentText.DOFade(1, timeBeforeFadeOnComplete).SetEase(Ease.InOutSine).OnComplete(() => actualCurrentText.DOFade(0, fadingTimeBeforeDisappearance).SetEase(Ease.InOutSine));
    }

    public void MarkAllTextComplete()
    {
        foreach(TextMeshProUGUI text in cutsceneTextHolder.GetComponentsInChildren<TextMeshProUGUI>())
        {
            text.DOFade(1, timeBeforeFadeOnComplete).SetEase(Ease.InOutSine).OnComplete(() => text.DOFade(0, fadingTimeBeforeDisappearance).SetEase(Ease.InOutSine));
        }
    }

    public void DestroyAllText()
    {
        foreach(TextMeshProUGUI text in cutsceneTextHolder.GetComponentsInChildren<TextMeshProUGUI>())
        {
            Destroy(text.gameObject);
        }
    }

    public void StartFirstChapter()
    {
        AudioManager.instance.StartFirstChapter();
        backgroundImage.DOFade(0, 3.5f).SetEase(Ease.InOutSine);
        ThoughtManager.instance.RestorePlayerMovement();
        PlayerLook.instance.inControl = true;
        ThoughtManager.instance.StartTutorial();
    }

}

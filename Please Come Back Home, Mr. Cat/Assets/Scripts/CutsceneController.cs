using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cutsceneTextReference;
    [SerializeField] private Image cutsceneImageReference;

    public event Action OnCutSceneCompleteAction;

    public void CallCutscene(Cutscene[] cutscenes)
    {
        StartCoroutine(ShowCutscene(cutscenes));
    }

    IEnumerator ShowCutscene(Cutscene[] cutscenes)
    {
        foreach (Cutscene cutscene in cutscenes)
        {
            cutsceneTextReference.text = cutscene.cutsceneText;
            cutsceneImageReference.sprite = cutscene.cutsceneImage;
            yield return new WaitForSeconds((cutsceneTextReference.text.Length * (0.077f)) + cutscene.cutsceneDelay);
        }
        OnCutSceneComplete();
    }

    public void OnCutSceneComplete()
    {
        cutsceneTextReference.enabled = false;
        cutsceneImageReference.enabled = false;
        OnCutSceneCompleteAction?.Invoke();
    }
}

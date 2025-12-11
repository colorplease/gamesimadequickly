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

    public bool isInteractable = false;
    bool isCutscenePlaying = false;
    int currentCutsceneIndex = 0;
    Cutscene[] cutscenes;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isInteractable && isCutscenePlaying)
            {
                currentCutsceneIndex++;
                if (currentCutsceneIndex < cutscenes.Length)
                {
                    cutsceneTextReference.text = cutscenes[currentCutsceneIndex].cutsceneText;
                    cutsceneImageReference.sprite = cutscenes[currentCutsceneIndex].cutsceneImage;
                }
                else
                {
                    OnCutSceneComplete();
                }
            }
        }
    }

    public void CallCutscene(Cutscene[] cutscenes)
    {
        if (isInteractable && !isCutscenePlaying)
        {
            this.cutscenes = cutscenes;
            if(!cutsceneTextReference.enabled)
            {
                cutsceneTextReference.enabled = true;
            }
            if(!cutsceneImageReference.enabled)
            {
                cutsceneImageReference.enabled = true;
            }
            cutsceneTextReference.text = cutscenes[0].cutsceneText;
            cutsceneImageReference.sprite = cutscenes[0].cutsceneImage;
            isCutscenePlaying = true;
        }
        else
        {
            this.cutscenes = cutscenes;
            isCutscenePlaying = true;
            StartCoroutine(ShowCutscene(cutscenes));
        }
        
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
        isCutscenePlaying = false;
        currentCutsceneIndex = 0;
        OnCutSceneCompleteAction?.Invoke();
    }
}

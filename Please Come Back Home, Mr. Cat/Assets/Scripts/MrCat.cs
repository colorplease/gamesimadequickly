using UnityEngine;

public class MrCat : MonoBehaviour
{
    [SerializeField] private CutsceneController cutsceneController;
    [SerializeField] private Cutscene[] beginningCutscene;

    void Awake()
    {
        StartCutscene(beginningCutscene);
    }

    void StartCutscene(Cutscene[] cutscene)
    {
        cutsceneController.CallCutscene(cutscene);
        cutsceneController.OnCutSceneCompleteAction += CutSceneEnded;
    }

    void CutSceneEnded()
    {
        Debug.Log("Cutscene ended");
        cutsceneController.OnCutSceneCompleteAction -= CutSceneEnded;
    }


}

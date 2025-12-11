using UnityEngine;

public class MrCat : MonoBehaviour
{
    [Header("Beginning Cutscene")]
    [SerializeField] private CutsceneController cutsceneController;
    [SerializeField] private Cutscene[] beginningCutscene;

    public bool disableFirstCutscene = false;

    [Header("Box Management")]
    [SerializeField] private GameObject boxPrefab;
    public int correctBoxes = 0;

    void Awake()
    {
        if (!disableFirstCutscene)
        {
            StartCutscene(beginningCutscene);
        }
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

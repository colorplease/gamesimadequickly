using UnityEngine;
using System.Collections.Generic;

public class MrCat : MonoBehaviour
{
    [Header("Beginning Cutscene")]
    [SerializeField] private CutsceneController cutsceneController;
    [SerializeField] private Cutscene[] beginningCutscene;

    public bool disableFirstCutscene = false;

    [Header("Box Management")]
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject boxSlotPrefab;
    [SerializeField] private Transform boxSlotParentOrganizer;
    [SerializeField] private Transform boxSpawnTransform;

    public int correctBoxes = 0;
    public int requiredBoxes;
    public int currentLevel = 0;
    public List<int[]> levelBoxes;

    [Header("Box Dependencies")]
    [SerializeField] private Canvas canvasDependency;




    void Awake()
    {
        BoxLevelInitialize();
        if (!disableFirstCutscene)
        {
            StartCutscene(beginningCutscene);
        }
        else
        {
            LoadLevel();
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
        LoadLevel();
    }

    void BoxLevelInitialize()
    {
        //box ids: 0: rocks, 1: bells, 
        Debug.Log("Box Level Initialize");
        levelBoxes = new List<int[]>();
        levelBoxes.Add(new int[] { 1, 0});
        levelBoxes.Add(new int[] { 0, 1});
    }

    void LoadLevel()
    {
        requiredBoxes = levelBoxes[currentLevel].Length;
        for (int i = 0; i < requiredBoxes; i++)
        {
            GameObject boxSlot = Instantiate(boxSlotPrefab, Vector3.zero, Quaternion.identity, boxSlotParentOrganizer);
            BoxSlot boxSlotComponent = boxSlot.GetComponent<BoxSlot>();
            boxSlotComponent.slotID= levelBoxes[currentLevel][i];
            boxSlotComponent.mrCat = this;
            GameObject box = Instantiate(boxPrefab, new Vector3(boxSpawnTransform.position.x + Random.Range(-1000, 1000), boxSpawnTransform.position.y + Random.Range(-250, 250), 0), Quaternion.identity, boxSpawnTransform);
            Box boxComponent = box.GetComponent<Box>();
            boxComponent.boxID = levelBoxes[currentLevel][i];
            boxComponent.canvas = canvasDependency;
        }
        currentLevel++;
    }

    


}

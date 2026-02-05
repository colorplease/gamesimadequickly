using UnityEngine;
using DG.Tweening;

public class SpecialEventManager : MonoBehaviour
{
    public static SpecialEventManager instance;

    [Header("Moving Portals and Text")]
    [SerializeField] Eyechecker eyechecker;
    [SerializeField] Transform playerFuturePortal;
    [SerializeField] Transform playerPastPortal;
    [SerializeField] Transform presentPastTextTransform;

    [Header("Special Events")]
    bool readyToExecuteSpecialEvent = false;
    Sequence specialEventSequence;
    
    [Header("Ending")]
    [SerializeField] LayerMask endingLayerMask;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        eyechecker = Eyechecker.instance;
    }

    public void SpecialEventCall(int specialEvent)
    {
        switch(specialEvent)
        {
            //case 1: “My First Reminscience” (have the past inch closer to your player character)
            //case 2: “My Last Lecture”  (have the past inch closer to your player character) //-1.643 //0.99
            //case 3: “My Last Dorm Bathroom” (present and future go further and further away, past engulfs character)
            //actually...case 3 should happen at like maybe the end of chapter 4 or the beginning of chapter 5.
            case 1:
                //-2.36 for actual portal
                //0.36 for present past
                specialEventSequence = DOTween.Sequence();
                specialEventSequence.Append(playerPastPortal.DOLocalMoveZ(-2.36f, 1f).SetEase(Ease.InOutSine));
                specialEventSequence.Append(presentPastTextTransform.DOLocalMoveZ(0.36f, 1f).SetEase(Ease.InOutSine).OnComplete(() => specialEventSequence = null));
                specialEventSequence.Pause();
                break;
            case 2:
                specialEventSequence = DOTween.Sequence();
                specialEventSequence.Append(playerPastPortal.DOLocalMoveZ(-1.643f, 1f).SetEase(Ease.InOutSine));
                specialEventSequence.Append(presentPastTextTransform.DOLocalMoveZ(0.99f, 1f).SetEase(Ease.InOutSine).OnComplete(() => specialEventSequence = null));
                specialEventSequence.Pause();
                break;
            case 3:
            PortalSwapManager.instance.chapterPortalCameraData.SetRenderer(2);
            PortalSwapManager.instance.chapterPortalCamera.cullingMask = endingLayerMask;
            break;
        }
    }

    void Update()
    {
        if(specialEventSequence != null)
        {
            if(!specialEventSequence.IsPlaying() && eyechecker.lookingAtPastPortal)
            {
                specialEventSequence.Play();
            }
        }
        
    }
}

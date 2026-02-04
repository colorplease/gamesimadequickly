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
            //case 2: “My Last Lecture”  (have the past inch closer to your player character)
            //case 3: “My Last Dorm Bathroom” (present and future go further and further away, past engulfs character)
            case 1:
                //-2.36 for actual portal
                //0.36 for present past
                specialEventSequence = DOTween.Sequence();
                specialEventSequence.Append(playerPastPortal.DOLocalMoveZ(-2.36f, 1f).SetEase(Ease.InOutSine));
                specialEventSequence.Append(presentPastTextTransform.DOLocalMoveZ(0.36f, 1f).SetEase(Ease.InOutSine).OnComplete(() => specialEventSequence = null));
                specialEventSequence.Pause();
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

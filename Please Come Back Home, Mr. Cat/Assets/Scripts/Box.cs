using UnityEngine;
using UnityEngine.EventSystems;

public class Box : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    bool isDragging = false;
    Vector2 lastPosition;

    public AudioClip[] boxSounds;
    public int boxID; //0 = rocks 1 = bells 2 = leaves 3 = misc
    int secondaryID;

    public AudioSource mainSound;
    public AudioSource secondarySound;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        lastPosition = rectTransform.anchoredPosition;
        secondaryID = Random.Range(0, 3);
        mainSound.clip = boxSounds[boxID];
        secondarySound.clip = boxSounds[secondaryID];
        mainSound.volume = 0;
        secondarySound.volume = 0;
        mainSound.Play();
        secondarySound.Play();
    }

    void Update()
    {
        float distance = (lastPosition - rectTransform.anchoredPosition).magnitude;
        if (isDragging)
        {
            if (distance > 0.001f && !isDragging)
            {
                if(mainSound.volume <= 0)
                {
                    mainSound.volume = 0.5f;
                }
                if(secondarySound.volume <= 0)
                {
                    secondarySound.volume = 0.1f;
                }
            }
            else
            {
                if(mainSound.volume > 0)
                {
                    mainSound.volume = 0;
                }
                if(secondarySound.volume > 0)
                {
                    secondarySound.volume = 0;
                }
            }
        }
        else
        {
            if(mainSound.volume > 0)
            {
                mainSound.volume = 0;
            }
            if(secondarySound.volume > 0)
            {
                secondarySound.volume = 0;
            }
        }
        
        lastPosition = rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.blocksRaycasts = false;
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        Vector2 newPosition = rectTransform.anchoredPosition + eventData.delta / canvas.scaleFactor;
        newPosition.x = Mathf.Clamp(newPosition.x, -866, 866);
        newPosition.y = Mathf.Clamp(newPosition.y, -442, 442);
        rectTransform.anchoredPosition = newPosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
    }
}

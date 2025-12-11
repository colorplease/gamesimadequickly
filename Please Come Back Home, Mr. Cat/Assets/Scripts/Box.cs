using UnityEngine;
using UnityEngine.EventSystems;

public class Box : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    [SerializeField] bool isDragging = false;
    Vector2 lastPosition;
    public int boxID; //0 = rocks 1 = bells 2 = leaves 3 = misc

    public AudioSource mainSound;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        lastPosition = rectTransform.anchoredPosition; 
        mainSound.volume = 0;
        mainSound.Play();
    }

    void FixedUpdate()
    {
        float distance = (lastPosition - rectTransform.anchoredPosition).magnitude;
        if (isDragging)
        {
            mainSound.volume = 0.5f;
        }
        else
        {
            mainSound.volume = 0;
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

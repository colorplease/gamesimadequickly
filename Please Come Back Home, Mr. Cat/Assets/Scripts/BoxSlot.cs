using UnityEngine;
using UnityEngine.EventSystems;

public class BoxSlot : MonoBehaviour, IDropHandler
{
    public int slotID;
    public MrCat mrCat;
    public Vector3 snappingOffset;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().localPosition = GetComponent<RectTransform>().localPosition + snappingOffset;
            if (eventData.pointerDrag.GetComponent<Box>().boxID == slotID)
            {
                mrCat.correctBoxes++;
                eventData.pointerDrag.GetComponent<Box>().enabled = false;
            }
        }
    }
}

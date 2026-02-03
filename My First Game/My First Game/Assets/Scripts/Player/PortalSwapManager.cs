using UnityEngine;

public class PortalSwapManager : MonoBehaviour
{
    public static PortalSwapManager instance;
    [Header("Swapping Portals")]
    [SerializeField] Material futureOutlineShaderMaterial;
    [SerializeField] Material chapterOutlineShaderMaterial;
    public Color newOutlineColor;
    float outlineWidth = 3;

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
        newOutlineColor = Color.black;
        futureOutlineShaderMaterial.SetColor("_OutlineColor", newOutlineColor);
        futureOutlineShaderMaterial.SetFloat("_Thickness", outlineWidth);
        chapterOutlineShaderMaterial.SetColor("_OutlineColor", newOutlineColor + new Color(0.19f, 0.19f, 0.19f, 1));
        chapterOutlineShaderMaterial.SetFloat("_Thickness", outlineWidth - 0.5f);

    }

    public void SwapPortals(Transform newFuturePortal)
    {
        // Destroy(GameObject.FindGameObjectWithTag("futurePortal"));
        // newFuturePortal.tag = "futurePortal";
        // newFuturePortal.parent = futurePortalHolder;
        // newFuturePortal.localPosition = Vector3.zero;
        // newFuturePortal.localRotation = Quaternion.identity;
        newOutlineColor = new Color(newOutlineColor.r + 0.19f, newOutlineColor.g + 0.19f, newOutlineColor.b + 0.19f, 1);
        futureOutlineShaderMaterial.SetColor("_OutlineColor", newOutlineColor);
        outlineWidth -= 0.5f;
        futureOutlineShaderMaterial.SetFloat("_Thickness", outlineWidth);
        chapterOutlineShaderMaterial.SetColor("_OutlineColor", newOutlineColor + new Color(0.19f, 0.19f, 0.19f, 1));
        chapterOutlineShaderMaterial.SetFloat("Thickness", outlineWidth - 0.5f);
    }
}

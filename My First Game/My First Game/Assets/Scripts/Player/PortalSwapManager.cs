using UnityEngine;

public class PortalSwapManager : MonoBehaviour
{
    public static PortalSwapManager instance;
    [Header("Swapping Portals")]
    [SerializeField] Material futureOutlineShaderMaterial;
    [SerializeField] Material chapterOutlineShaderMaterial;
    public Color newOutlineColor;
    [SerializeField] float outlineWidth = 2;
    [Header("Moving Portals")]
    [SerializeField] GameObject chapterPortalObject;
    [SerializeField] Transform[] chapterObjectHolders;
    [SerializeField] float[] chapterPortalLocalPositions;
    [Header("Portal Upkeep")]
    [SerializeField] RenderTexture chapterPortalRenderTexture;
    public Camera chapterPortalCamera;
    public UnityEngine.Rendering.Universal.UniversalAdditionalCameraData chapterPortalCameraData;

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
        chapterOutlineShaderMaterial.SetFloat("_Thickness", outlineWidth - 0.3f);
        chapterPortalCameraData = chapterPortalCamera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();

    }

    public void SetPortalPosition()
    {
        chapterPortalObject.transform.parent = chapterObjectHolders[AudioManager.instance.currentChapter];
        chapterPortalObject.transform.localPosition = new Vector3(chapterPortalObject.transform.localPosition.x, chapterPortalObject.transform.localPosition.y, chapterPortalLocalPositions[AudioManager.instance.currentChapter]);
        chapterPortalObject.GetComponent<MeshRenderer>().enabled = false;
        chapterPortalCamera.targetTexture = chapterPortalRenderTexture;
    }

    public void EnablePortalVisibility()
    {
        PortalTextureSetup.instance.PortalSwapUpdate();
        chapterPortalObject.GetComponent<MeshRenderer>().enabled = true;
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
        outlineWidth -= 0.3f;
        futureOutlineShaderMaterial.SetFloat("_Thickness", outlineWidth);
        chapterOutlineShaderMaterial.SetColor("_OutlineColor", newOutlineColor + new Color(0.19f, 0.19f, 0.19f, 1));
        chapterOutlineShaderMaterial.SetFloat("_Thickness", outlineWidth - 0.3f);
    }
}

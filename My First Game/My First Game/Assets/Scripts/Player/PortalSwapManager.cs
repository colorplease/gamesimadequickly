using UnityEngine;

public class PortalSwapManager : MonoBehaviour
{
    public static PortalSwapManager instance;
    [Header("Swapping Portals")]
    public Material[] chapterMaterials;
    [SerializeField] PortalTextureSetup portalTextureSetup;
    
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

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "chapterPortal")
        {
            portalTextureSetup.cameraMatFuture = chapterMaterials[AudioManager.instance.currentChapter];
        }
    }
}

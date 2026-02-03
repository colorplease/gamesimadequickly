using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    public Camera cameraPast;
    public Material cameraMatPast;

    public Camera cameraFuture;
    public Material cameraMatFuture;

    public Camera cameraChapter;
    public Material cameraMatChapter;  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(cameraPast.targetTexture != null){
            cameraPast.targetTexture.Release();
        }
        cameraPast.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatPast.mainTexture = cameraPast.targetTexture;

        if(cameraFuture.targetTexture != null){
            cameraFuture.targetTexture.Release();
        }
        cameraFuture.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatFuture.mainTexture = cameraFuture.targetTexture;

        if(cameraChapter.targetTexture != null){
            cameraChapter.targetTexture.Release();
        }
        cameraChapter.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatChapter.mainTexture = cameraChapter.targetTexture;
    }

    public void PortalSwapUpdate()
    {
        if(cameraFuture.targetTexture != null){
            cameraFuture.targetTexture.Release();
        }
        cameraFuture.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatFuture.mainTexture = cameraFuture.targetTexture;

        if(cameraChapter.targetTexture != null){
            cameraChapter.targetTexture.Release();
        }
        cameraChapter.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatChapter.mainTexture = cameraChapter.targetTexture;
    }
}

using UnityEngine;
using TMPro;

public class WobblyText : MonoBehaviour
{
        public TMP_Text textComponent;


    // Update is called once per frame
    void Update()
    {
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++) { 
            var charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible) {
                continue;
            
            }
            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;


            for (int j = 0; j < 4; ++j) {
                var orig = verts[charInfo.vertexIndex + j];
                var noise = Mathf.PerlinNoise(Time.time*1f + orig.x*5f, 0) * 20f;
                var noise2 = Mathf.PerlinNoise(Time.time*1f + orig.y*5f, 0) * 20f;
                verts[charInfo.vertexIndex + j] = orig + new Vector3(noise, noise2, 0);
        
            }
        }
         for (int i = 0; i < textInfo.meshInfo.Length; ++i){
         
             var meshInfo = textInfo.meshInfo[i];  
             meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        
        }



    }

}

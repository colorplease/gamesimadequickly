using UnityEngine;
using TMPro;

public class ThoughtManager : MonoBehaviour
{
    //manages "sweeping" of thought hitboxes
    //manages the UI of thoughts

    [Header("Thought TextMeshProUGUI")]
    [SerializeField] TextMeshProUGUI thoughtPastText;

    // Update is called once per frame
    void Update()
    {
        Material mat = thoughtPastText.material;
        mat.EnableKeyword("OUTLINE_ON");
        mat.SetFloat("_OutlineWidth", 11f);
        mat.SetColor("_OutlineColor", Color.black);
        thoughtPastText.UpdateMeshPadding();
    }
}

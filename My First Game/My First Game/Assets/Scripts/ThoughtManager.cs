using UnityEngine;
using TMPro;

public class ThoughtManager : MonoBehaviour
{
    //manages "sweeping" of thought hitboxes
    //manages the UI of thoughts

    [Header("Thought TextMeshProUGUI")]
    [SerializeField] TextMeshProUGUI thoughtPastText;
    [SerializeField] TextMeshProUGUI thoughtFutureText;
    [SerializeField] TextMeshProUGUI thoughtFutureBG;

    void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Sweeper IN FRONT":
                thoughtFutureText.SetText(other.gameObject.GetComponent<Thought>().futureThought);
                thoughtFutureBG.SetText("<mark=#ffffff82>" + other.gameObject.GetComponent<Thought>().futureThought + "</mark>");
                break;
            case "Sweeper ON THE OBJECT":
                thoughtFutureText.SetText("");
                thoughtPastText.SetText("");
                thoughtFutureBG.SetText("");
                break;
            case "Sweeper BEHIND":
                thoughtPastText.SetText(other.gameObject.GetComponent<Thought>().pastThought);
                break;
        }
    }
}

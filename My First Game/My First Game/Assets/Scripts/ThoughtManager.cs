using UnityEngine;
using TMPro;

public class ThoughtManager : MonoBehaviour
{
    //manages "sweeping" of thought hitboxes
    //manages the UI of thoughts

    [Header("Thought TextMeshProUGUI")]
    [SerializeField] ThoughtTextObject thoughtPastText;
    [SerializeField] ThoughtTextObject thoughtFutureText;
    [SerializeField] ThoughtTextObject thoughtFutureBG;

    void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Sweeper IN FRONT":
                thoughtFutureText.textComponent.text = other.gameObject.GetComponent<Thought>().futureThought;
                thoughtFutureBG.textComponent.text = "<mark=#ffffff82>" + other.gameObject.GetComponent<Thought>().futureThought + "</mark>";
                thoughtFutureText.FadeTextIn();
                thoughtFutureBG.FadeTextIn();

                break;
            case "Sweeper ON THE OBJECT":
                thoughtFutureText.textComponent.text = "";
                thoughtPastText.textComponent.text = "";
                thoughtFutureBG.textComponent.text = "";
                thoughtFutureText.FadeTextOut();
                thoughtFutureBG.FadeTextOut();
                thoughtPastText.FadeTextOut();
                break;
            case "Sweeper BEHIND":
                thoughtPastText.textComponent.text = other.gameObject.GetComponent<Thought>().pastThought;
                thoughtPastText.FadeTextIn();
                break;
        }
    }
}

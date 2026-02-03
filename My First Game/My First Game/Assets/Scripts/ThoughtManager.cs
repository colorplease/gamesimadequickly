using UnityEngine;
using System.Collections;
using TMPro;

public class ThoughtManager : MonoBehaviour
{
    //manages "sweeping" of thought hitboxes
    //manages the UI of thoughts

    public static ThoughtManager instance;

    [Header("Thought TextMeshProUGUI")]
    [SerializeField] ThoughtTextObject thoughtPastText;
    [SerializeField] ThoughtTextObject thoughtFutureText;
    [SerializeField] ThoughtTextObject thoughtFutureBG;
    [SerializeField] ThoughtTextObject thoughtPresentFutureText;
    [SerializeField] ThoughtTextObject thoughtPresentPastText;

    [Header("Read Check")]
    bool firstReadCheckBypass;
    [SerializeField] ThoughtTextObject pastThoughtTextObject;
    Coroutine readCheckCoroutine;

    [Header("Present Past Thoughts")]
    string presentPastThoughtString;

    [Header("Chapter Barrier")]
    bool listeningForNextChapter = false;

    [Header("Color Changes")]
    [SerializeField] MeshRenderer[] lighterMeshes;
    [SerializeField] MeshRenderer[] darkerMeshes;
    
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
                thoughtFutureText.FadeTextOut();
                thoughtFutureBG.FadeTextOut();
                thoughtPastText.FadeTextOut();
                thoughtPresentPastText.FadeTextOut();
                break;
            case "Sweeper BEHIND":
                thoughtPastText.SetTextTypeWriter(other.gameObject.GetComponent<Thought>().pastThought);
                presentPastThoughtString = other.gameObject.GetComponent<Thought>().presentPastThought;
                thoughtPastText.FadeTextIn();
                if(readCheckCoroutine == null)
                {
                    readCheckCoroutine = StartCoroutine(ReadCheck());
                }
                thoughtPastText.textComponent.color = other.gameObject.GetComponent<Thought>().pastTextColor;
                if(other.gameObject.GetComponent<Thought>().chapterBarrier)
                {
                    if(AudioManager.instance.readyForNextChapter)
                    {
                        listeningForNextChapter = false;
                    }
                    else
                    {
                        listeningForNextChapter = true;
                    }
                }
                foreach(MeshRenderer mesh in lighterMeshes)
                {
                    mesh.material = other.gameObject.GetComponent<Thought>().motherColor;
                }
                foreach(MeshRenderer mesh in darkerMeshes)
                {
                    mesh.material = other.gameObject.GetComponent<Thought>().motherColorDarker;
                }
                break;
            case "chapterPortal":
                AudioManager.instance.BeginNextChapter();
                listeningForNextChapter = false;
                PortalSwapManager.instance.SwapPortals(other.gameObject.transform);
                print("chapterPortal");
            break;
        }
    }

    void Update()
    {
        if(listeningForNextChapter)
        {
            if(AudioManager.instance.readyForNextChapter)
            {
                listeningForNextChapter = false;
                RestorePlayerMovement();
            }
        }
    }

    IEnumerator ReadCheck()
    {
        if(!firstReadCheckBypass)
        {
            yield return new WaitForSeconds(0.25f);
            if(!pastThoughtTextObject.hasBeenRead)
            {
                RestrictPlayerMovement();
            }
            yield return new WaitForSeconds(3f);
            print("setting text");
            thoughtPresentFutureText.textComponent.text = "i can't leave that behind yet.";
            thoughtPresentFutureText.FadeTextIn();
            yield return new WaitForSeconds(5f);
            thoughtPresentFutureText.textComponent.text = "i have to turn around.";
        }
        else
        {
            firstReadCheckBypass = false;
        }
    }

    public void PresentPastThought()
    {
        thoughtPresentPastText.textComponent.text = presentPastThoughtString;
        thoughtPresentPastText.FadeTextIn();
    }

    public void RestrictPlayerMovement()
    {
        PlayerMovement.instance.moveSpeed = 0;
    }

    public void RestorePlayerMovement()
    {
        if(!listeningForNextChapter)
        {
            if(readCheckCoroutine != null)
            {
                StopCoroutine(readCheckCoroutine);
                readCheckCoroutine = null;
            }
            readCheckCoroutine = null;
            PlayerMovement.instance.moveSpeed = 6;
            thoughtPresentFutureText.FadeTextOut();
        }
        
    }
}

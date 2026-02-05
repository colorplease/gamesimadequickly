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
            // sets the future text and BG for future text and then fades in both.
                thoughtFutureText.textComponent.text = other.gameObject.GetComponent<Thought>().futureThought;
                thoughtFutureBG.textComponent.text = "<mark=#ffffff82>" + other.gameObject.GetComponent<Thought>().futureThought + "</mark>";
                thoughtFutureText.FadeTextIn();
                thoughtFutureBG.FadeTextIn();

                break;
            case "Sweeper ON THE OBJECT":
            // fades out all text and BGs to prepare for new text.
                thoughtFutureText.FadeTextOut();
                thoughtFutureBG.FadeTextOut();
                thoughtPastText.FadeTextOut();
                thoughtPresentPastText.FadeTextOut();
                break;
            case "Sweeper BEHIND":
            //sets the past text to begin typewriting
            //sets present past text annd fades it inn
            //begins the readcheckcoroutine for the player before they get that text in front of them that says "I can't leave that behind yet."
            //additionally handles the chapter barrier check, meaning that if the player isn't ready for the next chapter, the player will be restricted from moving.
            //also handles the color changes for the enviroment on the Thought object with the mother color and mother color darker.
            //handles special event checks and executes them if they are triggered.
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
                SpecialEventManager.instance.SpecialEventCall(other.gameObject.GetComponent<Thought>().specialEvent);
            break;
            case "chapterPortal":
            //deadass just starts the next chapter and moves the portal to the new chapter.
                AudioManager.instance.BeginNextChapter();
                listeningForNextChapter = false;
                PortalSwapManager.instance.SwapPortals(other.gameObject.transform);
                print("chapterPortal");
            break;
            case "ENDING":
            CutsceneManager.instance.StartEndingCutscene();
            break;
        }
    }

    void Update()
    {
        //checks if the player is ready for the next chapter and if so, restores their movement and clears the listeningForNextChapter flag.
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
        //checks if the player has read the past text and if not, restricts their movement and displays the "I can't leave that behind yet." text.
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

    public void StartTutorial()
    {
        //displays the tutorial text and fades it in.
        thoughtPresentFutureText.textComponent.text = "press w to keep moving forward.";
        thoughtPresentFutureText.FadeTextIn();
    }

    public void StopTutorial()
    {
        //fades out the tutorial text.
        thoughtPresentFutureText.FadeTextOut();
    }

    public void PresentPastThought()
    {
        //sets the present past text to the present past thought string and fades it in.
        thoughtPresentPastText.textComponent.text = presentPastThoughtString;
        thoughtPresentPastText.FadeTextIn();
    }

    public void RestrictPlayerMovement()
    {
        //restricts the player's movement.
        PlayerMovement.instance.moveSpeed = 0;
    }

    public void RestorePlayerMovement()
    {
        //restores the player's movement and clears the readCheckCoroutine flag.
        if(!listeningForNextChapter)
        {
            if(readCheckCoroutine != null)
            {
                StopCoroutine(readCheckCoroutine);
                readCheckCoroutine = null;
            }
            readCheckCoroutine = null;
            PlayerMovement.instance.moveSpeed = 4.5f;
            if(CutsceneManager.instance.isDoneWithBeginningCutscene)
            {
                thoughtPresentFutureText.FadeTextOut();
            }
        }
        
    }
}

using UnityEngine;

public class Thought : MonoBehaviour
{
    public string futureThought;
    public string presentFutureThought;
    public string presentPastThought;
    [Header("PAST BARRIERS ONLY !!")]
    public string pastThought;
    public bool chapterBarrier;
    public int specialEvent = 0;
    public Material motherColor, motherColorDarker;
    public Color pastTextColor;
}

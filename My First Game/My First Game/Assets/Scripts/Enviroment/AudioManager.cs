using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("References")]
    public AudioSource musicSource;
    public AudioSource staticNoiseSource;

    [Header("Chapter Based Music")]
    public AudioClip[] chapterMusic;
    public int currentChapter = 0;
    public bool readyForNextChapter = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        StartFirstChapter(); 
    }

    // IEnumerator BeginNextChapter()
    // {
    //     musicSource.PlayOneShot(chapterMusic[currentChapter]);
    //     yield return new WaitForSecondsRealtime(chapterMusic[currentChapter].length - 0.05f);
    //     currentChapter++;
    //     musicSource.PlayOneShot(chapterMusic[currentChapter]);
    // }

    public void StartFirstChapter()
    {
        musicSource.PlayOneShot(chapterMusic[0]);
        currentChapter = 0;
        Invoke(nameof(NextChapterFlagSetter), chapterMusic[0].length - 0.05f);
    }

    public void NextChapterFlagSetter()
    {
        readyForNextChapter = true;
        staticNoiseSource.Play();
    }

    public void BeginNextChapter()
    {
        if (readyForNextChapter)
        {
            currentChapter++;
            musicSource.PlayOneShot(chapterMusic[currentChapter]);
            Invoke(nameof(NextChapterFlagSetter), chapterMusic[currentChapter].length - 0.05f);
            readyForNextChapter = false;
            staticNoiseSource.Stop();
        }
    }
}

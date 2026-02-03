using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Volume Settings")]
    public float masterVolume = 1f;

    [Header("References")]
    public AudioSource musicSource;
    public AudioSource staticNoiseSource;

    [Header("Chapter Based Music")]
    public AudioClip[] chapterMusic;
    public int currentChapter = 0;
    public bool readyForNextChapter = false;

    [Header("Dynamic Music Dimming")]
    Tween musicFadeLowPass;
    Tween musicFadeHighPass;
    bool isDoneDimming = false;
    AudioHighPassFilter musicHighPassFilter;
    AudioLowPassFilter musicLowPassFilter;
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
        musicHighPassFilter = musicSource.GetComponent<AudioHighPassFilter>();
        musicLowPassFilter = musicSource.GetComponent<AudioLowPassFilter>();
        StartFirstChapter(); 
        musicSource.volume *= masterVolume;
        staticNoiseSource.volume *= masterVolume;
    }

    void Update()
    {
        if(Eyechecker.instance.lookingAtPastPortal)
        {
            if(musicFadeLowPass == null && musicLowPassFilter.cutoffFrequency != 1500f)
            {
                musicFadeHighPass.Kill();
                musicFadeHighPass = null;
                DOTween.To(() => musicHighPassFilter.cutoffFrequency, x => musicHighPassFilter.cutoffFrequency = x, 10f, 1f).SetEase(Ease.InOutSine);
                musicFadeLowPass = DOTween.To(() => musicLowPassFilter.cutoffFrequency, x => musicLowPassFilter.cutoffFrequency = x, 1500f, 1f).SetEase(Ease.InOutSine);
            }
            
        }
        else if(Eyechecker.instance.lookingAtFuturePortal)
        {
            if(musicFadeHighPass == null && musicHighPassFilter.cutoffFrequency != 2000f)
            {
                musicFadeLowPass.Kill();
                musicFadeLowPass = null;
                DOTween.To(() => musicLowPassFilter.cutoffFrequency, x => musicLowPassFilter.cutoffFrequency = x, 22000f, 1f).SetEase(Ease.InOutSine);
                musicFadeHighPass = DOTween.To(() => musicHighPassFilter.cutoffFrequency, x => musicHighPassFilter.cutoffFrequency = x, 2000f, 1f).SetEase(Ease.InOutSine);
            }
            
        }
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
        print(chapterMusic[0].length);
    }

    public void NextChapterFlagSetter()
    {
        readyForNextChapter = true;
        staticNoiseSource.Play();
    }

    public void BeginNextChapter()
    {
        currentChapter++;
        // musicSource.PlayOneShot(chapterMusic[currentChapter]);
        // Invoke(nameof(NextChapterFlagSetter), chapterMusic[currentChapter].length - 0.05f);
        // readyForNextChapter = false;
        // staticNoiseSource.Stop();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager> {
    [SerializeField]
    private AudioSource musicSource=null;

    [SerializeField]
    private AudioSource sfxSource = null;

    [SerializeField]
    private Slider musicSlider=null;

    [SerializeField]
    private Slider sfxSlider=null;

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

	// Use this for initialization
	void Start () {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio") as AudioClip[];

        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }

        LoadVolum();
        musicSlider.onValueChanged.AddListener(delegate { UpdataVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdataVolume(); });
    }
	
	// Update is called once per frame
	    public void PlaySFX(string name)
    {
        sfxSource.PlayOneShot(audioClips[name]);
    }
    public void UpdataVolume()
    {
        musicSource.volume = musicSlider.value;

        sfxSource.volume = sfxSlider.value;

        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
        PlayerPrefs.SetFloat("Music", musicSlider.value);
    }
    public void LoadVolum()
    {

        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 0.5f);

        musicSource.volume = PlayerPrefs.GetFloat("Music", 0.5f);

        musicSlider.value = musicSource.volume;

        sfxSlider.value = sfxSource.volume;
    }
}

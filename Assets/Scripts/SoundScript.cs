using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundScript : MonoBehaviour {

	public Slider volumeSlider;
	public Image volumeIcon;
	public Sprite volumeOn;
	public Sprite volumeOff;
	public AudioClip updateSound;
	private AudioSource source;
	void Awake () {
		volumeSlider.value = PlayerPrefs.GetFloat ("volume", 0);
		updateVolume ();
	}

	public void updateVolume(){
		source = GetComponent<AudioSource> ();
		AudioListener.volume = volumeSlider.value;
		PlayerPrefs.SetFloat("volume", AudioListener.volume);
		if(!source.isPlaying)source.PlayOneShot (updateSound);
		if (volumeSlider.value <= 0.0f)
			volumeIcon.sprite = volumeOff;
		else
			volumeIcon.sprite = volumeOn;
	}
}

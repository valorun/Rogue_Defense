using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundScript : MonoBehaviour, IPointerUpHandler {

	private Slider volumeSlider;
	public Image volumeIcon;
	public Sprite volumeOn;
	public Sprite volumeOff;
	public AudioClip updateSound;
	public AudioSource source;
	void Awake () {
		volumeSlider = GetComponent<Slider>();
		volumeSlider.value = PlayerPrefs.GetFloat ("volume", 0);
		updateVolume ();
	}

	public void updateVolume(){
		//source = GetComponent<AudioSource> ();
		AudioListener.volume = volumeSlider.value;
		PlayerPrefs.SetFloat("volume", AudioListener.volume);
		if(!source.isPlaying)source.PlayOneShot (updateSound);
		if (volumeSlider.value <= 0.0f)
			volumeIcon.sprite = volumeOff;
		else
			volumeIcon.sprite = volumeOn;
	}
	 public void OnPointerUp(PointerEventData eventData){
		 updateVolume();
	}
}

using System.Collections;
using UnityEngine;

public class SoundFader : MonoBehaviour
{
	
	public void killAllCoroutines()
	{
		if (m_corMainFadeIn != null)
			StopCoroutine(m_corMainFadeIn);
		if (m_corHappyFadeOut != null)
			StopCoroutine(m_corHappyFadeOut);
		if (m_corMainFadeOut != null)
			StopCoroutine(m_corMainFadeOut);
		if (m_corHappyFadeIn != null)
			StopCoroutine(m_corHappyFadeIn);
		if (m_corVoodooFadeOut != null)
			StopCoroutine(m_corVoodooFadeOut);
		if (m_corVoodooFadeIn != null)
			StopCoroutine(m_corVoodooFadeIn);
	}
	public void FadeMainToHappy()
	{
		m_corMainFadeOut = FadeOut(m_main,2f, 0.2f);
		m_corHappyFadeIn = FadeIn(m_happy,2f, 0.9f);
		 
		killAllCoroutines();
	
		StartCoroutine(m_corMainFadeOut);
		StartCoroutine(m_corHappyFadeIn);
	}
	
	public void FadeHappyToMain()
	{
		
		m_corHappyFadeOut = FadeOut(m_happy,1f, 0f);
		m_corMainFadeIn = FadeIn(m_main,1f, 1f); 
		
		killAllCoroutines();
		
		StartCoroutine(m_corHappyFadeOut);
		StartCoroutine(m_corMainFadeIn);
	}
	
	public void FadeMainToVoodoo()
	{
		m_corMainFadeOut = FadeOut(m_main,2f, 0.4f);
		m_corVoodooFadeIn = FadeIn(m_voodoo,2f, 0.8f);
		 
		killAllCoroutines();
	
		StartCoroutine(m_corMainFadeOut);
		StartCoroutine(m_corVoodooFadeIn);
	}
	
	public void FadeVoodooToMain()
	{
		
		m_corVoodooFadeOut = FadeOut(m_voodoo,1f, 0f);
		m_corMainFadeIn = FadeIn(m_main,1f, 1f); 
		
		killAllCoroutines();
		
		StartCoroutine(m_corVoodooFadeOut);
		StartCoroutine(m_corMainFadeIn);
	}
	
	public static IEnumerator  FadeOut(AudioSource audioSource, float FadeTime, float threshold) {
        while (audioSource.volume > threshold) {
            audioSource.volume -= Time.deltaTime / FadeTime;
			yield return null;
        }
    }
	public static IEnumerator  FadeIn(AudioSource audioSource, float FadeTime, float threshold) {
		while (audioSource.volume < threshold) {
			audioSource.volume +=  Time.deltaTime / FadeTime;
			yield return null;
		}

	}
	
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public  AudioSource m_main;
	public  AudioSource m_voodoo;
	public AudioSource m_happy;
	private IEnumerator m_corMainFadeOut ;
	private IEnumerator m_corMainFadeIn ;
	private IEnumerator m_corVoodooFadeOut;
	private IEnumerator m_corVoodooFadeIn;
	private IEnumerator m_corHappyFadeOut;
	private IEnumerator m_corHappyFadeIn;
}


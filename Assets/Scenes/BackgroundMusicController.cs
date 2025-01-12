using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Obține componenta AudioSource
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.loop = true; // Asigură-te că sunetul este setat să ruleze în buclă
            audioSource.Play(); // Pornește sunetul de fundal
        }
        else
        {
            Debug.LogError("AudioSource nu este atașat la obiectul BackgroundMusic!");
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume; // Setează volumul
        }
    }
}

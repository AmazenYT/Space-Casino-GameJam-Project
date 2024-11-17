using UnityEngine;

public class PlaySoundOnPlayerCollision : MonoBehaviour
{
    public AudioClip soundEffect;  // The sound effect to play
    private AudioSource audioSource;  // The AudioSource component

    void Start()
    {
        // Get the AudioSource component attached to this object
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.clip = soundEffect;  // Set the sound effect clip
        }
    }

    // This method will be called when another collider enters this trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player object (or any object you specify) entered the trigger
        if (other.CompareTag("Player"))
        {
            // Play the sound effect
            if (audioSource != null && soundEffect != null)
            {
                audioSource.Play();
            }
        }
    }
}

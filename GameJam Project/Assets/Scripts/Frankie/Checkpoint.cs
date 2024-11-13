using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    KillPlayer killPlayer;
    public Transform respawnPoint;
    private void Awake()
    {
        killPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<KillPlayer>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            killPlayer.UpdateCheckpoint(respawnPoint.position);
        }
    }
}

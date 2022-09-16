using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource footstepSource;
    [SerializeField] AudioClip[] footstepClips;

    public void Footstep()
    {
        footstepSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
    }
}
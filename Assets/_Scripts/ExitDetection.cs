using UnityEngine;

public class ExitDetection : MonoBehaviour
{
    [SerializeField] Animator doorAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.PlayerExited();
            doorAnimator.SetBool("character_nearby", true);
        }
    }
}

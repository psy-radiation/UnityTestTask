
using UnityEngine;

public class RightZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = FindFirstObjectByType<MainGameScript>();
            if (controller != null)
                controller.SetPlayerInRightZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = FindFirstObjectByType<MainGameScript>();
            if (controller != null)
                controller.SetPlayerInRightZone(false);
        }
    }
}
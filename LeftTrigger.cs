
using UnityEditor;
using UnityEngine;



public class LeftZoneTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = FindFirstObjectByType<MainGameScript>();
            if (controller != null)
                controller.SetPlayerInLeftZone(true);
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = FindFirstObjectByType<MainGameScript>();
            if (controller != null)
                controller.SetPlayerInLeftZone(false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            for (int i = 0; i < other.transform.childCount; i++)
            {
                Transform child = other.transform.GetChild(i);
                if (child.CompareTag("Player"))
                {
                    child.SetParent(other.transform.parent);
                    break;
                }
            }
            Managers.Sound.PlayCubeDropSound(other.gameObject.transform);
            Destroy(other.gameObject);
            Managers.UI.UpdateCubeText();
        }
        // else if (other.gameObject.CompareTag("Player"))
        // {
        //     Destroy(other.gameObject);
        //     Managers.Game.GameOver();
        // }
    }
}

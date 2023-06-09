using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (Managers.Game.gameState == GameState.LevelPassed) return;
        if (other.transform.CompareTag("WallCube") || other.transform.CompareTag("FinishWall"))
        {
            if (Managers.Reference.player.parent == transform)
            {
                Managers.Reference.player.SetParent(transform.parent);
                Managers.Reference.player.GetComponentInChildren<Animator>().SetTrigger("Run");
            }

            transform.SetParent(Managers.Reference.worldCubes);
            Managers.UI.UpdateCubeText();
            Managers.Sound.PlayCubeDropSound(transform);
        }

        else if (other.transform.CompareTag("Finish"))
        {
            Debug.Log("Finish");
            Managers.Game.LevelPassed();
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        if (Managers.Game.gameState == GameState.LevelPassed) return;
        if (!transform.parent) return;

        if (transform.parent.CompareTag("PlayerCubes")) return; // if this is the collected cube, return (only check non collected cubes)

        if (other.gameObject.CompareTag("Cube") && other.transform.parent.CompareTag("PlayerCubes"))
        {
            // player cubes hit the cube
            Managers.Game.CollectCube(transform);
        }

        else if (other.gameObject.CompareTag("Player"))
        {
            // player hit the cube
            Managers.Game.CollectCube(transform);
        }
    }

}

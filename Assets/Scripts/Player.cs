using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        // transform.localRotation = Quaternion.identity;
        if (other.transform.CompareTag("WallCube"))
        {
            // Destroy(gameObject);
            Managers.Game.ResetPlayerModel();
            animator.SetTrigger("Die");
            Managers.Game.GameOver();
        }

        else if (other.transform.CompareTag("FinishWall"))
        {
            Managers.Game.LevelPassed();
        }

        else if (other.transform.CompareTag("Lava"))
        {
            Destroy(gameObject);
            Managers.Game.GameOver();
        }

        else if (other.transform.CompareTag("Platform"))
        {
            Managers.Game.ResetPlayerModel();
            animator.SetTrigger("Run");
        }

        else if (other.transform.CompareTag("Cube"))
        {
            Managers.Game.ResetPlayerModel();
            animator.SetTrigger("Idle");
        }
    }
}

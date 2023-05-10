using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Vector3 startPos = Vector3.zero;
    Vector3 touchStart;
    Vector3 touchRelative;
    Vector3 playerPosOnTouch;
    Vector3 camPosOnTouch;

    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {
        if (Managers.Game.gameState != GameState.Playing && Managers.Game.gameState != GameState.MainMenu) return;

        if (Managers.Game.gameState == GameState.Playing)
            MoveHorizontal();

        MoveForward();

        Managers.UI.UpdateCubeText();
    }

    private void MoveForward()
    {
        Vector3 moveVec = Vector3.forward * Time.deltaTime * Managers.Game.playerForwardSpeed;

        if (Managers.Game.gameState != GameState.MainMenu)
        {
            transform.Translate(moveVec);
            Managers.Game.Camera.transform.Translate(moveVec, Space.World);
        }
        else
        {
            Managers.Reference.worldCubes.Translate(-moveVec);
        }
    }

    private void MoveHorizontal()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
            playerPosOnTouch = transform.position;
            camPosOnTouch = Managers.Game.Camera.transform.position;
        }
        if (Input.GetMouseButton(0))
        {
            touchRelative = (Input.mousePosition - touchStart) / 100;
            touchRelative.x *= Managers.Game.playerHorizontalSpeed;

            if (touchRelative.x != 0)
            {
                int bounds = (int)(Managers.Game.platformWidth / 2);

                transform.position = new Vector3(
                    Mathf.Clamp(playerPosOnTouch.x + touchRelative.x, -bounds, +bounds),
                    transform.position.y,
                    transform.position.z
                );

                Managers.Game.Camera.transform.position = Vector3.Lerp(
                    Managers.Game.Camera.transform.position,
                    new Vector3(
                        Mathf.Clamp(camPosOnTouch.x + touchRelative.x / 10, -bounds, +bounds),
                        Managers.Game.Camera.transform.position.y,
                        Managers.Game.Camera.transform.position.z
                    ),
                    0.1f
                );
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    public Vector3 rotationSpeed = Vector3.zero;
    public float verticalSpeed = 0f;
    void Start()
    {
        Vector2 range = Managers.Game.diamondRotationSpeedRange;
        // set random rotation speed
        rotationSpeed = new Vector3(
            Random.Range(range.x, range.y),
            Random.Range(range.x, range.y),
            Random.Range(range.x, range.y)
        );

        // set random vertical speed
        verticalSpeed = Random.Range(10f, 12f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Managers.Game.gameState == GameState.Paused) return;

        transform.Rotate(rotationSpeed * Time.deltaTime);

        // move up and down with sin wave between height 0.0005f
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + Mathf.Sin(Time.time * verticalSpeed) * 0.002f,
            transform.position.z
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == transform) return;
        Destroy(gameObject);
        Managers.Game.CollectDiamond();
    }
}

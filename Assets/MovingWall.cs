using System.Collections;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public GameObject wall;

    private float moveSpeed = 2f;
    private bool wallCanMove = false;
    private float delay = 3f;
    private Vector2 moveDir = Vector2.right;
    private Vector2 currentPos;

    private void Awake()
    {
        currentPos = transform.position;
    }

    private void Update()
    {
        if (!wallCanMove) return;

        currentPos += moveDir.normalized * moveSpeed * Time.deltaTime;
        transform.position = currentPos;
    }

    public void StartWall()
    {
        StartCoroutine(MoveWall());
    }

    IEnumerator MoveWall()
    {
        yield return new WaitForSeconds(delay);

        wallCanMove = true;
    }
}

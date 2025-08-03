using System;
using System.Collections;
using UnityEngine;

public class EndHouse : MonoBehaviour
{
    
    public float panSpeed = 2f;
    public float playerSpeed = 7f;
    Animator playerAnimator;
    Transform player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("Player entered end house");
        playerAnimator = other.GetComponent<PlayerBehaviour>().animator;
        playerAnimator.SetBool("isFront", true);
        player = other.transform;
        other.GetComponent<PlayerBehaviour>().enabled = false;
        PanToPosition(transform.position + new Vector3(0, -4.6f, -10));
        MovePlayerToPos(transform.position);
    }
    
    
    

    public void PanToPosition(Vector3 targetPosition)
    {
        //StopAllCoroutines(); // Optional: stops any current pan
        StartCoroutine(PanCamera(targetPosition));
    }

    IEnumerator PanCamera(Vector3 targetPosition)
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            Camera.main.transform.position = Vector3.Lerp(startPos, targetPosition, elapsedTime);
            player.position = Vector3.Lerp(startPos, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * panSpeed;
            yield return null;
        }

        transform.position = targetPosition;
    }
    
    public float moveSpeed = 10f;

    public void MovePlayerToPos(Vector3 targetPos)
    {
        StopAllCoroutines();
        StartCoroutine(PanPlayer(targetPos));
    }

    private IEnumerator PanPlayer(Vector3 targetPos)
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;

        if (col != null)
            col.enabled = true;
    }

}

using System;
using System.Collections;
using UnityEngine;

public class EndHouse : MonoBehaviour
{
    
    public float panSpeed = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("Player entered end house");
        PanToPosition(transform.position + new Vector3(0, -12, 0));
    }
    

    public void PanToPosition(Vector3 targetPosition)
    {
        StopAllCoroutines(); // Optional: stops any current pan
        StartCoroutine(PanCamera(targetPosition));
    }

    IEnumerator PanCamera(Vector3 targetPosition)
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startPos, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * panSpeed;
            yield return null;
        }

        transform.position = targetPosition;
    }
}

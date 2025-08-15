using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        other.GetComponent<PlayerBehaviour>().ChangeDirection(PlayerBehaviour.Direction.none);
        other.GetComponent<PlayerBehaviour>().enabled = false;
        player = other.transform;
        other.GetComponent<PlayerBehaviour>().enabled = false;
        //PanToPosition(transform.position + new Vector3(0, -4.6f, -10));
        MovePlayerToPos(transform.position);
    }
    
    
    

    public void PanToPosition(Vector3 targetPosition)
    {
        //StopAllCoroutines(); // Optional: stops any current pan
        StartCoroutine(PanCamera(targetPosition));
        
        
        // while (elapsedTime < 1f)
        // {
        //     Debug.Log($"Pan camera time: {elapsedTime}");
        //     elapsedTime += Time.deltaTime;
        //     Camera.main.transform.position = Vector3.Lerp(startPos, targetPosition, elapsedTime);
        //     //player.position = Vector3.Lerp(startPos, targetPosition, elapsedTime);
        //     yield return null;
        //     Debug.Log($"pleseeeee e {elapsedTime < 1f}");
        // }
        //
        // Debug.Log($"Pan camera finished -> {elapsedTime}");

        
    }

    IEnumerator PanCamera(Vector3 targetPos)
    {
        Debug.Log($"Pan camera started -> {targetPos}");
        float duration = 2f;
        Vector3 startPos = Camera.main.transform.position;
        float elapsed = 0f;
        
        Transform cam = Camera.main.transform;
        
        while (Vector3.Distance(cam.position, targetPos) > 0.01f)
        {
            
            cam.position = Vector3.MoveTowards(cam.position, targetPos, moveSpeed * Time.deltaTime);
            Debug.Log($"looping cam -> {Vector3.Distance(cam.position, targetPos) > 0.01f}");
            yield return null;
        }

        // while (elapsed < duration)
        // {
        //     elapsed += Time.deltaTime;
        //     float t = elapsed / duration;
        //
        //     // Smooth interpolation
        //     Camera.main.transform.position = Vector3.Lerp(startPos, targetPosition, t);
        //
        //     yield return null; // wait one frame
        // }

        cam.position = targetPos; 

    }
    
    public float moveSpeed = 10f;

    public void MovePlayerToPos(Vector3 targetPos)
    {
        StopAllCoroutines();
        StartCoroutine(PanPlayer(targetPos));
    }

    private IEnumerator PanPlayer(Vector3 targetPos)
    {
        Debug.Log($"Pan player started -> {targetPos}");
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        while (Vector3.Distance(player.position, targetPos) > 0.01f)
        {
            Debug.Log($"looping");
            player.position = Vector3.MoveTowards(player.position, targetPos, moveSpeed * Time.deltaTime);
            Camera.main.transform.Translate(Vector3.down * panSpeed * Time.deltaTime);
            yield return null;
        }
        Debug.Log($"Pan player finished -> {targetPos}");

        player.position = targetPos;

        if (col != null)
            col.enabled = true;

        StartCoroutine(SwitchToEndDelay(4f));
    }

    private IEnumerator SwitchToEndDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        SceneManager.LoadScene("Home");
        PlayerPrefs.SetInt("VisitedStartBefore", 1);
    }

}

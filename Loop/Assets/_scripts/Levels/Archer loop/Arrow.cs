using UnityEngine;

public class Arrow : MonoBehaviour
{
    float moveSpeed;
    Archer archer;
    public float Arrowlength = 1f;
    public string targetTag;
    public Vector2 flightDirection = Vector2.right;
    public float flightDistance = 15f;
    private bool isFlying;
    Vector2 startPos;
    public void InisialiseArrow(float speed, Archer archer)
    {
        moveSpeed = speed;
        this.archer = archer;
        isFlying = true;
        startPos = transform.position;
    }
    
    void Update()
    {
        if (!isFlying) return;
        transform.Translate(flightDirection * (moveSpeed * Time.deltaTime));
        CheckHit();

        if (Vector2.Distance(transform.position, startPos) > flightDistance)
        {
            archer.ArrowMiss();
            Destroy(gameObject);
        }
    }

    void CheckHit()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, flightDirection, Arrowlength);
        
        if (hit.collider != null && hit.collider.CompareTag(targetTag))
        {
            archer.ArrowHit();
            isFlying = false;
        }
    }
}

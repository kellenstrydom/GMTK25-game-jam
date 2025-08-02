using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public float detectionRadius = 3f;
    private Transform player;
    private bool isTargeted = false;
    public GameObject indecator;

    public string objName;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the Player has the tag 'Player'.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius && !isTargeted)
        {
            // Player came into range
            player.GetComponent<Interact>()?.SetTarget(this);
            isTargeted = true;
        }
        else if (distance > detectionRadius && isTargeted)
        {
            // Player left the range
            player.GetComponent<Interact>()?.ClearTarget();
            isTargeted = false;
        }
        
        indecator.SetActive(isTargeted);
        
    }

    public void InteractWith(Interact interact = null)
    {
        Debug.Log("Interact with: " + gameObject.name);
        interact.GetComponent<Hold>().PickUp(GetComponent<SpriteRenderer>().sprite
        );
        Destroy(gameObject);
    }
}

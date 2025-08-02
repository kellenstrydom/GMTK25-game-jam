using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private InteractableObject currentTarget;

    public void SetTarget(InteractableObject target)
    {
        currentTarget = target;
        Debug.Log("Target set to: " + target.name);
    }

    public void ClearTarget()
    {
        if (currentTarget != null)
        {
            Debug.Log("Target cleared.");
            currentTarget = null;
        }
    }
    

    public void InteractWithObject()
    {
        currentTarget.GetComponent<InteractableObject>()?.InteractWith(this);
    }
}

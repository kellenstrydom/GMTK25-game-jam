using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPulse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float pulseScale = 1.1f;   // How big it gets on hover
    public float pulseSpeed = 5f;     // How fast it pulses

    private Vector3 originalScale;
    private bool isHovering = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isHovering)
        {
            // Smoothly scale up and down
            float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * 0.05f;
            transform.localScale = originalScale * scale * pulseScale;
        }
        else
        {
            // Smoothly go back to original scale
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * pulseSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}

using TMPro;
using UnityEngine;

public class Hold : MonoBehaviour
{
    public string objectName;
    public Sprite sprite;
    public GameObject itemIndicator;

    public AudioClip pickUp;
    public AudioSource pickUpAS;

    void Start()
    {
        pickUpAS.clip = pickUp; 
    }

    public void PickUp(string objectName, Sprite objSprite)
    {
        this.objectName = objectName;
        this.sprite = objSprite;
        itemIndicator.SetActive(true);
        pickUpAS.Play(); 
        itemIndicator.GetComponent<SpriteRenderer>().sprite = objSprite;
    }

    public void Drop()
    {
        this.objectName = null;
        this.sprite = null;
        itemIndicator.SetActive(false);
    }
}

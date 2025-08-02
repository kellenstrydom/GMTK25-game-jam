using TMPro;
using UnityEngine;

public class Hold : MonoBehaviour
{
    public string objectName;
    public Sprite sprite;
    public GameObject itemIndicator;

    public void PickUp(string objectName)
    {
        this.objectName = objectName;
        itemIndicator.SetActive(true);
        itemIndicator.GetComponent<TMP_Text>().text = objectName;
    }
    public void PickUp(Sprite objSprite)
    {
        this.sprite = objSprite;
        itemIndicator.SetActive(true);
        itemIndicator.GetComponent<SpriteRenderer>().sprite = objSprite;
    }
}

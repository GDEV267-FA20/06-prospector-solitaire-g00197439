using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Set Dyncamically")]
    public string suit; // Suit of the Card (C, D, H, or S)
    public int rank; // Rank of the Card (1-14)
    public Color color = Color.black; // Color to tint pips
    public string colS = "Black"; // or "Red". Name of the Color

    // This List holds all of the Decorator GameObjects
    public List<GameObject> decoGOs = new List<GameObject>();
    // This List holds all of the Pip GameObjects
    public List<GameObject> pipGOs = new List<GameObject>();

    public GameObject back; // The GameObject of the back of the card
    public CardDefinition def; // Parsed from DeckXML.xml

    public SpriteRenderer[] spriteRenderers;

    private void Start()
    {
        SetSortOrder(0); // Ensures that the card starts properly depth sorted
    }

    public void PopulateSpriteRenderers()
    {
        if(spriteRenderers == null || spriteRenderers.Length == 0)
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }
    }

    public void SetSortingLayerName(string tName)
    {
        PopulateSpriteRenderers();

        foreach(SpriteRenderer tSR in spriteRenderers)
        {
            tSR.sortingLayerName = tName;
        }
    }

    public void SetSortOrder(int sOrd)
    {
        PopulateSpriteRenderers();

        // Iterate through all the spriteRenderers as tSR
        foreach (SpriteRenderer tSR in spriteRenderers)
        {
            if (tSR.gameObject == this.gameObject)
            {
                // If the gameObject is this.gameObject, it's the background
                tSR.sortingOrder = sOrd; // Set it's order to sOrd
                continue; // And continue to the next iteration of the loop
            }
            // Each of the children of this GameObject are name. Switch based on the names.
            switch(tSR.gameObject.name)
            {
                case "back":
                    tSR.sortingOrder = sOrd + 2;
                    break;

                case "face":
                default:
                    tSR.sortingOrder = sOrd + 1;
                    break;
            }
        }
    }

    public bool faceUp
    {
        get
        {
            return (!back.activeSelf);
        }
        set
        {
            back.SetActive(!value);
        }
    }

    // Virtual mehtods can be overridden by subclass methods with the same name
    virtual public void OnMouseUpAsButton()
    {
        print(name); // When clicked, this outputs the card name
    }
}

[System.Serializable]
public class Decorator
{
    // This class stores info about each decorator or pip from DeckXML
    public string type; // For card pips, type = "pip"
    public Vector3 loc; // The location of the Sprite on the Card
    public bool flip = false; // Whether to flip the Sprite vertically
    public float scale = 1f; // The scale of the Sprite
}

[System.Serializable]
public class CardDefinition
{
    // This class stores info for each rank of card
    public string face; // Sprite to use for each face card
    public int rank; // The rank (1-13) of this card
    public List<Decorator> pips = new List<Decorator>(); // Pips used
}
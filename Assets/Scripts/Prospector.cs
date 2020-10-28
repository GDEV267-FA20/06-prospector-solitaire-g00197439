﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Prospector : MonoBehaviour
{
    static public Prospector S;

    [Header("Set in Inspector")]
    public TextAsset deckXML;
    public TextAsset alternateXML;
    public TextAsset layoutXML;
    public float xOffset = 3;
    public float yOffset = -2.5f;
    public Vector3 layoutCenter;

    [Header("Set Dynamically")]
    public Deck deck;
    public Layout layout;
    public List<CardProspector> drawPile;
    public Transform layoutAnchor;
    public CardProspector target;
    public List<CardProspector> tableau;
    public List<CardProspector> discardPile;

    void Awake()
    {
        S = this; // Set up a Singleton for Prospector
    }

    void Start()
    {
        deck = GetComponent<Deck>(); // Get the Deck
        if(deck.altCardLayout)
        {
            deck.InitDeck(alternateXML.text);
        } else
        {
            deck.InitDeck(deckXML.text); // Pass DeckXML to it
        }
        Deck.Shuffle(ref deck.cards); // This shuffles the deck by reference

        //Card c;
        //for (int cNum = 0; cNum < deck.cards.Count; cNum++)
        //{
        //    c = deck.cards[cNum];
        //    c.transform.localPosition = new Vector3((cNum % 13) * 3, cNum / 13 * 4, 0);
        //}

        layout = GetComponent<Layout>();
        layout.ReadLayout(layoutXML.text);
        drawPile = ConvertListCardsToListCardProspectors(deck.cards);
        LayoutGame();
    }

    List<CardProspector> ConvertListCardsToListCardProspectors(List<Card> lCD)
    {
        List<CardProspector> lCP = new List<CardProspector>();
        CardProspector tCP;
        foreach(Card tCD in lCD) {
            tCP = tCD as CardProspector;
            lCP.Add(tCP);
        }
        return lCP;
    }

    // The Draw function will pull a single card from the drawPile and return it
    CardProspector Draw()
    {
        CardProspector cd = drawPile[0]; // Pull the 0th CardProspector
        drawPile.RemoveAt(0); // Then remove it from List<> drawPile
        return cd; // And return it
    }

    // LayoutGame() positions the initial tableau of cards, aka the "mine"
    void LayoutGame()
    {
        // Create an empty GameObject to serve as an anchor for the tableau
        if(layoutAnchor == null)
        {
            GameObject tGO = new GameObject("LayoutAnchor");
            layoutAnchor = tGO.transform;
            layoutAnchor.transform.position = layoutCenter;
        }

        CardProspector cp;
        // Follow the layout
        foreach(SlotDef tSD in layout.slotDefs)
        {
            cp = Draw(); // Pull a card from the top (beginning) of the draw Pile
            cp.faceUp = tSD.faceUp; // Set its faceUp to the value in SlotDef
            cp.transform.parent = layoutAnchor; // Make its parent layoutAnchor
            // This replaces the previous parent: deck.deckAnchor, 
            // which appear as Deck in the Hierarchy when the scene is playing.
            cp.transform.localPosition = new Vector3(
                layout.multiplier.x * tSD.x,
                layout.multiplier.y * tSD.y,
                -tSD.layerID);
            cp.layoutID = tSD.id;
            cp.slotDef = tSD;
            // CardProspectors in the tableau have the state CardState.tableau
            cp.state = eCardState.tableau;
            // CardProspectors in the tableau have the state CardState.tableau
            cp.SetSortingLayerName(tSD.layerName); // Set the sorting layers

            tableau.Add(cp); // Add this CardProspector to the List<> tableau
        }
    }
}

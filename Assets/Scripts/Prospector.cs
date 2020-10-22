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

    [Header("Set Dynamically")]
    public Deck deck;

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

        Card c;
        for (int cNum = 0; cNum < deck.cards.Count; cNum++)
        {
            c = deck.cards[cNum];
            c.transform.localPosition = new Vector3((cNum % 13) * 3, cNum / 13 * 4, 0);
        }
    }
}

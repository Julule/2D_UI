using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] [Range(2, 24)] private int cardNumber = 6;
    [SerializeField] private int cardsByLine = 4;
    [SerializeField] private CardBehaviour prefab;
    [SerializeField] private Vector2 offset = Vector2.one * 0.2f;
    private List<CardBehaviour> cardBehaviours = new();
    private int cardFocusedId = -1; // attribut
    private CardBehaviour CardFocused => cardFocusedId >= 0 ? cardBehaviours[cardFocusedId] : null; // propriété équiv à la carte focused si elle est pas nulle

    private void Start(){
        if(cardNumber % 2 != 0) 
        {
            cardNumber += 1;
            Debug.LogError("Card number needs to be an even number");
        }

        InstantiateCards();
    }

    private void InstantiateCards()
    {

        BoxCollider2D collider = prefab.GetComponentInChildren<BoxCollider2D>();
        float width = collider.size.x;
        float height = collider.size.y;

        int lines = cardNumber / cardsByLine;
        if(cardNumber % cardsByLine != 0) lines++;

        float px = (cardsByLine * (width + offset.x) - offset.x) / -2f;
        float py = (lines * (height + offset.y) - offset.y) / -2f;

        Vector2 origin = new(px, py);
        
        for (int x = 0; x < cardsByLine; x++)
        {
            for (int y = 0; y < lines; y++)
            {
                Vector2 position = origin + y * (height + offset.y) * Vector2.up;
                position += x * (width + offset.x) * Vector2.right;
                CardBehaviour cardBehaviour = Instantiate(prefab, position, Quaternion.identity);

                cardBehaviour.ConnectToManager(this, cardBehaviours.Count);

                cardBehaviours.Add(cardBehaviour);
                if(cardBehaviours.Count >= cardNumber) return;
            }
        }
 
   }
   
    public void MouseOverCard(CardBehaviour cardBehaviour)
    {
        if(cardBehaviour == null)
        {
            if(cardFocusedId != -1)
            {
                CardFocused.Unfocus();
                cardFocusedId = -1;
            } 
                
        }
        else if(cardBehaviour.Id != cardFocusedId)
        {
            if (CardFocused != null)
            {
                CardFocused.Unfocus();
            }
            cardFocusedId = cardBehaviour.Id;
            cardBehaviour.Focus();
        }
    }
}

using System;
// using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] [Range(2, 24)] private int cardNumber = 6;
    [SerializeField] private int cardsByLine = 4;

    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector2 offset = Vector2.one * 0.2f;

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

        float px = cardsByLine / 2f * - width;
        float py = lines/2f * - height + (offset.y * lines -1) / 2f;

        Vector2 origin = new(px, py);
        
        for (int x = 0; x < cardsByLine; x++)
        {
            for (int y = 0; y < lines; y++)
            {
                Vector2 position = origin + y * (height + offset.y) * Vector2.up;
                position += x * (width + offset.x) * Vector2.right;
                Instantiate(prefab, position, Quaternion.identity);
            }
        }
    }
}

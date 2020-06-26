using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePiece : MonoBehaviour
{

    private GamePiece gamePiece;

    private void Awake()
    {
        gamePiece = GetComponent<GamePiece>();
    }

    public void Move(int newX, int newY)
    {
        gamePiece.X = newX;
        gamePiece.Y = newY;

        gamePiece.transform.localPosition = gamePiece
        .GridReference.GetPiecePosition(newX, newY);
    }
}

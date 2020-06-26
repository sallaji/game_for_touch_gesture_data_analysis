using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{

    private int x;
    private int y;

    public int X
    {
        get { return x; }
        set
        {
            if (IsMovable())
            {
                x = value;
            }
        }
    }

    public int Y
    {
        get { return y; }
        set
        {
            if (IsMovable())
            {
                y = value;
            }
        }
    }

    private Grid.PieceType type;

    //in case another piece value is asked
    public Grid.PieceType Type
    {
        get { return type; }
    }


    private Grid grid;
    public Grid GridReference
    {
        get { return grid; }
    }

    private MovablePiece movableComponent;
    public MovablePiece MovableComponent
    {
        get { return movableComponent; }

    }

    private FigurePiece figureComponent;
    public FigurePiece FigureComponent
    {
        get { return figureComponent; }
    }

    private SelectedFigureAnimation selectedFigureComponent;

    public SelectedFigureAnimation SelectedFigureComponent
    {
        get
        {
            return selectedFigureComponent;
        }
    }

    private void Awake()
    {
        movableComponent = GetComponent<MovablePiece>();
        figureComponent = GetComponent<FigurePiece>();
        selectedFigureComponent = GetComponent<SelectedFigureAnimation>();
    }

    //is being called after the figurePiece is instantiated
    public void Init(int _x, int _y, Grid _grid, Grid.PieceType _type)
    {
        x = _x;
        y = _y;
        grid = _grid;
        type = _type;
    }

    public bool IsMovable()
    {
        return movableComponent != null;

    }

    public bool HasFigurePieceComponent()
    {
        return figureComponent != null;
    }
}

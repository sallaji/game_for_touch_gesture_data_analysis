using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedFigureAnimation : MonoBehaviour
{

    public AnimationClip selectedAnimation;

    private bool figureIsSelected = false;

    public bool FigureIsSelected
    {
        get
        {
            return figureIsSelected;
        }
    }

    protected GamePiece gamePiece;

    private void Awake()
    {
        gamePiece = GetComponent<GamePiece>();
    }

    private IEnumerator SelectCoroutine()
    {
        Animator animator = GetComponent<Animator>();
        if (animator)
        {
            animator.Play(selectedAnimation.name);
            yield return new WaitForSeconds(selectedAnimation.length);
        }
    }

    public void Select()
    {
        figureIsSelected = true;
        StartCoroutine(SelectCoroutine());
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

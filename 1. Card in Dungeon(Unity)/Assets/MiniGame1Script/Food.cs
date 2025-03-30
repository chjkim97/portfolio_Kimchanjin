using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Food : MonoBehaviour
{
    public SpriteRenderer food;
    public PRS originPRS;
    public PRS newPRS;


    void Start()
    {
        originPRS = new PRS(transform.position, transform.rotation, transform.localScale);

    }
    private void Update()
    {
        newPRS = new PRS(transform.position, transform.rotation, transform.localScale); // 아직 사용하지 않음
    }

    public void MoveTransformFood(PRS prs, bool useDotweem, float dotweenTime = 0)
    {
        if (useDotweem)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }

    void OnMouseOver()
    {
        FoodController controller = FindObjectOfType<FoodController>();
        if (controller == null)
        {
            Debug.LogError("FoodController instance is not set.");
            return;
        }
        controller.FoodMouseOver(this);
    }

    void OnMouseExit()
    {
        FoodController controller = FindObjectOfType<FoodController>();
        controller.FoodMouseExit(this);
    }

    void OnMouseDown()
    {
        FoodController controller = FindObjectOfType<FoodController>();
        controller.FoodMouseDown();
    }

    void OnMouseUp()
    {
        FoodController controller = FindObjectOfType<FoodController>();
        controller.FoodMouseUp();
    }
}

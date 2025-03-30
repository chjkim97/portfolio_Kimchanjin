using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

public class FoodController : MonoBehaviour
{
    Food selectFood;
    bool isMyFoodDrag;
    bool onMyFoodArea;

    int layerAnimal;

    public void FoodMouseOver(Food food)
    {
        selectFood = food;
        EnlargeFood(true, food);
        Debug.Log("FoodMouseOver");
    }

    public void FoodMouseExit(Food food)
    {
        EnlargeFood(false, food);
        print("FoodMouseExit");
    }

    public void FoodMouseDown()
    {
        isMyFoodDrag = true;
        print("FoodMouseDown");
    }

    public void FoodMouseUp()
    {
        isMyFoodDrag = false;
        if (selectFood != null && !onMyFoodArea)
        {
            // ���� ���콺�� ��ġ�� ���� ������Ʈ�� ����
            Vector3 mousePosition = Utils.MousePos; // ���� ���콺�� ���� ��ǥ
            Vector3 placePosition = new Vector3(mousePosition.x, mousePosition.y, -10f); // z �� �� ����

            // ���콺 ��ġ�� �̵���Ű��
            Debug.Log($"Moving to: {placePosition}");
            selectFood.MoveTransformFood(new PRS(placePosition, Utils.QI, selectFood.originPRS.scale), false);

        }
        
        print("FoodMouseUp");
    }
    void FoodDrag()
    {
        if (!onMyFoodArea)
        {
            Vector3 fixedZPosition = new Vector3(Utils.MousePos.x, Utils.MousePos.y, -10f);
            selectFood.MoveTransformFood(new PRS(fixedZPosition, Utils.QI, selectFood.originPRS.scale), false);
        }
    }

    void EnlargeFood(bool isEnlarge, Food food)
    {
        if (onMyFoodArea) 
        {
            if (isEnlarge)
            {
                Vector3 enlargePos = new Vector3(food.originPRS.pos.x, food.originPRS.pos.y + 2.5f, -10f);
                food.MoveTransformFood(new PRS(enlargePos, Utils.QI, Vector3.one * 15f), false);
            }
            else
            {
                food.MoveTransformFood(food.originPRS, false);
            }
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        layerAnimal = LayerMask.NameToLayer("AnimalLayer");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int foodLayerArea = LayerMask.NameToLayer("FoodArea");

        onMyFoodArea = Array.Exists(hits, x => x.collider.gameObject.layer == foodLayerArea);

        if (isMyFoodDrag && selectFood != null)
        {
            
            FoodDrag();
        }
    }
}

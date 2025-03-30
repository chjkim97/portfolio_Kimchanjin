using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    public Button[] Buttons;
    void Start()
    {
        UpdateButtonStates(); 
    }
    private void UpdateButtonStates()
    {
        foreach (var button in Buttons)
        {
            button.interactable = false;
        }
        if (GameData.Instance.bosscount >= 3)
        {
            Buttons[0].interactable = true;
            Transform shaderTransform = Buttons[0].transform.Find("Shader");
            if (shaderTransform != null)
            {
                shaderTransform.gameObject.SetActive(false);
            }
        }
        if (GameData.Instance.elapsedTime >= 1800)
        {
            Buttons[1].interactable = true;
            Transform shaderTransform = Buttons[1].transform.Find("Shader");
            if (shaderTransform != null)
            {
                shaderTransform.gameObject.SetActive(false);
            }
        }
        if (GameData.Instance.elapsedTime >= 3600)
        {
            Buttons[2].interactable = true;
            Transform shaderTransform = Buttons[2].transform.Find("Shader");
            if (shaderTransform != null)
            {
                shaderTransform.gameObject.SetActive(false);
            }
        }
        if (GameData.Instance.itemCount == 1)
        {
            Buttons[3].interactable = true;
            Transform shaderTransform = Buttons[3].transform.Find("Shader");
            if (shaderTransform != null)
            {
                shaderTransform.gameObject.SetActive(false);
            }
        }

        if (GameData.Instance.buyCard >= 5)
        {
            Buttons[4].interactable = true;
            Transform shaderTransform = Buttons[4].transform.Find("Shader");
            if (shaderTransform != null)
            {
                shaderTransform.gameObject.SetActive(false); 
            }
        }
        if (GameData.Instance.buyCard >= 10)
        {
            Buttons[5].interactable = true;
            Transform shaderTransform = Buttons[5].transform.Find("Shader");
            if (shaderTransform != null)
            {
                shaderTransform.gameObject.SetActive(false);
            }
        }


    }
    public void ButtonFiveClick()
    {
        Debug.Log($"Button5 clicked!");
        GameData.Instance.AddDiamonds(50);
        Buttons[4].interactable = false; 

        Transform shaderTransform = Buttons[4].transform.Find("Shader");
        if (shaderTransform != null)
        {
            shaderTransform.gameObject.SetActive(true); 
        }
    }
    public void ButtonTenClick()
    {
        Debug.Log($"Button5 clicked!");
        GameData.Instance.AddDiamonds(50);
        Buttons[5].interactable = false;

        Transform shaderTransform = Buttons[5].transform.Find("Shader");
        if (shaderTransform != null)
        {
            shaderTransform.gameObject.SetActive(true);
        }
    }
    public void ButtonBossCountClick()
    {
        Debug.Log($"ButtonBossCountClick");
        GameData.Instance.AddDiamonds(50);
        Buttons[0].interactable = false;

        Transform shaderTransform = Buttons[0].transform.Find("Shader");
        if (shaderTransform != null)
        {
            shaderTransform.gameObject.SetActive(true);
        }
    }
    public void ButtonDeckTwentyClick()
    {
        Debug.Log($"ButtonDeckTwentyClick");
        GameData.Instance.AddDiamonds(50);
        Buttons[3].interactable = false;

        Transform shaderTransform = Buttons[3].transform.Find("Shader");
        if (shaderTransform != null)
        {
            shaderTransform.gameObject.SetActive(true);
        }
    }

    public void ButtonPlayTime30Click()
    {
        Debug.Log($"ButtonPlayTime30Click");
        GameData.Instance.AddDiamonds(50);
        Buttons[1].interactable = false;

        Transform shaderTransform = Buttons[1].transform.Find("Shader");
        if (shaderTransform != null)
        {
            shaderTransform.gameObject.SetActive(true);
        }
    }
    public void ButtonPlayTime1hourClick()
    {
        Debug.Log($"ButtonPlayTime1hourClick");
        GameData.Instance.AddDiamonds(50);
        Buttons[2].interactable = false;

        Transform shaderTransform = Buttons[2].transform.Find("Shader");
        if (shaderTransform != null)
        {
            shaderTransform.gameObject.SetActive(true);
        }
    }
}

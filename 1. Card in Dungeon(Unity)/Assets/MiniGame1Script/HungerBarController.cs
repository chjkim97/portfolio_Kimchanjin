using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerBarController : MonoBehaviour
{

    public int hungerBarNumber = 0;
    public Renderer[] segmentRenderers;
    private Color startColor = Color.green;
    private Color endColor = Color.red;

    public AnimalBehavior AnimalBehaivor;

    // Start is called before the first frame update
    void Start()
    {
        AnimalBehaivor = GetComponent<AnimalBehavior>();
        segmentRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (var renderer in segmentRenderers)
        {
            renderer.material.color = startColor;
        }

        InvokeRepeating("HungerBarState", 2, 1.5f);


    }

    public void HungerBarState()
    {
        if (hungerBarNumber < segmentRenderers.Length)
        {
            segmentRenderers[hungerBarNumber].material.color = endColor;
            hungerBarNumber++;
        }

        else 
        { 
            CancelInvoke("HungerBarState");
            Debug.Log("Stop InvokeRepeat");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

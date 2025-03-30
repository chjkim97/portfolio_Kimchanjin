using UnityEngine;
using System.Collections;

public class HungerBarController : MonoBehaviour
{
    public int hungerBarNumber = 0;
    public Renderer[] segmentRenderers;
    private Color startColor = Color.green;
    private Color endColor = Color.red;

    public AnimalBehaivor AnimalBehaivor;

    void Start()
    {

        AnimalBehaivor = GetComponent<AnimalBehaivor>();
        segmentRenderers = GetComponentsInChildren<MeshRenderer>();

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

        else { CancelInvoke("HungerBarState"); }
    }



    void Update()
    {
    }
}

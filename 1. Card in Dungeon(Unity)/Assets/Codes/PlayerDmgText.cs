using UnityEngine;
using TMPro;

public class PlayerDmgText : MonoBehaviour
{
    public TextMeshPro textMesh;

    void Awake()
    {


        if (textMesh == null)
        {
            Debug.LogWarning("TextMeshPro 컴포넌트를 찾을 수 없습니다!");
        }
        else
        {
            // CardEffect 인스턴스를 찾음
            CardEffect cardEffect = FindObjectOfType<CardEffect>();
            if (cardEffect != null)
            {
                textMesh.text = cardEffect.DamageToEnemy.ToString();
            }
            else
            {
                Debug.LogWarning("CardEffect 인스턴스를 찾을 수 없습니다!");
            }
        }
    }
}

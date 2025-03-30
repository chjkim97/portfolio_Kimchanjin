using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleOrder : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers; // 파티클 렌더러 배열
    private List<int> particleOriginalOrders = new List<int>(); // 원래 sortingOrder 저장 리스트
    private ParticleSystem[] particleSystems;

    // Start is called before the first frame update
    void Start()
    {
        // 렌더러 배열에 포함된 파티클 시스템의 sortingOrder 조정
        foreach (Renderer renderer in renderers)
        {
            ParticleSystemRenderer particleRenderer = renderer.GetComponent<ParticleSystemRenderer>();

            if (particleRenderer != null)
            {
                particleOriginalOrders.Add(particleRenderer.sortingOrder);
                particleRenderer.sortingOrder += 1000; // 현재 sortingOrder에 1000 더해 가장 위에 표시
            }
            else
            {
                particleOriginalOrders.Add(0); // 파티클 렌더러가 없을 경우 기본값
            }
        }
    }

    // 만약 나중에 sortingOrder를 원래대로 복구하고 싶다면 이 함수를 추가할 수 있음
    public void RestoreOriginalOrder()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            ParticleSystemRenderer particleRenderer = renderers[i].GetComponent<ParticleSystemRenderer>();
            if (particleRenderer != null)
            {
                particleRenderer.sortingOrder = particleOriginalOrders[i]; // 원래 sortingOrder로 복원
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }



}

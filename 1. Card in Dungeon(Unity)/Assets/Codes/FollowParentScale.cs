using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParentScale : MonoBehaviour
{
    private Transform parentTransform;

    void Start()
    {
        // 부모 오브젝트의 Transform을 참조
        parentTransform = transform.parent;
    }

    void LateUpdate()
    {
        if (parentTransform != null)
        {
            // 부모 오브젝트의 스케일을 자식 오브젝트에 적용
            transform.localScale = parentTransform.lossyScale;
        }
    }
}

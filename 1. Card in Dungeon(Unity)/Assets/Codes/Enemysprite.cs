using UnityEngine;
using UnityEngine.U2D.Animation; // SpriteSkin을 사용하기 위한 네임스페이스

public class Enemysprite : MonoBehaviour
{
    private Animator animator;  // Animator 컴포넌트 변수
    public string baseName = "enemy"; // 애니메이션과 스프라이트의 기본 이름

    void Start()
    {
        // SpriteRenderer 컴포넌트 가져오기
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        PlayerStats playerStats = FindObjectOfType<PlayerStats>(); // PlayerStats 가져오기

        // 오브젝트 이름이 "sprite" + playerStats.enemynumber와 일치하지 않으면 비활성화
        string expectedName = "sprite" + playerStats.enemynumber;
        if (gameObject.name != expectedName)
        {
            Debug.Log("Deactivating GameObject: " + gameObject.name + ", Expected: " + expectedName);
            gameObject.SetActive(false); // 오브젝트 비활성화
            return;
        }

        if (playerStats != null && spriteRenderer != null)
        {
            // playerStats.enemynumber에 따라 스프라이트와 애니메이션 이름 설정
            string enemyName = baseName + playerStats.enemynumber; // enemy1, enemy2 등의 이름

            // Resources/Enemy 폴더에서 스프라이트 로드
            Sprite enemySprite = Resources.Load<Sprite>("Enemy/" + enemyName);

            if (enemySprite != null)
            {
                // SpriteRenderer에 스프라이트 적용
                spriteRenderer.sprite = enemySprite;
            }
            else
            {
                Debug.LogWarning("Sprite not found at path: Resources/Enemy/" + enemyName);
            }

            // Animator 컴포넌트 가져오기
            animator = GetComponentInChildren<Animator>();

            if (animator != null)
            {
                // Resources/Enemy 폴더에서 Animator Controller 로드
                RuntimeAnimatorController animatorController = Resources.Load<RuntimeAnimatorController>("Enemy/" + enemyName);

                if (animatorController != null)
                {
                    // Animator에 컨트롤러 적용
                    animator.runtimeAnimatorController = animatorController;

                    // 애니메이션 상태가 존재하는지 확인 후 애니메이션 실행
                    if (AnimatorHasState(animator, enemyName))
                    {
                        // enemy1 등의 애니메이션 상태 실행 (기본 레이어 0)
                        animator.Play(enemyName, 0); 
                    }
                    else
                    {
                        Debug.LogWarning("Animation state " + enemyName + " does not exist in the Animator.");
                    }
                }
                else
                {
                    Debug.LogWarning("Animator Controller not found at path: Resources/Enemy/" + enemyName);
                }
            }
            else
            {
                Debug.LogWarning("No Animator component found in this GameObject.");
            }

            // SpriteSkin 컴포넌트 가져오기
            SpriteSkin spriteSkin = GetComponentInChildren<SpriteSkin>();

            if (spriteSkin != null)
            {
                // 본이 설정되어 있는지 확인 후, 활성화
                if (spriteSkin.boneTransforms != null && spriteSkin.boneTransforms.Length > 0)
                {
                    spriteSkin.enabled = true;  // 본이 설정되어 있으면 SpriteSkin을 활성화
                }
                else
                {
                    Debug.LogWarning("SpriteSkin's bone transforms are not set.");
                }
            }
            else
            {
                Debug.LogWarning("No SpriteSkin component found on this GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("PlayerStats or SpriteRenderer component not found.");
        }
    }

    // Animator에 특정 상태가 존재하는지 확인하는 함수
    private bool AnimatorHasState(Animator animator, string stateName)
    {
        foreach (var animationClip in animator.runtimeAnimatorController.animationClips)
        {
            if (animationClip.name == stateName)
            {
                return true;
            }
        }
        return false;
    }
}

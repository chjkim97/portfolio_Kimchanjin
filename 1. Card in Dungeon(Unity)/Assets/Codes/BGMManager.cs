using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioClip bgmClip1; // 기본 배경음악 (예: Main)
    public AudioClip bgmClip2; // 상점 배경음악 (Shop 씬)
    public AudioClip bgmClip3; // 테스트 맵 배경음악 (TestMapCam 씬)
    public AudioClip bgmClip4; // 사망 배경음악 (Dead 씬)

    private AudioSource audioSource;
    private string currentSceneName;
    private AudioClip currentClip;

    public Slider volumeSlider; // 음량 조절 슬라이더

    private void Awake()
    {
        // Singleton 패턴으로 오브젝트가 중복되지 않게 설정
        if (FindObjectsOfType<BGMManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // 오디오 소스 컴포넌트를 가져오거나 추가합니다.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 오디오 소스 초기 설정
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.05f;

        // 슬라이더의 초기 값 설정
        volumeSlider.value = audioSource.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume); // 슬라이더 값 변경 이벤트 등록

        // 씬 변경 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 처음 시작할 때 현재 씬의 배경음악 재생
        PlayBGMForCurrentScene();
    }

    // 씬이 로드될 때마다 호출되는 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 변경될 때마다 새로운 BGM을 재생
        PlayBGMForCurrentScene();
    }

    // 현재 씬 이름에 따라 적절한 BGM을 재생하는 함수
    private void PlayBGMForCurrentScene()
    {
        // 현재 씬 이름을 가져옴
        string newSceneName = SceneManager.GetActiveScene().name;

        // 새로운 씬일 경우에만 BGM을 변경
        if (newSceneName != currentSceneName)
        {
            currentSceneName = newSceneName;

            // 적절한 BGM 선택
            AudioClip selectedClip = null;

            switch (newSceneName)
            {
                case "Shop": // 상점 씬
                    selectedClip = bgmClip2;
                    break;
                case "BattleScene": // 테스트 맵 씬
                    selectedClip = bgmClip3;
                    break;
                case "Dead": // 사망 씬
                    selectedClip = bgmClip4;
                    break;
                default: // 그 외 씬은 기본 BGM
                    selectedClip = bgmClip1;
                    break;
            }

            // BGM이 같지 않을 경우에만 변경
            if (selectedClip != currentClip)
            {
                ChangeBGM(selectedClip);
                currentClip = selectedClip; // 현재 재생 중인 BGM 업데이트
            }
        }
    }

    // BGM을 변경하는 함수
    private void ChangeBGM(AudioClip newClip)
    {
        // 현재 재생 중인 BGM이 있다면 멈춤
        if (audioSource.isPlaying && audioSource.clip != newClip)
        {
            audioSource.Stop();
        }

        // 새로운 BGM이 설정된 경우에만 재생
        if (newClip != null)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }

    private void OnDestroy()
    {
        // 씬 변경 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 음량 조절 함수
    public void SetVolume(float volume)
    {
        audioSource.volume = volume; // 슬라이더의 값에 따라 오디오 소스의 음량을 조절
    }

    // 수동으로 BGM을 시작하는 함수 (필요한 경우 사용)
    public void PlayBGM()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // 수동으로 BGM을 멈추는 함수 (필요한 경우 사용)
    public void StopBGM()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}

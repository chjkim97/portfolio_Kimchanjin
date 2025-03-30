using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraPos : MonoBehaviour
{
    private Vector3 changeposition;
    Vector3 offset;
    // Start is called before the first frame update
    
    void Start()
    {
        offset = new Vector3 (0, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {
      //  changeposition = mainCamera.transform.position + offset;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
    //    if (SceneManager.GetActiveScene().name == "TestRandomMap") { mainCamera.transform.position = changeposition + offset; }
    }

    public Vector3 GetCameraPosition()
    {
        return changeposition;
    }
}

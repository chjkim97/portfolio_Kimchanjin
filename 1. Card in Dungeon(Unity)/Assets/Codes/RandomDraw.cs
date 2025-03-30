using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RandomDraw : MonoBehaviour
{
    public int[] arr_randmapnum;

    public Button[] Randbuttons;
    // ���� = 1, ���� = 2, �����̺�Ʈ = 3
    public Button[] Start_Stage;
    public Button Boss_Stage;
   
    void Start()
    {
        arr_randmapnum = new int[20];
        Randbuttons = new Button[20];

        Start_Stage = new Button[3];

        Randbuttons[0] = GameObject.Find("random (1)").GetComponent<Button>();
        Randbuttons[1] = GameObject.Find("random (2)").GetComponent<Button>();
        Randbuttons[2] = GameObject.Find("random (3)").GetComponent<Button>();
        Randbuttons[3] = GameObject.Find("random (4)").GetComponent<Button>();
        Randbuttons[4] = GameObject.Find("random (5)").GetComponent<Button>();
        Randbuttons[5] = GameObject.Find("random (6)").GetComponent<Button>();
        Randbuttons[6] = GameObject.Find("random (7)").GetComponent<Button>();
        Randbuttons[7] = GameObject.Find("random (8)").GetComponent<Button>();
        Randbuttons[8] = GameObject.Find("random (9)").GetComponent<Button>();
        Randbuttons[9] = GameObject.Find("random (10)").GetComponent<Button>();
        Randbuttons[10] = GameObject.Find("random (11)").GetComponent<Button>();
        Randbuttons[11] = GameObject.Find("random (12)").GetComponent<Button>();
        Randbuttons[12] = GameObject.Find("random (13)").GetComponent<Button>();
        Randbuttons[13] = GameObject.Find("random (14)").GetComponent<Button>();
        Randbuttons[14] = GameObject.Find("random (15)").GetComponent<Button>();
        Randbuttons[15] = GameObject.Find("random (16)").GetComponent<Button>();
        Randbuttons[16] = GameObject.Find("random (17)").GetComponent<Button>();
        Randbuttons[17] = GameObject.Find("random (18)").GetComponent<Button>();
        Randbuttons[18] = GameObject.Find("random (19)").GetComponent<Button>();
        Randbuttons[19] = GameObject.Find("random (20)").GetComponent<Button>();

        Start_Stage[0] = GameObject.Find("start (1)").GetComponent<Button>();
        Start_Stage[1] = GameObject.Find("start (2)").GetComponent<Button>();
        Start_Stage[2] = GameObject.Find("start (3)").GetComponent<Button>();
        Boss_Stage = GameObject.Find("boss").GetComponent<Button>();


        for (int i=0; i<arr_randmapnum.Length; i++)
        {
            arr_randmapnum[i] = Random.Range(1, 9);
            //arr_randmapnum[i] = 8;
        }
        for (int i = 0; i < 20; i++)
        {
            int buttonIndex = arr_randmapnum[i]; // Ŭ���� ������ ����� ���� �ε����� ����
            Randbuttons[i].onClick.AddListener(() => RandMap_Button(buttonIndex));
        }

        for (int i = 0; i<3; i++)
        {
            Start_Stage[i].onClick.AddListener(() => SS_Stage());
        }

        Boss_Stage.onClick.AddListener(() => BS_stage());
       
        Debug.Log(arr_randmapnum[0]);
        Debug.Log(arr_randmapnum[3]);
        Debug.Log(arr_randmapnum[5]);
        Debug.Log(arr_randmapnum[7]);
        Debug.Log(arr_randmapnum[12]);
        Debug.Log(arr_randmapnum[18]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandMap_Button(int num)
    {
        if (num == 1) { SceneManager.LoadScene("BattleScene"); }
        if (num == 2) { SceneManager.LoadScene("Shop"); }
        if (num == 3) { SceneManager.LoadScene("Randomevent"); }
        if (num == 4) { SceneManager.LoadScene("Rest"); }
        if (num == 5) { SceneManager.LoadScene("REvent1"); }
        if (num == 6) { SceneManager.LoadScene("REvent2"); }
        if (num == 7) { SceneManager.LoadScene("REvent3"); }
        if (num == 8) { SceneManager.LoadScene("REvent4"); }

    }
    
    public void SS_Stage()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void BS_stage()
    {
        SceneManager.LoadScene("BattleScene");
    }
}

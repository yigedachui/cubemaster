using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Newtonsoft.Json;

public class CubeManager : MonoBehaviour
{
    public GameObject cubePrefa;
    public Transform CubeHandler;

    Ray ray;

    public int x_num;
    public int y_num;
    public int z_num;
    public Cube[][][] cubes;
    public int[][][] cubeMap;
    public int totalCount;
    public float repeatPercentage = 0.28f;

    public List<Cube> cubeList;

    private void Awake()
    {
        DataManager dataManager = new DataManager();
        dataManager.Init();

        LoadLevelInfo();
        int TotalCubes = totalCount;
        cubeList = new List<Cube>(TotalCubes);

        cubes = new Cube[x_num][][];

        for (int x = 0; x < x_num; x++)
        {
            cubes[x] = new Cube[y_num][];
            for (int y = 0; y < y_num; y++)
            {
                cubes[x][y] = new Cube[z_num];
            }
        }

        int TotalCubeType = TotalCubes / 3;

        int RepeatCountType = (int)(TotalCubeType * repeatPercentage);

        int CubeTypeCount = TotalCubeType - RepeatCountType;

        int colorCount = dataManager.colors.Count;

        for (int i = 0; i < TotalCubeType; i++)
        {
            if (colorCount > TotalCubeType)
            {

            }
            string colorStr = dataManager.colors[i];

            //string var = (i <= CubeTypeCount) ? i : Random.Range(0, CubeTypeCount - 1);

            for (int j = 0; j < 3; j++)
            {
                GameObject cubeObj = Instantiate(cubePrefa);
                Cube cube = cubeObj.GetComponent<Cube>();
                //cube.data.type = type;
                cubeObj.name = cube.data.color.ToString();
                cube.SetCubeColor(colorStr);
                cubeList.Add(cube);
            }
        }

        for (int i = 0; i < cubeList.Count; i++)
        {
            int randomNum = Random.Range(i, cubeList.Count - 1);            
            Cube tempCube = cubeList[randomNum];
            cubeList[randomNum] = cubeList[i];
            cubeList[i] = tempCube;
        }
        SpawnCube();
        StartCoroutine(BeginTween());
    }

    private void Start()
    {
        GameSceneUI.Instance.OnEventBack += OnCubeTurnBack;

    }

    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Ended:
                    ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    if (Physics.Raycast(ray, out RaycastHit hitInfo, 1 << LayerMask.NameToLayer("Cube")))
                    {
                        GameObject obj = hitInfo.collider.gameObject;
                        Debug.Log(obj.name);
                        if (obj)
                        {
                            Cube cube = obj.GetComponent<Cube>();
                            CubeContainer.Instance.AddCube(cube);
                        }
                    }
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    break;
            }

        }
        else if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 1 << LayerMask.NameToLayer("Cube")))
            {
                GameObject obj = hitInfo.collider.gameObject;
                //Debug.Log(obj.name);
                if (obj)
                {
                    Cube cube = obj.GetComponent<Cube>();                    
                    CubeContainer.Instance.AddCube(cube);
                }
            }
        }
    }

    private void LoadLevelInfo()
    {
        string str = Resources.Load<TextAsset>("level_2").text;

        LevelInfo lvInfo = JsonConvert.DeserializeObject<LevelInfo>(str);

        x_num = lvInfo.x; 
        y_num = lvInfo.y;
        z_num = lvInfo.z;
        totalCount = lvInfo.total;
        cubeMap = lvInfo.cubeMap;
    }


    public void SpawnCube()
    {
        //int TierNum = 0;
        int index = 0;

        float xHalf = x_num / 2.0f;
        float yHalf = y_num / 2.0f;
        float zHalf = z_num / 2.0f;

        for (int y = 0; y < y_num; y++)
        {
            for (int z = 0; z < z_num; z++)
            {
                for (int x = 0; x < x_num; x++)
                {
                    if (cubeMap[x][y][z] == 1)
                    {
                        //int i = TierNum + (z * y_num + x);
                        //Debug.Log("i:" + i);
                        Cube cube = cubeList[index];
                        index++;
                        cubes[x][y][z] = cube;
                        cube.transform.SetParent(CubeHandler, true);
                        Vector3 localPos = new Vector3(x - xHalf + 0.5f, y - yHalf + 0.5f, z - zHalf + 0.5f);
                        cube.data.position = localPos;
                        cube.data.OrangePosition = localPos;
                        cube.gameObject.SetActive(false);

                    }
                }
            }
            //TierNum += z_num * y_num;
        }
        //CubeHandler.transform.rotation = Quaternion.Euler(0, 30, 0);
    }


    public IEnumerator BeginTween()
    {
        for (int y = 0; y < y_num; y++)
        {
            for (int x = 0; x < x_num; x++)
            {
                for (int z = 0; z < z_num; z++)
                {
                    if (cubeMap[x][y][z] == 1)
                    {
                        Cube c = cubes[x][y][z];
                        c.transform.position = c.data.position + Vector3.up * y_num + Vector3.forward * 6;
                        c.gameObject.SetActive(true);
                        c.transform.DOLocalMove(c.data.position, 0.3f);
                    }
                }                
            }
            yield return new WaitForSeconds(0.1f);
        }

    }

    public void OnCubeTurnBack()
    {
        Cube c = CubeContainer.Instance.GetEnd;
        if (c != null)
        {
            c.transform.SetParent(CubeHandler);
            c.transform.DOLocalMove(c.data.OrangePosition, 0.3f);
            c.transform.localScale = Vector3.one;
            c.transform.localRotation = Quaternion.identity;
        }
    }

}

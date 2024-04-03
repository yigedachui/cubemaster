using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public GameObject cubePrefa;
    public Transform CubeHandler;

    Ray ray;

    public int height = 3;
    public int width = 3;
    public int depth = 3;
    
    public float repeatPercentage = 0.28f;

    public Cube[,,] cubes;
    public List<Cube> cubeList;


    private void Awake()
    {
        int TotalCubes = height * width * depth;
        cubeList = new List<Cube>(TotalCubes);

        cubes = new Cube[width, height, depth];

        int TotalCubeType = TotalCubes / 3;

        int RepeatCountType = (int)(TotalCubeType * repeatPercentage);

        int CubeTypeCount = TotalCubeType - RepeatCountType;

        for (int i = 0; i < TotalCubeType; i++)
        {
            CubeData.Type type = (i <= CubeTypeCount) ? (CubeData.Type)i : (CubeData.Type)Random.Range(0, CubeTypeCount - 1);

            for (int j = 0; j < 3; j++)
            {
                GameObject cubeObj = Instantiate(cubePrefa);
                Cube cube = cubeObj.GetComponent<Cube>();
                cube.data.type = type;
                cubeObj.name = cube.data.type.ToString();
                cube.SetCubeColor();
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

        StartCoroutine(SpawnCube());

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
                            cube.GetComponent<BoxCollider>().enabled = false;
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

    public IEnumerator SpawnCube()
    {
        int TierNum = 0;

        for (int y = 0; y < height; y++)
        {
            for (int z = 0; z < depth; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    int i = TierNum + (z * width + x);
                    Cube cube = cubeList[i];

                    cube.transform.SetParent(CubeHandler, true);
                    Vector3 localPos = new Vector3(x - width / 2.0f + 0.5f, y - height / 2.0f + 0.5f, z - depth / 2.0f + 0.5f);
                    cube.transform.localPosition = localPos;
                }
            }
            TierNum += depth * width;
            yield return new WaitForSeconds(0.3f);
        }
    }

}

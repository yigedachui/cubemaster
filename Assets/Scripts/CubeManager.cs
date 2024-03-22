using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Text.RegularExpressions;

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
    private List<Cube> cubeList;
    public List<LinkedListNode<Cube>> matchedCubes;
    CubeMatchContainer matchContainer;

    private void Awake()
    {
        matchedCubes = new List<LinkedListNode<Cube>>();
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
        matchContainer = new CubeMatchContainer();
        matchContainer.OnMatchEvent += OnMatchedEvent;
    }

    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        }
        else if (Input.GetMouseButtonDown(0)) 
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 1 << LayerMask.NameToLayer("Cube")))
            {
                GameObject obj = hitInfo.collider.gameObject;
                if (obj)
                {
                    Cube cube = obj.GetComponent<Cube>();
                    matchContainer.AddCube(cube);
                }
            }
        }                
    }

    private bool OnMatchedEvent(LinkedList<Cube> LinkedHead)
    {
        int MatchedCount = 0;
        if (LinkedHead.Count < 3)
        {
            return false;
        }

        LinkedListNode<Cube> fast;
        LinkedListNode<Cube> late = fast = LinkedHead.First;

        while (fast != null)
        {
            if (late.Value.data.type == fast.Value.data.type)
            {
                matchedCubes.Add(fast);
                fast = fast.Next;                
                MatchedCount++;
                if (MatchedCount == 3)
                {
                    for (int i = 0; i < matchedCubes.Count; i++)
                    {
                        LinkedHead.Remove(matchedCubes[i]);
                    }
                    matchedCubes.Clear();
                    return true;
                }
            }
            else
            {
                late = fast;
                fast = late.Next;
                MatchedCount = 0;
                matchedCubes.Clear();
            }            
        }
        return false;
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

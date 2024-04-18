using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CubeContainer : MonoBehaviour
{
    public List<Transform> platform;
    public Cube[] LinkedHead;

    public Cube GetEnd
    {
        get
        {
            if (CurrentCount <= 0)
            {
                return null;
            }
            else 
            {
                if (LinkedHead[CurrentCount - 1] != null)
                {
                    int i = CurrentCount;
                    CurrentCount--;
                    Cube c = LinkedHead[i - 1];
                    LinkedHead[i - 1] = null;
                    return c;
                }
                
            }
            return null;
        }
    }

    [SerializeField]
    private List<Cube> matchedList;
    public static CubeContainer Instance;
    public int CurrentCount = 0;
    public const int MaxCount = 7;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LinkedHead = new Cube[MaxCount];
        matchedList = new List<Cube>();        
    }

    public void AddCube(Cube cube)
    {        
        if (CurrentCount < MaxCount)
        {
            Debug.Log("add");
            SortCube(cube);
        }
        Matched();
        StartCoroutine(DeleteMatchedCube());
    }

    private IEnumerator DeleteMatchedCube()
    {
        if (matchedList.Count >= 3)
        {
            for (int i = 0; i < MaxCount; i++)
            {
                if (LinkedHead[i] != null && matchedList.Contains(LinkedHead[i]))
                {
                    yield return new WaitForSeconds(0.5f);
                    for (int j = i; j < i + 3; j++)
                    {
                        Destroy(LinkedHead[j].gameObject);
                        LinkedHead[j] = null;
                    }
                    matchedList.Clear();
                    CurrentCount -= 3;

                    for (int k = i + 3; k < MaxCount; k++)
                    {
                        if (LinkedHead[k] != null)
                        {
                            LinkedHead[k - 3] = LinkedHead[k];
                            LinkedHead[k].MovePosition(platform[k - 3].position);
                            LinkedHead[k] = null;
                        }                        
                    }
                }

            }
        }
    }

    public void SortCube(Cube cube)
    {
        if (LinkedHead[0] == null)
        {
            LinkedHead[0] = cube;
            cube.MovePosition(platform[0].position);
            CurrentCount++;
            return;
        }
        
        for (int i = 0; i < MaxCount; i++)
        {
            if (LinkedHead[i] == null)
            {
                LinkedHead[i] = cube;
                CurrentCount++;
                cube.MovePosition(platform[i].position);
                break;
            }
            if (LinkedHead[i] != null && LinkedHead[i].IsSameType(cube) == false)
            {
                continue;
            }
            else
            {
                for (int j = i; j < CurrentCount ; j++)
                {
                    if (LinkedHead[j] == null)
                    {
                        break;
                    }
                    if (LinkedHead[j] != null && LinkedHead[j].IsSameType(cube) == true)
                    {
                        continue;
                    }
                    else 
                    {
                        for (int k = MaxCount - 2; k >= j; k--)
                        {
                            if (LinkedHead[k] != null)
                            {
                                LinkedHead[k + 1] = LinkedHead[k];
                                LinkedHead[k].MovePosition(platform[k + 1].position);
                            }
                        }

                        cube.MovePosition(platform[j].position);
                        LinkedHead[j] = cube;
                        CurrentCount++;
                        return;
                    }
                }
            }

        }
    }


    public void Matched()
    {
        if (CurrentCount < 3)
        {
            return;
        }
        int late = 0;
        int fast = 0;
        while (fast < CurrentCount)
        {
            if (matchedList.Count == 3)
            {
                break;
            }
            if (LinkedHead[fast] != null && LinkedHead[fast].IsSameType(LinkedHead[late]) == true)
            {
                matchedList.Add(LinkedHead[fast]);
                fast++;
                continue;
            }
            else
            {
                if (LinkedHead[fast] == null)
                {
                    matchedList.Clear();
                    break;
                }
                late = fast;
                matchedList.Clear();
            }            
        }
        if (matchedList.Count < 3)
        {
            matchedList.Clear();
        }
    }
}

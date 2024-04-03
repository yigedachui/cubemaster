using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CubeEditorData
{
    public int x;
    public int y;
    public int z;
    public int total;

    public int[][][] cubeMap;
    public GameObject[][][] cubesObj;

    public int this[int x, int y, int z]
    {
        get
        {
            if (x > this.x || y > this.y || z > this.z)
            {
                throw new IndexOutOfRangeException();
            }
            return cubeMap[x][y][z];
        }
        set
        {
            if (x > this.x || y > this.y || z > this.z)
            {
                throw new IndexOutOfRangeException();
            }
            cubeMap[x][y][z] = value;
        }
    }

    public bool Init(int x,int y,int z)
    {
        bool canInit = x > 0 && y > 0 && z > 0;
        if (canInit)
        {
            this.x = x;
            this.y = y;
            this.z = z;

            cubeMap = new int[x][][];
            cubesObj = new GameObject[x][][];

            for (int i = 0; i < x; i++)
            {
                cubeMap[i] = new int[y][];
                cubesObj[i] = new GameObject[y][];
                for (int j = 0; j < y; j++)
                {
                    cubeMap[i][j] = new int[z];
                    cubesObj[i][j] = new GameObject[z];
                }
            }
        }

        return canInit;
    }

}

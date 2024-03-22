using UnityEngine;

public class Cube : MonoBehaviour
{
    public CubeData data;
    public Renderer render;

    private void Awake()
    {
        data = new CubeData();
        render = GetComponent<Renderer>();
    }

    public void SetCubeColor()
    {
        switch (data.type)
        {
            case CubeData.Type.None:

                break;
            case CubeData.Type.Red:
                data.color = Color.red;
                break;
            case CubeData.Type.Green:
                data.color = Color.green;
                break;
            case CubeData.Type.Blue:
                data.color = Color.blue;
                break;
            case CubeData.Type.Black:
                data.color = Color.black;
                break;
            case CubeData.Type.Grey:
                data.color = Color.grey;
                break;
            case CubeData.Type.Cyan:
                data.color = Color.cyan;
                break;
            case CubeData.Type.White:
                data.color = Color.white;
                break;
            case CubeData.Type.Magenta:
                data.color = Color.magenta;
                break;
            case CubeData.Type.Yellow:
                data.color = Color.yellow;
                break;
        }

        render.material.color = data.color;
    }

}

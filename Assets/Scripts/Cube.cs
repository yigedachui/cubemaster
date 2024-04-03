using UnityEngine;
using DG.Tweening;

public class Cube : MonoBehaviour
{
    public CubeData data;
    public Renderer render;

    private void Awake()
    {
        data = new CubeData();
        render = GetComponent<Renderer>();
    }

    public bool IsSameType(Cube cube)
    {
        return cube.data.type == this.data.type;
    }

    public void MovePosition(Vector3 target)
    {
        transform.SetParent(null);
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        transform.localRotation = Quaternion.identity;
        //Quaternion q = transform.localRotation;
        //transform.localRotation = Quaternion.Lerp(q, Quaternion.identity,0.3f);  
        transform.DOMove(target + new Vector3(0, 0.5f, 0), 0.5f);
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
        //ColorUtility.TryParseHtmlString(hexColor, out color);
        render.material.color = data.color;
    }
}

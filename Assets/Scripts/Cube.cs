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
        return cube.data.color == this.data.color;
    }

    public void MovePosition(Vector3 target)
    {
        transform.SetParent(null);
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        transform.DOMove(target + new Vector3(0, 0.5f, 0), 0.5f);
    }

    public void SetCubeColor(string hexColor)
    {
        Color color;
        ColorUtility.TryParseHtmlString(hexColor, out color);
        data.color = color;
        render.material.color = data.color;
    }
}

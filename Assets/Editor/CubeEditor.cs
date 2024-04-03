using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

public class CubeEditor : EditorWindow
{
    public int x_num;
    public int y_num;
    public int z_num;

    public GUISkin mGuiSkin;
    public GUIStyle mSelectedStyle;
    public GUIStyle mNormalStyle;

    private CubeEditorData mCubeData;
    private Vector2 mScrollPosition = Vector2.zero;
    private bool mInited = false;
    private GameObject mCubeObj;

    [MenuItem("Cube/OpenEditor")]
    public static void Open()
    {
        EditorWindow window = GetWindow<CubeEditor>();
        window.Show();
    }

    private void OnEnable()
    {
        mGuiSkin = Resources.Load<GUISkin>("GameSettingsCreatorSkin");
        mSelectedStyle = mGuiSkin.GetStyle("Selected");
        mNormalStyle = mGuiSkin.GetStyle("Normal");

        mCubeObj = Resources.Load<GameObject>("cube");
    }
    bool active = false;
    private void OnGUI()
    {
        mScrollPosition = GUILayout.BeginScrollView(mScrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("创建", GUILayout.Width(150), GUILayout.Height(50)))
                {
                    BeginEdit();
                }

                if (GUILayout.Button("导入", GUILayout.Width(150), GUILayout.Height(50)))
                {

                }
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            if (mCubeData == null)
            {
                GUILayout.EndScrollView();
                return;
            }

            x_num = EditorGUILayout.IntField("x:", x_num);
            y_num = EditorGUILayout.IntField("y:", y_num);
            z_num = EditorGUILayout.IntField("z:", z_num);
           

            if (GUILayout.Button("生成"))
            {
                mInited = mCubeData.Init(x_num, y_num, z_num);
            }

            if(!mInited)
            {
                GUILayout.EndScrollView();
                return;
            }


            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < y_num; i++)
            {
                GUILayout.TextArea(string.Format("第{0}层", i + 1));
            }
            EditorGUILayout.EndHorizontal();

            for (int x = 0; x < x_num; x++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int y = 0; y < y_num; y++)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int z = 0; z < z_num; z++)
                    {
                        bool selected = mCubeData[x, y, z] == 1;
                        if (GUILayout.Button(string.Format("{0},{1}", x, z), selected ? mSelectedStyle : mNormalStyle))
                        {
                            mCubeData[x, y, z] = selected ? 0 : 1;
                            Draw();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        GUILayout.EndScrollView();
    }

    private void BeginEdit()
    {
        mInited = false;
        x_num = y_num = z_num = 0;
        mCubeData = new CubeEditorData();
        for (int x = 0; x < x_num; x++)
        {
            for (int y = 0; y < y_num; y++)
            {
                for (int z = 0; z < z_num; z++)
                {
                    if (mCubeData[x, y, z] == 1)
                    {
                        Destroy(mCubeData.cubesObj[x][y][z]);
                    }

                }
            }
        }
    }

    private void Draw()
    {
        for (int x = 0; x < x_num; x++)
        {
            for (int y = 0; y < y_num; y++)
            {
                for (int z = 0; z < z_num; z++)
                {
                    if (mCubeData[x,y,z] == 1)
                    {
                        GameObject obj = Instantiate(mCubeObj);
                        obj.transform.position = new Vector3(x, y, z);
                        mCubeData.cubesObj[x][y][z] = obj;
                    }
                    else
                    {

                    }
                }
            }
        }
    }
}

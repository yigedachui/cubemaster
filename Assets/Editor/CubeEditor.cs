using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;


public class CubeEditor : EditorWindow
{
    public int x_num;
    public int y_num;
    public int z_num;
    private int count;
    public string TotalCount;

    public GUISkin mGuiSkin;
    public GUIStyle mSelectedStyle;
    public GUIStyle mNormalStyle;

    private CubeEditorData mCubeData;
    private Vector2 mScrollPosition = Vector2.zero;
    private Vector2 mScrollImportedSize = Vector2.zero;
    private bool mInited = false;
    private bool generated = false;
    private GameObject mCubeObj;
    public GameObject[][][] cubesObj;

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

    private void OnGUI()
    {
        mScrollPosition = GUILayout.BeginScrollView(mScrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("创建", GUILayout.Width(150), GUILayout.Height(50)))                    
                {
                    generated = false;
                    BeginEdit();
                }

                if (GUILayout.Button("导入", GUILayout.Width(150), GUILayout.Height(50)))
                {
                    string path = EditorUtility.OpenFilePanel("save", "Assets", "json");
                    if (path != null)
                    {
                        var json = File.ReadAllText(path);
                        mCubeData = JsonConvert.DeserializeObject<CubeEditorData>(json);
                        x_num = mCubeData.x;
                        y_num = mCubeData.y;
                        z_num = mCubeData.z;
                        count = mCubeData.total;
                        generated = true;
                        mInited = true;

                        cubesObj = new GameObject[x_num][][];
                        for (int x = 0; x < x_num; x++)
                        {
                            cubesObj[x] = new GameObject[y_num][];
                            for (int y = 0; y < y_num; y++)
                            {
                                cubesObj[x][y] = new GameObject[z_num];
                            }
                        }

                        for (int x = 0; x < x_num; x++)
                        {
                            for (int y = 0; y < y_num; y++)
                            {
                                for (int z = 0; z < z_num; z++)
                                {
                                    Draw(x, y, z);
                                }
                            }
                        }
                    }
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
            TotalCount = EditorGUILayout.TextArea(string.Format("方块数量：{0}", count));

            GUILayout.Space(10);
            if (GUILayout.Button("生成"))
            {
                mInited = mCubeData.Init(x_num, y_num, z_num);
                cubesObj = new GameObject[x_num][][];
                for (int x = 0; x < x_num; x++)
                {
                    cubesObj[x] = new GameObject[y_num][];
                    for (int y = 0; y < y_num; y++)
                    {
                        cubesObj[x][y] = new GameObject[z_num];
                    }
                }
            }
            GUILayout.Space(10);

            if (!mInited)
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
                            count += selected? -1 : 1;
                            mCubeData.total = count;
                            Draw(x, y, z);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(30);

        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUI.color = Color.red;
            if (GUILayout.Button("删除", GUILayout.Width(150), GUILayout.Height(50)))
            {
                BeginEdit();
            }
            GUI.color = Color.white;
            GUI.color = Color.green;
            if (GUILayout.Button("保存", GUILayout.Width(150), GUILayout.Height(50)))
            {
                // 弹出对话框
                if (count != 0 && count % 3 == 0)
                {
                    Save();
                }
                else
                {
                    EditorUtility.DisplayDialog("保存失败", string.Format("方块数量必须为3的倍数.{0}", count), "OK");
                }
            }
            GUI.color = Color.white;
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndScrollView();
    }

    private void BeginEdit()
    {
        for (int x = 0; x < x_num; x++)
        {
            for (int y = 0; y < y_num; y++)
            {
                for (int z = 0; z < z_num; z++)
                {
                    if (cubesObj != null && cubesObj[x][y][z])
                    {
                        DestroyImmediate(cubesObj[x][y][z]);
                        cubesObj[x][y][z] = null;
                    }
                }
            }
        }
        mInited = false;
        x_num = y_num = z_num = 0;
        count = 0;
        mCubeData = new CubeEditorData();
    }

    private void Draw(int x, int y, int z)
    {
        float xHalf = x_num / 2.0f;
        float yHalf = y_num / 2.0f;
        float zHalf = z_num / 2.0f;
        if (mCubeData[x, y, z] == 1 && cubesObj[x][y][z] == null)
        {
            GameObject obj = Instantiate(mCubeObj);
            obj.transform.position = new Vector3(x - xHalf + 0.5f, y - yHalf + 0.5f, z - zHalf + 0.5f);
            cubesObj[x][y][z] = obj;
        }
        else
        {
            if (cubesObj != null && cubesObj[x][y][z]) 
            {
                DestroyImmediate(cubesObj[x][y][z]);
            }
        }
    }

    private void Save()
    {
        string json = JsonConvert.SerializeObject(mCubeData);

        string path = EditorUtility.SaveFilePanel("save", "Assets", "level", "json");

        File.WriteAllText(path, json);
    }
}

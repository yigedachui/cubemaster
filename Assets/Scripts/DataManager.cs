using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LevelInfo
{
    public int x;
    public int y;
    public int z;
    public int total;
    public int[][][] cubeMap;
}

public class DataManager
{
    const string Level = "Level_{0}";

    public List<string> colors;

    public static DataManager Instance;

    public void SaveLevel(int lv)
    {
        PlayerPrefs.SetInt(string.Format(Level, lv), lv);
        
    }

    public void Init()
    {
        colors = new List<string>();

        string str = Resources.Load<TextAsset>("CubeData").text;
        
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(str);

        XmlNodeList xmlNodeList = xmlDoc.ChildNodes;

       foreach (XmlNode xmlNode in xmlNodeList)
        {
            if (xmlNode.HasChildNodes)
            {
                foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
                {
                    colors.Add(xmlNode2.InnerText);
                    Debug.Log(xmlNode2.Name + ":" + xmlNode2.InnerText);
                }
                
            }            
        }

    }

    public void LoadLevelData()
    {

    }

}

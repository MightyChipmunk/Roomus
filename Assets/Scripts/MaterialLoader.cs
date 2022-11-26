using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MaterialLoader : MonoBehaviour
{
    public static MaterialLoader Instance { get; private set; } 

    Dictionary<string, string> dicImage = new Dictionary<string, string>();
    Dictionary<string, string> dicMatName = new Dictionary<string, string>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeMat(Transform obj, string path)
    {
        InitMaterial(path);

        MeshRenderer mr = null;
        foreach (Transform tr in obj)
        {
            mr = tr.GetComponent<MeshRenderer>();
            for (int i = 0; i < mr.materials.Length; i++)
            {
                string key = GetMaterialName(mr.materials[i].name);
                if (dicMatName.ContainsKey(key))
                    SetMaterialInfo(mr.materials[i], dicMatName[key]);
            }
        }
    }

    string GetMaterialName(string originName)
    {
        return originName.Replace("_", "").Replace(" ", "").Replace("(Instance)", "");
    }


    void InitMaterial(string path)
    {
        string[] filePaths = Directory.GetFiles(path, "*.meta");
        
        for (int i = 0; i < filePaths.Length; i++)
        {
            foreach (string line in File.ReadLines(filePaths[i]))
            {
                if (line.Contains("guid"))
                {
                    string key = line.Replace("guid:", "").Trim();
                    dicImage[key] = filePaths[i].Replace(".meta", "");
                    break;
                }
            }
        }

        filePaths = Directory.GetFiles(path, "*.mat");
        for (int i = 0; i < filePaths.Length; i++)
        {
            string key = GetMaterialName(Path.GetFileName(filePaths[i]).Replace(".mat", ""));
            dicMatName[key] = filePaths[i];
        }
    }

    void SetMaterialInfo(Material m, string path)
    {
        bool noMetal = false;
        string matString = File.ReadAllText(path);
        string[] matLines = matString.Split("\n");
        m.SetFloat("_SmoothnessTextureChannel", 1);
        for (int i = 0; i < matLines.Length; i++)
        {
            if (matLines[i].Contains("WorkflowMode:"))
            {
                m.SetFloat("_WorkflowMode", GetSigle(matLines[i]));
            }

            if (matLines[i].Contains("SmoothnessTextureChannel:"))
            {
                m.SetFloat("_SmoothnessTextureChannel", GetSigle(matLines[i]));
            }

            if (matLines[i].Contains("OcclusionStrength:"))
            {
                m.SetFloat("_OcclusionStrength", GetSigle(matLines[i]));
            }

            //if (matLines[i].Contains("Blend:"))
            //{
            //    m.SetFloat("_Blend", GetSigle(matLines[i]));
            //}

            if (matLines[i].Contains("Surface:"))
            {
                m.SetFloat("_Surface", GetSigle(matLines[i]));
            }

            if (matLines[i].Contains("AlphaClip:"))
            {
                m.SetFloat("_AlphaClip", GetSigle(matLines[i]));
            }

            if (matLines[i].Contains("ReceiveShadows:"))
            {
                m.SetFloat("_ReceiveShadows", GetSigle(matLines[i]));
            }

            //Metallic
            if (matLines[i].Contains("Metallic:"))
            {
                m.SetFloat("_Metallic", GetSigle(matLines[i]));
            }

            //Smoothness
            else if (matLines[i].Contains("Smoothness:"))
            {
                m.SetFloat("_Smoothness", GetSigle(matLines[i]));
            }
            //Parallax
            else if (matLines[i].Contains("Parallax:"))
            {
                m.SetFloat("_Parallax", GetSigle(matLines[i]));
            }

            //BaseMap
            else if (matLines[i].Contains("BaseMap:"))
            {
                //m.SetTexture("_BaseMap", GetTexture(matLines[i + 1]));

                if (matLines[i + 2].Contains("Scale"))
                {
                    //m.mainTextureScale = GetScale(matLines[i + 2]);
                    m.SetTextureScale("_BaseMap", GetScale(matLines[i + 2]));
                }                
            }
            //BumpMap (Normal)
            //else if (matLines[i].Contains("BumpMap:"))
            //{
            //    m.SetTexture("_BumpMap", GetTexture(matLines[i + 1]));
            //}
            //MetallicMap
            else if (matLines[i].Contains("MetallicGlossMap"))
            {
                if (GetTexture(matLines[i + 1]) == null)
                {
                    noMetal = true;
                    continue;
                }
                m.SetTexture("_MetallicGlossMap", GetTexture(matLines[i + 1]));
            }
            //ParallaxMap (Height)
            else if (matLines[i].Contains("ParallaxMap:"))
            {
                if (GetTexture(matLines[i + 1]) == null)
                    continue;
                m.SetTexture("_ParallaxMap", GetTexture(matLines[i + 1]));
            }
            //OcclusionMap 
            else if (matLines[i].Contains("OcclusionMap:"))
            {
                if (GetTexture(matLines[i + 1]) == null)
                    continue;
                m.SetTexture("_OcclusionMap", GetTexture(matLines[i + 1]));
            }
            //BaseColor
            else if (matLines[i].Contains("BaseColor:"))
            {
                m.SetColor("_BaseColor", GetColor(matLines[i]));
            }
            //EmissionColor
            else if (matLines[i].Contains("EmissionColor:"))
            {
                m.SetColor("_EmissionColor", GetColor(matLines[i]));
            }
            //SpecColor
            else if (matLines[i].Contains("SpecColor:"))
            {
                m.SetColor("_SpecColor", GetColor(matLines[i]));
            }


        }

        if (noMetal)
            m.SetFloat("_SmoothnessTextureChannel", 1);
    }

    Texture2D GetTexture(string line)
    {
        Texture2D map = null;
        if (line.Contains("guid"))
        {
            int s = line.IndexOf("guid: ") + 6;
            int e = line.IndexOf(", type");
            string guid = line.Substring(s, e - s);
            byte[] img = null;
            if (dicImage.ContainsKey(guid))
                img = File.ReadAllBytes(dicImage[guid]);
            //Debug.Log(dicImage[guid]);
            map = new Texture2D(2, 2, TextureFormat.RGB24, false);
            if (img != null)
            {
                map.LoadImage(img);
                map.Apply();
            }
        }
        return map;
    }

    Color GetColor(string line)
    {
        int s = line.IndexOf("{") + 1;
        int e = line.IndexOf("}");
        string color = line.Substring(s, e - s).Replace(" ", "");
        color = color.Replace("r:", "").Replace("g:", "").Replace("b:", "").Replace("a:", "");
        string[] colors = color.Split(",");

        Color c = new Color(float.Parse(colors[0]), float.Parse(colors[1]), float.Parse(colors[2]), float.Parse(colors[3]));
        return c;
    }

    float GetSigle(string line)
    {
        int s = line.IndexOf(":") + 1;
        string value = line.Substring(s).Trim();
        return float.Parse(value);
    }

    Vector2 GetScale(string line)
    {
        int s = line.IndexOf("{") + 1;
        int e = line.IndexOf("}");
        string scale = line.Substring(s, e - s).Replace(" ", "");
        scale = scale.Replace("x:", "").Replace("y:", "");
        string[] scales = scale.Split(",");
        return new Vector2(float.Parse(scales[0]), float.Parse(scales[1]));

    }
}

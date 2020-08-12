using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class MaterialConverter : ScriptableWizard
{
    [MenuItem("Tools/Material Serializer")]
    static void Open()
    {
        MaterialConverter.DisplayWizard("Material Serializer", typeof(MaterialConverter), "Save");
    }

    public Material material;

    public string[] textures = new[]
    {
        "_MainTex",
        "_SpecGlossMap",
        "_BumpMap",
        "_OcclusionMap",
        "_DetailMask",
        "_DetailAlbedoMap",
        "_DetailNormalMap",
        "_ParallaxMap",
    };

    public string[] floats = new[]
    {
        "_Cutoff",
        "_Glossiness"
    };

    // public string[] vectors = new[]
    // {
    //     "_BumpScale",
    //     "_GlossMapScale",
    // };

    public string[] colors = new[]
    {
        "_SpecColor",
    };

    void OnWizardUpdate()
    {
        isValid = material != null;
    }

    void OnWizardCreate()
    {
        var path = EditorUtility.SaveFilePanelInProject("Save", $"{material.name}.material", "material",
            "Save location",
            "Assets/StreamingAssets/Materials");

        Shader shader = material.shader;
        var info = new SerializedMaterial(shader.name);

        foreach (var texture in textures)
        {
            var tex = material.GetTexture(texture);
            if (tex != null)
                info.textureProperties[texture] = AssetDatabase.GetAssetPath(tex);
        }

        foreach (var f in floats)
        {
            info.floatProperties[f] = material.GetFloat(f);
        }

        // foreach (var v in vectors)
        // {
        //     info.vectorProperties[v] = material.GetVector(v);
        // }
        
        foreach (var v in colors)
        {
            info.colorProperties[v] = material.GetColor(v);
        }
        
        File.WriteAllText(path, JsonConvert.SerializeObject(info));
        AssetDatabase.ImportAsset(path);
        AssetDatabase.Refresh();
        
        
        
        // AssetDatabase.CreateAsset(, path);
    }
}

[Serializable]
public class SerializedMaterial
{
    public string shader;
    public Dictionary<string, float> floatProperties;
    public Dictionary<string, Vector4> vectorProperties;
    public Dictionary<string, Vector4> colorProperties;
    public Dictionary<string, string> textureProperties;

    public SerializedMaterial(string shader)
    {

        this.shader = shader;
        
        floatProperties = new Dictionary<string, float>();
        vectorProperties = new Dictionary<string, Vector4>();
        textureProperties = new Dictionary<string, string>();
        colorProperties = new Dictionary<string, Vector4>();
    }

    public Material Deserialize()
    {
        var mat = new Material(Shader.Find(shader));
        var mtl = new MTLLoader();

        foreach (var pair in floatProperties)
        {
            mat.SetFloat(pair.Key, pair.Value);
        }
        
        // foreach (var pair in vectorProperties)
        // {
        //     mat.SetVector(pair.Key, pair.Value);
        // }
        
        foreach (var pair in colorProperties)
        {
            mat.SetVector(pair.Key, pair.Value);
        }
        
        foreach (var pair in textureProperties)
        {
            mat.SetTexture(pair.Key, mtl.TryLoadTexture(pair.Value));
        }

        return mat;
    }
}

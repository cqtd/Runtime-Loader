using Dummiesman;
using UnityEditor;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public string objName;
    
    // Start is called before the first frame update
    void Start()
    {
        var path = Application.streamingAssetsPath + "/Meshes/";
        
        Debug.Log($"{path}{objName}.obj");
        
        var objLoader = new OBJLoader();
        var a = objLoader.Load($"{path}{objName}.obj");

        var tex = new MTLLoader().TryLoadTexture($"{Application.streamingAssetsPath}/Textures/Apple_Albedo.png");
        tex.filterMode = FilterMode.Point;
        
        var mat = new Material(Shader.Find("Standard"));
        mat.SetTexture("_MainTex", tex);

        a.GetComponentInChildren<MeshRenderer>().sharedMaterial = mat;
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateModel
{
    /// <summary>
    /// according project data to create hierarchical structure
    /// </summary>
    /// <param name="pd"></param>
    /// <returns></returns>
    public static GameObject GenerateSpatialStructure(IProjectData pd)
    {
        foreach(var spatial in pd.SubSpatials)
        {
            spatial.ThisGameObject.transform.parent = pd.ThisGameObject.transform;
            FindSubSpatialStructure(spatial);
        }
        return pd.ThisGameObject;
    }

    /// <summary>
    /// according spatial data to create hierarchical structure
    /// </summary>
    /// <param name="sd"></param>
    /// <returns></returns>
    private static ISpatialData FindSubSpatialStructure(ISpatialData sd)
    {
        HashSet<string> typeName = new HashSet<string>();
        foreach (var p in sd.SubProducts)
        {
            if (!typeName.Contains(p.TypeName))
            {
                typeName.Add(p.TypeName);
                var go = new GameObject(p.TypeName);
                go.transform.parent = sd.ThisGameObject.transform;
            }
            p.ThisGameObject.transform.parent = sd.ThisGameObject.transform.Find(p.TypeName);
            if (p.DecomposedProducts.Count > 0)
            {
                foreach(var dp in p.DecomposedProducts)
                {
                    dp.ThisGameObject.transform.parent = p.ThisGameObject.transform;
                }
            }
        }
        if (sd.SubSpatials.Count == 0)
        {
            return sd;
        }
        foreach(var ss in sd.SubSpatials)
        {
            ss.ThisGameObject.transform.parent = sd.ThisGameObject.transform;
            FindSubSpatialStructure(ss);
        }
        return sd;
    }

    /// <summary>
    /// accoring MyBimProduct to create gameobject
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public static GameObject GenerateProduct(MyBimProduct product)
    {
        var go = new GameObject()
        {
            name = product.entityLabel.ToString()
        };
        var data = go.AddComponent<ProductData>();
        data.ProductGeoData = product;
        SomeValue.products.Add(data);
        foreach (var si in product.shapeInstances)
            GenerateShapeInstance(si, go);
        return go;
    }

    /// <summary>
    /// create mesh and append them to productGO
    /// </summary>
    /// <param name="si"></param>
    /// <param name="productGO"></param>
    private static void GenerateShapeInstance(MyBimShapeInstance si, GameObject productGO)
    {
        GameObject shapeGO = new GameObject(si.instanceLabel.ToString());
        shapeGO.transform.parent = productGO.transform;

        var mf = shapeGO.AddComponent<MeshFilter>();
        var mesh = mf.mesh;
        foreach (var tri in si.triangulations)
        {
            mesh.vertices = tri.vertices.ToArray();
            mesh.triangles = tri.triangles.ToArray();
            mesh.normals = tri.normals.ToArray();
            mesh.Optimize();
        }

        var mr = shapeGO.AddComponent<MeshRenderer>();
        var mat = new Material(Shader.Find("Standard"));
        var c = MyBimGeomorty.colors.Find(cl => cl.styleLabel == si.styleLabel);
        mat.color = c.color;
        mr.material = mat;

        if (c.color.a < 0.9f)
            SetMaterialRenderingMode(mat, RenderingMode.Transparent);
        else
            SetMaterialRenderingMode(mat, RenderingMode.Opaque);
    }

    public enum RenderingMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent,
    }

    /// <summary>
    /// set alpha value of some transparent product
    /// </summary>
    /// <param name="material"></param>
    /// <param name="renderingMode"></param>
    public static void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }


    public static void AppendCollider(IProductData product)
    {
        if (product.ProductGeoData.shapeInstances.Count == 0)
            return;

        var mc = product.ThisGameObject.AddComponent<MeshCollider>();
        CombineInstance[] combimes = new CombineInstance[product.ProductGeoData.shapeInstances.Count];
        var mesh = new Mesh();

        var thisTransform = product.ThisGameObject.transform;
        int j = 0;
        for(int i = 0; i < thisTransform.childCount; i++)
        {
            if (thisTransform.GetChild(i).GetComponent<MeshRenderer>() != null)
            {
                combimes[j].mesh = thisTransform.GetChild(i).GetComponent<MeshFilter>().mesh;
                combimes[j].transform = thisTransform.GetChild(i).localToWorldMatrix;
                j++;
            }
        }
        mesh.CombineMeshes(combimes);
        mesh.name = product.ProductName;
        mc.sharedMesh = mesh;
        mc.convex = true;
    }
}

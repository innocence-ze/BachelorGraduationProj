using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateModel
{
    /*
    /// <summary>
    /// according project data to create hierarchical structure
    /// </summary>
    /// <param name="pd"></param>
    /// <returns></returns>
    public static GameObject GenerateRelatedProducts(IProjectData pd)
    {
        foreach(var spatial in pd.RelatedProducts)
        {
            spatial.ThisGameObject.transform.parent = pd.ThisGameObject.transform;
            AppendRelatedProducts(spatial);
        }
        return pd.ThisGameObject;
    }

    /// <summary>
    /// according spatial data to create hierarchical structure
    /// </summary>
    /// <param name="sd"></param>
    /// <returns></returns>
    private static ISpatialData AppendRelatedProducts(ISpatialData sd)
    {
        HashSet<string> typeName = new HashSet<string>();
        int spatialCount = 0;
        foreach (var p in sd.RelatedProducts)
        {
            if(p is IElementData)
            {
                if (!typeName.Contains(p.TypeName))
                {
                    typeName.Add(p.TypeName);
                    var go = new GameObject(p.TypeName);
                    go.transform.parent = sd.ThisGameObject.transform;
                }
                p.ThisGameObject.transform.parent = sd.ThisGameObject.transform.Find(p.TypeName);
                if (p.RelatedProducts.Count > 0)
                {
                    foreach (var dp in p.RelatedProducts)
                    {
                        dp.ThisGameObject.transform.parent = p.ThisGameObject.transform;
                        dp.RelatingProduct = p;
                    }
                }
            }
            else if(p is ISpatialData)
            {
                spatialCount++;
                p.ThisGameObject.transform.parent = sd.ThisGameObject.transform;
                AppendRelatedProducts((ISpatialData)p);
            }
            p.RelatingProduct = sd;
        }
        if (spatialCount == 0)
        {
            return sd;
        }
        return sd;
    }
    */

    /// <summary>
    /// accoring MyBimProduct to create gameobject
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static IProductData GenerateProduct(MyBimProduct element, List<IProductData> allPD)
    {
        /*var go = new GameObject()
        {
            name = element.entityLabel.ToString()
        };
        var data = go.AddComponent<ElementData>();
        data.ProductGeoData = element;
        SomeValue.Elements.Add(data);

        var prodCtrl = go.AddComponent<ProductController>();
        prodCtrl.id = data.ProductGeoData.entityLabel;

        foreach (var si in element.shapeInstances)
            GenerateShapeInstance(si, go);
        return go;*/
        var productData = allPD.First(e => e.EntityLabel == element.entityLabel);
        if(!(productData is ISpaceData))
            productData.ProductGeoData = element;
        allPD.Remove(productData);

        var prodCtrl = productData.ThisGameObject.AddComponent<ProductController>();
        prodCtrl.id = productData.EntityLabel;

        foreach (var si in element.shapeInstances)
            GenerateShapeInstance(si, productData.ThisGameObject);

        return productData;
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
        //var mat = new Material(Shader.Find("Standard"));
        var c = MyBimGeomorty.colors.Find(cl => cl.styleLabel == si.styleLabel);
        //mat.color = c.color;
        mr.material = c.mat;

        if (c.color.a < 0.9f)
            SetMaterialRenderingMode(c.mat, RenderingMode.Transparent);
        else
            SetMaterialRenderingMode(c.mat, RenderingMode.Opaque);
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
        if (product.ProductGeoData.Equals(MyBimProduct.Default) || product.ProductGeoData.shapeInstances.Count == 0)
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
        mesh.name = product.Name;
        mc.sharedMesh = mesh;
        if(product is IElementData)
        {
            IElementData ele = product as IElementData;
            if(ele.HasOpening == false)
            {
                mc.convex = true;
            }
        }
    }
}

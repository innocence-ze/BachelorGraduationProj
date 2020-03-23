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
        foreach (var p in sd.SubProducts)
        {
            p.ThisGameObject.transform.parent = sd.ThisGameObject.transform;
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
            //mesh.normals = tri.normals.ToArray();
            mesh.triangles = tri.triangles.ToArray();
            mesh.Optimize();
            mesh.RecalculateNormals();
        }

        var mr = shapeGO.AddComponent<MeshRenderer>();
        var mat = new Material(Shader.Find("Standard"));
        var c = MyBimGeomorty.colors.Find(cl => cl.styleLabel == si.styleLabel);
        mat.color = c.color;
        mr.material = mat;
    }



}

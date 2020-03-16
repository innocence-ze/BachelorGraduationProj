using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

public class GenerateModel
{

    public static GameObject GenerateFloor()
    {
        return null;
    }

    public static GameObject GenerateProduct(MyBimProduct product)
    {
        var go = new GameObject()
        {
            name = product.entityLabel.ToString()
        };
        var data = go.AddComponent<ProductData>();
        data.ProductGeoData = product;
        foreach (var si in product.shapeInstances)
            GenerateShapeInstance(si, go);
        return go;
    }

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
        var c = ReadWexBim.colors.Find(cl => cl.styleLabel == si.styleLabel);
        mat.color = c.color;
        mr.material = mat;
        //shapeGO.AddComponent<test>();
    }


    public void Show(string file)
    {
        using (var model = IfcStore.Open(file))
        {
            var project = model.Instances.FirstOrDefault<IIfcProject>();
            PrintHierarchy(project, 0);
        }
    }

    private void PrintHierarchy(IIfcObjectDefinition o, int level)
    {
        Debug.Log(string.Format("{0}{1} [{2}]", GetIndent(level), o.Name, o.GetType().Name));

        //只有空间元素可以包含建筑元素
        var spatialElement = o as IIfcSpatialStructureElement;
        if (spatialElement != null)
        {
            GameObject go = new GameObject(o.Name);
            //使用 IfcRelContainedInSpatialElement 获取包含的元素
            var containedElements = spatialElement.ContainsElements.SelectMany(rel => rel.RelatedElements);
            foreach (var element in containedElements)
            {
                Debug.Log(string.Format("{0}    ->{1} [{2}]", GetIndent(level), element.Name, element.GetType().Name));
            }
        }
        //利用 IfcRelAggregares 获取空间结构元素的空间分解
        foreach (var item in o.IsDecomposedBy.SelectMany(r => r.RelatedObjects))
            PrintHierarchy(item, level + 1);
    }

    private string GetIndent(int level)
    {
        var indent = "";
        for (int i = 0; i < level; i++)
            indent += "    ";
        return indent;
    }
}

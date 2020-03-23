using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Common.Geometry;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.ProductExtension;

public static class MyBimGeomorty
{
    public static List<MyBimRegion> regions = new List<MyBimRegion>();
    public static List<MyBimColor> colors = new List<MyBimColor>();
    public static List<MyBimProduct> products = new List<MyBimProduct>();
    public static List<MyBimShapeInstance> shapeInstances = new List<MyBimShapeInstance>();
    public static List<MyBimTriangulation> triangulations = new List<MyBimTriangulation>();
}

public struct MyBimRegion
{
    public int population;
    public Vector3 position;
    public Vector3 scale;
    public XbimRegion xbimRegion;
    public MyBimRegion(int population, float px, float py, float pz,float sx,float sy,float sz)
    {
        this.population = population;
        this.position = new Vector3(px, py, pz);
        this.scale = new Vector3(sx, sy, sz);
        xbimRegion = new XbimRegion()
        {
            Population = population,
            Centre = new XbimPoint3D(px, py, pz),
            Size = new XbimVector3D(sx, sy, sz)
        };
    }
}

public struct MyBimColor
{
    public int styleLabel;
    public float r, g, b, a;
    public Color color;
    public XbimColour xbimColour;
    public MyBimColor(int label, float r, float g, float b, float a = 1)
    {
        styleLabel = label;
        this.r = r; this.g = g;
        this.b = b; this.a = a;
        color = new Color(r, g, b, a);
        xbimColour = new XbimColour(label, r, g, b, a);
    }
}

[Serializable]
public struct MyBimProduct
{
    public int entityLabel;
    public short typeId;
    //public Vector3 position;
    //public Vector3 scale;
    public List<MyBimShapeInstance> shapeInstances;
    public MyBimProduct(int label, short type)
    {
        entityLabel = label;
        typeId = type;
        shapeInstances = new List<MyBimShapeInstance>();
    }

    public void AddShapeInstance(MyBimShapeInstance si)
    {
        shapeInstances.Add(si);
    }
}

[Serializable]
public struct MyBimShapeInstance
{
    public XbimShapeInstance xbimShapeInstance;
    public int productLabel;
    public short typeId;
    public int instanceLabel;
    public int styleLabel;
    public List<MyBimTriangulation> triangulations;
    public XbimMatrix3D transform;
    public MyBimShapeInstance(int product, short type, int instanceLabel, int styleLabel, XbimMatrix3D matrix = default)
    {
        this.productLabel = product;
        this.typeId = type;
        this.instanceLabel = instanceLabel;
        this.styleLabel = styleLabel;
        this.triangulations = new List<MyBimTriangulation>();
        transform = matrix;
        xbimShapeInstance = new XbimShapeInstance(this.instanceLabel)
        {
            IfcProductLabel = productLabel,
            IfcTypeId = typeId,
            StyleLabel = this.styleLabel,
            Transformation = transform
        };    
    }

    public void AddTriangulation(MyBimTriangulation triang)
    {
        triangulations.Add(triang);
    }
}

[Serializable] 
public struct MyBimTriangulation
{
    public XbimShapeTriangulation bimTriangulation;
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector3> normals;
    public MyBimTriangulation(XbimShapeTriangulation triangulation, Vector3 offsite, float scale, XbimMatrix3D matrix = default, bool bMatrix = false)
    {
        if (bMatrix)
        {
            bimTriangulation = triangulation.Transform(matrix);
        }
        else
            bimTriangulation = triangulation;

        vertices = new List<Vector3>();
        triangles = new List<int>();
        normals = new List<Vector3>();

        foreach (var v in bimTriangulation.Vertices)
        {
            vertices.Add(new Vector3((float)v.X, (float)v.Y, (float)v.Z) / scale - offsite);
        }
        foreach (var f in bimTriangulation.Faces)
        {
            triangles.AddRange(f.Indices);
            foreach (var n in f.Normals)
            {
                var nor = new Vector3((float)n.Normal.X, (float)n.Normal.Y, (float)n.Normal.Z);
                normals.Add(nor);
            }
        }
    }
}

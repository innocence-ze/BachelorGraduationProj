using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Xbim.Common.Geometry;
using Xbim.Common.XbimExtensions;

public class ReadWexBim
{
    public static List<MyBimRegion> regions = new List<MyBimRegion>();
    public static List<MyBimColor> colors = new List<MyBimColor>();
    public static List<MyBimProduct> products = new List<MyBimProduct>();
    public static List<MyBimShapeInstance> shapeInstances = new List<MyBimShapeInstance>();
    public static List<MyBimTriangulation> triangulations = new List<MyBimTriangulation>();

    public static void ReadWexbimFile(string fileName)
    {
        Vector3 offsite;
        using(var fs = new FileStream(fileName,FileMode.Open,FileAccess.Read))
        {
            using(var br = new BinaryReader(fs))
            {
                var magicalNum = br.ReadInt32();
                var version = br.ReadByte();
                var shapeCount = br.ReadInt32();
                var vertexCount = br.ReadInt32();
                var triangleCount = br.ReadInt32();
                var matrixCount = br.ReadInt32();
                var productCount = br.ReadInt32();
                var styleCount = br.ReadInt32();
                var scale = br.ReadSingle();
                var regionCount = br.ReadInt16();

                //Region
                for (int i = 0; i < regionCount; i++)
                {
                    var population = br.ReadInt32();
                    var centreX = br.ReadSingle(); centreX /= scale;
                    var centreY = br.ReadSingle(); centreY /= scale;
                    var centreZ = br.ReadSingle(); centreZ /= scale;
                    var boundsBytes = br.ReadBytes(6 * sizeof(float));
                    var bounds = XbimRect3D.FromArray(boundsBytes);
                    bounds.X /= scale; bounds.Y /= scale; bounds.Z /= scale;
                    bounds.SizeX /= scale; bounds.SizeY /= scale; bounds.SizeZ /= scale;
                    regions.Add(new MyBimRegion(population, centreX, centreY, centreZ, (float)bounds.SizeX, (float)bounds.SizeY, (float)bounds.SizeZ));
                }
                offsite = regions[0].position;

                //texture
                for (int i = 0; i < styleCount; i++)
                {
                    var styleId = br.ReadInt32();
                    var red = br.ReadSingle();
                    var green = br.ReadSingle();
                    var blue = br.ReadSingle();
                    var alpha = br.ReadSingle();
                    colors.Add(new MyBimColor(styleId, red, green, blue, alpha));
                }

                //product
                for (int i = 0; i < productCount; i++)
                {
                    var entityLabel = br.ReadInt32();
                    var typeId = br.ReadInt16();
                    var boxBytes = br.ReadBytes(6 * sizeof(float));
                    XbimRect3D bb = XbimRect3D.FromArray(boxBytes);
                    //float x = (float)bb.X, y = (float)bb.Y, z = (float)bb.Z;
                    //float sizeX = (float)bb.SizeX, sizeY = (float)bb.SizeY, sizeZ = (float)bb.SizeZ;
                    products.Add(new MyBimProduct(entityLabel, typeId));
                }

                //shape
                for (int i = 0; i < shapeCount; i++)
                {
                    var shapeRepetition = br.ReadInt32();
                    MyBimShapeInstance si;
                    if (shapeRepetition > 1)
                    {
                        List<MyBimShapeInstance> curShapeInstances = new List<MyBimShapeInstance>();
                        for (int j = 0; j < shapeRepetition; j++)
                        {
                            var ifcProductLabel = br.ReadInt32();
                            var ifcTypeId = br.ReadInt16();
                            var instanceLabel = br.ReadInt32();
                            var styleLabel = br.ReadInt32();
                            var transform = XbimMatrix3D.FromArray(br.ReadBytes(sizeof(double) * 16));

                            si = new MyBimShapeInstance(ifcProductLabel, ifcTypeId, instanceLabel, styleLabel, transform);
                            shapeInstances.Add(si);
                            curShapeInstances.Add(si);
                            var p = products.Find(product => product.entityLabel == ifcProductLabel);
                            p.AddShapeInstance(si);
                        }
                        var triangulation = br.ReadShapeTriangulation();

                        foreach (var csi in curShapeInstances)
                        {
                            var tri = new MyBimTriangulation(triangulation, offsite, scale, csi.transform, true);
                            csi.AddTriangulation(tri);
                            triangulations.Add(tri);
                        }
                    }
                    else if (shapeRepetition == 1)
                    {
                        var ifcProductLabel = br.ReadInt32();
                        var ifcTypeId = br.ReadInt16();
                        var instanceLabel = br.ReadInt32();
                        var styleLabel = br.ReadInt32();

                        si = new MyBimShapeInstance(ifcProductLabel, ifcTypeId, instanceLabel, styleLabel);
                        shapeInstances.Add(si);
                        var p = products.Find(product => product.entityLabel == ifcProductLabel);
                        p.AddShapeInstance(si);

                        XbimShapeTriangulation triangulation = br.ReadShapeTriangulation();
                        var tri = new MyBimTriangulation(triangulation, offsite, scale);
                        triangulations.Add(tri);

                        si.AddTriangulation(tri);
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;

public static class SomeValue
{
    public static string fileName;
    public static string WexbimFileName => fileName + ".wexBIM";
    public static string BimFileName => fileName + ".ifc";


    public static IProjectData project;
    public static List<IProductData> products= new List<IProductData>();
    public static List<ISpatialData> spatialStructures = new List<ISpatialData>();
    public static HashSet<Type> allTypes = new HashSet<Type>();
}

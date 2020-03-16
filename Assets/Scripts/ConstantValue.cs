using System;

public static class ConstantValue
{
    public static string fileName;
    public static string WexbimFileName => fileName + ".wexBIM";
    public static string BimFileName => fileName + ".ifc";
}

using System;

[Serializable]
public class ContourData
{
    public Contour[] contours;
}

[Serializable]
public class Contour
{
    public Vertex[] vertices;
}

[Serializable]
public class Vertex
{
    public float x;
    public float y;
}
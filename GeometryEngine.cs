
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace MEGAPLAN.Core
{
    public class GeometryEngine
    {
        public static Line CreateLine(Point3d p1, Point3d p2)
        {
            return new Line(p1, p2);
        }
    }
}

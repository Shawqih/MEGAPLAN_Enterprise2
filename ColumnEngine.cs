
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace MEGAPLAN.Core
{
    public class ColumnEngine
    {
        public static Circle CreateColumn(Point3d center, double radius)
        {
            return new Circle(center, Vector3d.ZAxis, radius);
        }
    }
}

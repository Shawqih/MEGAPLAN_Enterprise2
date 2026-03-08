
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace MEGAPLAN.Core
{
    public class GridEngine
    {
        public static Line CreateGridLine(Point3d start, Point3d end)
        {
            return new Line(start, end);
        }
    }
}

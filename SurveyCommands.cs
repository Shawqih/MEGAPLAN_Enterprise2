
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using System;

namespace MEGAPLAN_Enterprise.SurveyTools
{
    public class SurveyCommands
    {

        [CommandMethod("MEGA_SURFACE_ELEV")]
        public void GetSurfaceElevation()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            CivilDocument civDoc = CivilApplication.ActiveDocument;

            PromptEntityOptions peo = new PromptEntityOptions("\nSelect Surface:");
            peo.SetRejectMessage("\nMust be a Civil3D Surface.");
            peo.AddAllowedClass(typeof(TinSurface), true);

            PromptEntityResult res = ed.GetEntity(peo);
            if (res.Status != PromptStatus.OK) return;

            using (Transaction tr = doc.Database.TransactionManager.StartTransaction())
            {
                TinSurface surface = tr.GetObject(res.ObjectId, OpenMode.ForRead) as TinSurface;

                PromptPointResult ppr = ed.GetPoint("\nPick point:");
                if (ppr.Status != PromptStatus.OK) return;

                double elev = surface.FindElevationAtXY(ppr.Value.X, ppr.Value.Y);
                ed.WriteMessage("\nSurface Elevation: " + elev);

                tr.Commit();
            }
        }


        [CommandMethod("MEGA_LIST_SURFACES")]
        public void ListSurfaces()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            CivilDocument civDoc = CivilApplication.ActiveDocument;

            foreach (ObjectId id in civDoc.GetSurfaceIds())
            {
                using (Transaction tr = doc.Database.TransactionManager.StartTransaction())
                {
                    Surface s = tr.GetObject(id, OpenMode.ForRead) as Surface;
                    ed.WriteMessage("\nSurface: " + s.Name);
                    tr.Commit();
                }
            }
        }


        [CommandMethod("MEGA_SURVEY_INFO")]
        public void SurveyInfo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            CivilDocument civDoc = CivilApplication.ActiveDocument;

            ed.WriteMessage("\nMEGAPLAN Survey Tools Loaded.");
            ed.WriteMessage("\nAvailable Commands:");
            ed.WriteMessage("\nMEGA_SURFACE_ELEV - Get elevation from surface");
            ed.WriteMessage("\nMEGA_LIST_SURFACES - List all surfaces");
        }

    }
}


using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;

namespace MEGAPLAN.UI
{
    public class Commands
    {
        [CommandMethod("MEGA")]
        public void Start()
        {
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\nMEGAPLAN Enterprise Loaded.");
        }
    }
}

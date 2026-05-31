using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace MEGAPLAN.Configuration
{
    /// <summary>
    /// محلل إصدارات AutoCAD وتحديد المسارات المتوافقة
    /// </summary>
    public class AutoCADVersionResolver
    {
        // قائمة الإصدارات المدعومة بالترتيب (الأحدث أولاً)
        private static readonly List<AutoCADVersion> SupportedVersions = new List<AutoCADVersion>
        {
            new AutoCADVersion { Year = 2025, ProductName = "AutoCAD Civil 3D 2025", RelativePath = @"Autodesk\AutoCAD Civil 3D 2025" },
            new AutoCADVersion { Year = 2024, ProductName = "AutoCAD Civil 3D 2024", RelativePath = @"Autodesk\AutoCAD Civil 3D 2024" },
            new AutoCADVersion { Year = 2023, ProductName = "AutoCAD Civil 3D 2023", RelativePath = @"Autodesk\AutoCAD Civil 3D 2023" },
            new AutoCADVersion { Year = 2022, ProductName = "AutoCAD Civil 3D 2022", RelativePath = @"Autodesk\AutoCAD Civil 3D 2022" },
            new AutoCADVersion { Year = 2021, ProductName = "AutoCAD Civil 3D 2021", RelativePath = @"Autodesk\AutoCAD Civil 3D 2021" },
            new AutoCADVersion { Year = 2020, ProductName = "AutoCAD Civil 3D 2020", RelativePath = @"Autodesk\AutoCAD Civil 3D 2020" },
        };

        private static AutoCADVersion _detectedVersion;

        /// <summary>
        /// الحصول على الإصدار المكتشف من AutoCAD
        /// </summary>
        public static AutoCADVersion DetectedVersion
        {
            get
            {
                if (_detectedVersion == null)
                {
                    _detectedVersion = DetectAutoCADVersion();
                }
                return _detectedVersion;
            }
        }

        /// <summary>
        /// كشف إصدار AutoCAD المثبت
        /// </summary>
        public static AutoCADVersion DetectAutoCADVersion()
        {
            // محاولة القراءة من متغيرات البيئة أولاً
            string envPath = Environment.GetEnvironmentVariable("AUTOCAD_PATH");
            if (!string.IsNullOrEmpty(envPath) && Directory.Exists(envPath))
            {
                return ValidatePath(envPath);
            }

            // البحث في المسارات الافتراضية
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

            foreach (var version in SupportedVersions)
            {
                // محاولة في Program Files
                string path64 = Path.Combine(programFiles, version.RelativePath);
                if (Directory.Exists(path64))
                {
                    return ValidatePath(path64);
                }

                // محاولة في Program Files (x86)
                string path86 = Path.Combine(programFilesX86, version.RelativePath);
                if (Directory.Exists(path86))
                {
                    return ValidatePath(path86);
                }
            }

            throw new InvalidOperationException(
                "لم يتم العثور على أي إصدار من AutoCAD Civil 3D. تأكد من التثبيت أو اضبط متغير AUTOCAD_PATH"
            );
        }

        /// <summary>
        /// التحقق من صحة المسار والملفات المطلوبة
        /// </summary>
        private static AutoCADVersion ValidatePath(string installPath)
        {
            try
            {
                string[] requiredDlls = { "AcCoreMgd.dll", "AcDbMgd.dll", "AcMgd.dll", "AeccDbMgd.dll" };

                foreach (string dll in requiredDlls)
                {
                    if (!File.Exists(Path.Combine(installPath, dll)))
                    {
                        return null;
                    }
                }

                // محاولة استخراج رقم الإصدار من المجلد
                string folderName = new DirectoryInfo(installPath).Name;
                if (int.TryParse(folderName.Last().ToString(), out int year))
                {
                    return new AutoCADVersion
                    {
                        Year = 2000 + year,
                        ProductName = $"AutoCAD {2000 + year}",
                        InstallPath = installPath
                    };
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// الحصول على جميع الإصدارات المثبتة
        /// </summary>
        public static List<AutoCADVersion> GetInstalledVersions()
        {
            var installed = new List<AutoCADVersion>();
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

            foreach (var version in SupportedVersions)
            {
                var path64 = Path.Combine(programFiles, version.RelativePath);
                var path86 = Path.Combine(programFilesX86, version.RelativePath);

                if (Directory.Exists(path64))
                {
                    installed.Add(new AutoCADVersion
                    {
                        Year = version.Year,
                        ProductName = version.ProductName,
                        InstallPath = path64
                    });
                }
                else if (Directory.Exists(path86))
                {
                    installed.Add(new AutoCADVersion
                    {
                        Year = version.Year,
                        ProductName = version.ProductName,
                        InstallPath = path86
                    });
                }
            }

            return installed;
        }

        /// <summary>
        /// الحصول على المسار الكامل للمجلد الثنائيات
        /// </summary>
        public static string GetBinariesPath()
        {
            return DetectedVersion?.InstallPath ?? throw new InvalidOperationException("لم يتم كشف AutoCAD");
        }
    }

    /// <summary>
    /// معلومات إصدار AutoCAD
    /// </summary>
    public class AutoCADVersion
    {
        public int Year { get; set; }
        public string ProductName { get; set; }
        public string RelativePath { get; set; }
        public string InstallPath { get; set; }

        public override string ToString()
        {
            return $"{ProductName} ({Year})";
        }
    }
}

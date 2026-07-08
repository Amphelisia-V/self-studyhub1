using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using self_studyhub.Models;
using static self_studyhub.Pages.PDFPage;

namespace self_studyhub.Models
{
    public class RecentManager
    {
        public static ObservableCollection<Recentfile> RecentFiles
           = new ObservableCollection<Recentfile>();


        public static void AddRecent(string path)
        {
            RecentFiles.Insert(0, new Recentfile
            {
                FileName = System.IO.Path.GetFileName(path),
                FilePath = path
            });
        }
    }
}

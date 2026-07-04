using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace self_studyhub.Models
{
    public class DatabaseHelper
    {
        public static string ConnectionString =
            @"Data Source=DESKTOP-8BBV09R\MSSQLSERVER01;
            Initial Catalog=StudyControlDB;
            Integrated Security=True;
            Encrypt=True;
            TrustServerCertificate=True";
    }
}

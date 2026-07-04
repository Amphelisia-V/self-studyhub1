using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace self_studyhub.Models
{
    
        public class TaskItem
        {
            public int TaskId { get; set; }

            public int UserId { get; set; }

            public string TaskName { get; set; }

            public bool IsCompleted { get; set; }
        }
    
}

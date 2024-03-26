using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe
{
    public class ShiftModel
    {
        public int? Id { get; set; }
        public DateOnly? Date { get; set; }

        public TimeOnly? TimeBeginning { get; set; }
        public TimeOnly? TimeEnd { get; set; }
        public List<UserModel>? Workers { get; set; }

    }
}

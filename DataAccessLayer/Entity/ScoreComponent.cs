using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entity
{
    public class ScoreComponent
    {
        public int Id { get; set; }

        public int ScoreRecordId { get; set; }

        public string ComponentName { get; set; } = null!;

        public double ScoreValue { get; set; }
        public double Weight { get; set; }

        public ScoreRecord? ScoreRecord { get; set; }
    }
}

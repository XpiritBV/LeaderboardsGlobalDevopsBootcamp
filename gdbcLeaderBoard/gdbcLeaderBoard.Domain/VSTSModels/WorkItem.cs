using System;
using System.Collections.Generic;
using System.Text;

namespace gdbcLeaderBoard.Domain.VSTSModels
{
    public class WorkItem
    {
        public Fields fields { get; set; }

        public class Fields
        {
            public string SystemTeamProject { get; set; }
            public string SystemTags { get; set; }
            public string SystemState { get; set; }
        }
    }
}

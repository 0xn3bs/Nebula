using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core
{
    public interface ITrackable
    {
        bool IsDirty { get; }
        bool IsNew { get; set; }
        bool IsDeleted { get; set; }
        void BeginDirtyTracking();
        void AcceptChanges();
    }
}

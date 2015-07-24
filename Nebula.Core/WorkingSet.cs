using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core
{
    [DataContract]
    public class WorkingSet : IWorkingSet
    {
        [DataMember]
        private readonly List<ITrackable> _workingSet;

        public WorkingSet()
        {
            _workingSet = new List<ITrackable>();
        }

        public WorkingSet(List<ITrackable> workingSet)
        {
            _workingSet = workingSet;
        }

        public IEnumerator<ITrackable> GetEnumerator()
        {
            return _workingSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ITrackable item)
        {
            _workingSet.Add(item);
        }

        public void Clear()
        {
            _workingSet.Clear();
        }

        public bool Contains(ITrackable item)
        {
            return _workingSet.Contains(item);
        }

        public void CopyTo(ITrackable[] array, int arrayIndex)
        {
            _workingSet.CopyTo(array, arrayIndex);
        }

        public bool Remove(ITrackable item)
        {
            return _workingSet.Remove(item);
        }

        public int Count
        {
            get { return _workingSet.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}

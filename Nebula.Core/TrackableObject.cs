using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core
{
    public class TrackableObject : ITrackable
    {
        private byte[] _bytes;
        public bool IsDirtyTracking { get; set; }

        public object ObjectSnapshot
        {
            get
            {
                if (_bytes != null)
                {
                    return DeserializeFromBytes(_bytes);
                }
                return null;
            }
        }

        public void BeginDirtyTracking()
        {
            IsDirtyTracking = true;
            _bytes = null;
            _bytes = SerializeToBytes(this);
        }

        public void AcceptChanges()
        {
        }

        public bool IsDirty
        {
            get
            {
                if (IsDirtyTracking && _bytes != null)
                {
                    var oldBytes = _bytes;
                    _bytes = null;
                    var currentBytes = SerializeToBytes(this);
                    var result = !oldBytes.SequenceEqual(currentBytes);
                    _bytes = oldBytes;
                    return result;
                }
                return false;
            }
        }

        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }

        public static byte[] SerializeToBytes(object o)
        {
            using (var stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, o);
                var bytes = stream.ToArray();
                return bytes;
            }
        }

        public static object DeserializeFromBytes(byte[] bytes)
        {
            using (var stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Seek(0, SeekOrigin.Begin);
                IFormatter formatter = new BinaryFormatter();
                var o = formatter.Deserialize(stream);
                return o;
            }
        }
    }
}

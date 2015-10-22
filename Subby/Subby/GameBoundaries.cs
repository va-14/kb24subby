using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Subby
{
    [DataContract]
    public class GameBoundaries
    {

        private float _bottom;

        [DataMember]
        public float Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }

        private float _top;

        [DataMember]
        public float Top
        {
            get { return _top; }
            set { _top = value; }
        }

        private float _left;

        [DataMember]
        public float Left
        {
            get { return _left; }
            set { _left = value; }
        }

        private float _right;

        [DataMember]
        public float Right
        {
            get { return _right; }
            set { _right = value; }
        }
        
        
        
    }
}

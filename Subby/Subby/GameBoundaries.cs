using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subby
{
    public class GameBoundaries
    {
        private float _top;

        public float Top
        {
            get { return _top; }
            set { _top = value; }
        }

        private float _left;

        public float Left
        {
            get { return _left; }
            set { _left = value; }
        }

        private float _bottom;

        public float Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }

        private float _right;

        public float Right
        {
            get { return _right; }
            set { _right = value; }
        }
        
        
        
    }
}

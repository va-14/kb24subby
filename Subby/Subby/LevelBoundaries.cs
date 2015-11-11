using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace Subby
{
    [DataContract]
    public class LevelBoundaries
    {
        public void SetByGraphicDevice(GraphicsDevice graphicsDevice)
        {
            Top = graphicsDevice.Viewport.Height / 2.1f;
            Bottom = graphicsDevice.Viewport.Height - 80;
            Left = 20;
            Right = graphicsDevice.Viewport.Width / 2;
        }

        [DataMember]
        public float Bottom { get; set; }

        [DataMember]
        public float Left { get; set; }

        [DataMember]
        public float Right { get; set; }
        
        [DataMember]
        public float Top { get; set; }
        
    }
}

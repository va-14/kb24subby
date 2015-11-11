using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Subby
{
    [DataContract]
    class HighscoreList
    {
        [DataMember]
        int[] Highscores;

        public void CompareScoreWithHighscores(int score)
        {
            if (score > Highscores[Highscores.Length - 1])
            {
                
            }
        }
    }
}

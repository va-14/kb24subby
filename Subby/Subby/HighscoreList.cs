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
        public int[] Highscores;

        public void CompareScoreWithHighscores(int score)
        {
            if (score > Highscores[Highscores.Length - 1])
            {
                Highscores[Highscores.Length - 1] = score;
                Array.Sort<int>(Highscores);
                Array.Reverse(Highscores);
            }
        }
    }
}

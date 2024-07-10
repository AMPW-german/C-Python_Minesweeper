using System;
using System.Collections.Generic;

namespace minesweeper
{
    public class Test()
    {
        readonly Random rnd = new();

        public List<List<int>> Function()
        {
            List<int> boardsize = ConsoleApp1.Global_vars.boardsize;
            //int seed = 325345453;
            int seed = rnd.Next(Int32.MaxValue);
            SimplexNoise.Noise.Seed = seed;
            ConsoleApp1.Global_vars.jsontransfer.Add("seed", seed);

            float scale = 4f;

            List<List<int>> coordinatelist = new();
            List<int> subcoordinatelist = new();
            for (int f = 0; f < boardsize[1]; f++)
            {
                subcoordinatelist.Clear();
                for (int e = 0; e < boardsize[0]; e++)
                {
                    float pixel = SimplexNoise.Noise.CalcPixel2D(e, f, scale);
                    if (pixel >= 200)
                    {
                        subcoordinatelist.Add(1);
                    }
                    else
                    {
                        subcoordinatelist.Add(0);
                    }
                }
                coordinatelist.Add(new List<int>(subcoordinatelist));
            }
            return coordinatelist;
        }


    }
}
/* savespace

List<List<string>> myList = new List<List<string>>();
myList.Add(new List<string> { "a", "b" });

*/
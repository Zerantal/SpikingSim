using System.Collections.Generic;

namespace SpikingLibrary
{
    // ReSharper disable once UnusedMember.Global
    public class SensorArray
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly List<Synapse>[,] _outputAxons;  // array of axonal trees

        public SensorArray(int x, int y)
        {
            _outputAxons = new List<Synapse>[x,y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    _outputAxons[i, j] = new List<Synapse>();
        }
    }
}

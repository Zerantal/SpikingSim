using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpikingLibrary
{
    public class SensorArray
    {
        private int _x; // # horizontal sensors
        private int _y; // # vertical sensors
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Member")]
        private readonly List<Synapse>[,] _outputAxons;  // array of axonal trees

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "y"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "x")]
        public SensorArray(int x, int y)
        {
            _x = x;
            _y = y;
            _outputAxons = new List<Synapse>[_x,_y];
            for (int i = 0; i < _x; i++)
                for (int j = 0; j < _y; j++)
                    _outputAxons[i, j] = new List<Synapse>();
        }
    }
}

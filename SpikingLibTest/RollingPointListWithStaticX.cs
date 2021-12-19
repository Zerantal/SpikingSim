//============================================================================
//ZedGraph Class Library - A Flexible Line Graph/Bar Graph Library in C#
//Copyright © 2006  John Champion
//RollingPointPairList class Copyright © 2006 by Colin Green
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Diagnostics.Contracts;

using ZedGraph;

// based upon RollingPointPairList source
namespace SpikingLibTest
{
    /// <summary>
    /// A class that provides a rolling list of <see cref="PointPair" /> objects.
    /// This is essentially a 
    /// first-in-first-out (FIFO) queue with a fixed capacity which allows 'rolling' 
    /// (or oscilloscope like) graphs to be be animated without having the overhead of an
    /// ever-growing ArrayList.
    /// 
    /// The queue is constructed with a fixed capacity and new points can be enqueued. When the 
    /// capacity is reached the oldest (first in) PointPair is overwritten. However, when 
    /// accessing via <see cref="IPointList" />, the <see cref="PointPair" /> objects are
    /// seen in the order in which they were enqueued.
    ///
    /// RollingPointPairList supports data editing through the <see cref="IPointListEdit" />
    /// interface.
    /// 
    /// <author>Colin Green with mods by John Champion</author>
    /// <version> $Date: 2007-11-05 04:33:26 $ </version>
    /// </summary>
    [Serializable]
    [ContractVerification(false)]
    public class RollingPointListWithStaticX : ISerializable, IPointListEdit
    {

        #region Fields

        private double [] _xValues;
        private double [] _yValues;        

        /// <summary>
        /// The index of the previously enqueued item. -1 if buffer is empty.
        /// </summary>
        private int _headIdx;

        /// <summary>
        /// The index of the next item to be dequeued. -1 if buffer is empty.
        /// </summary>
        private int _tailIdx;

        private double _min;
        private double _max;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an empty buffer with the specified capacity.
        /// </summary>
        /// <param name="capacity">Number of elements in the rolling list.  This number
        /// cannot be changed once the RollingPointPairList is constructed.</param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public RollingPointListWithStaticX(int capacity, double min, double max)        
        {
            // Contract.Requires(capacity >= 0);
            // Contract.Requires(min <= max);
            
            _xValues = new double[capacity];
            _yValues = new double[capacity];
            
            _headIdx = _tailIdx = -1;
            Capacity = capacity;
            _min = min;
            _max = max;

            double xStep = (max - min) / capacity;
            double x = min;
            for (int xIdx = 0; xIdx < capacity; xIdx++, x+=xStep)
                _xValues[xIdx] = x;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the capacity of the rolling buffer.
        /// </summary>
        public int Capacity { get; }

        /// <summary>
        /// Gets the count of items within the rolling buffer. Note that this may be less than
        /// the capacity.
        /// </summary>
        public int Count
        {
            get
            {
                if (_headIdx == -1)
                    return 0;

                if (_headIdx > _tailIdx)
                    return _headIdx - _tailIdx + 1;

                if (_tailIdx > _headIdx)
                    return Capacity - _tailIdx + _headIdx + 1;

                return 1;
            }
        }

        /// <summary>
        /// Gets a boolean that indicates if the buffer is empty.
        /// Alternatively you can test Count==0.
        /// </summary>
        public bool IsEmpty => _headIdx == -1;

        /// <summary>
        /// Gets or sets the <see cref="PointPair" /> at the specified index in the buffer.
        /// </summary>
        /// <remarks>
        /// Index must be within the current size of the buffer, e.g., the set
        /// method will not expand the buffer even if <see cref="Capacity" /> is available
        /// </remarks>
        public PointPair this[int index]
        {
            get
            {
                var yIdx = index + _tailIdx;
                if (yIdx >= Capacity)
                    yIdx -= Capacity;

                return new PointPair(_xValues[index], _yValues[yIdx]);
            }
            set => throw new NotSupportedException();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Implement the <see cref="ICloneable" /> interface in a type safe manner by just
        /// calling the typed version of <see cref="Clone" />
        /// </summary>
        /// <returns>A deep copy of this object</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Type safe, deep-copy clone method.
        /// </summary>
        /// <returns>A new, independent copy of this class</returns>
        public RollingPointListWithStaticX Clone()
        {
            RollingPointListWithStaticX copy = new RollingPointListWithStaticX(Capacity, _min, _max)
            {
                _xValues = (double [])_xValues.Clone()
            };

            return copy;
        }

        /// <summary>
        /// Clear the buffer of all <see cref="PointPair"/> objects.
        /// Note that the <see cref="Capacity" /> remains unchanged.
        /// </summary>
        public void Clear()
        {
            _headIdx = _tailIdx = -1;
        }

        /// <summary>
        /// Calculate that the next index in the buffer that should receive a new data point.
        /// Note that this method actually advances the buffer, so a data point should be
        /// added at _mBuffer[_headIdx].
        /// </summary>
        /// <returns>The index position of the new head element</returns>
        private int GetNextIndex()
        {
            if (_headIdx == -1)
            {	// buffer is currently empty.
                _headIdx = _tailIdx = 0;
            }
            else
            {
                // Determine the index to write to.
                if (++_headIdx == Capacity)
                {	// Wrap around.
                    _headIdx = 0;
                }

                if (_headIdx != _tailIdx) return _headIdx;
                // Buffer overflow. Increment tailIdx.
                if (++_tailIdx == Capacity)
                {	// Wrap around.
                    _tailIdx = 0;
                }
            }

            return _headIdx;
        }

        /// <summary>
        /// Add a <see cref="PointPair"/> onto the head of the queue,
        /// overwriting old values if the buffer is full.
        /// </summary>
        /// <param name="item">The <see cref="PointPair" /> to be added.</param>
        public void Add(PointPair item)
        {
            throw new NotSupportedException();
            /*_mBuffer[GetNextIndex()] = item;*/
        }

        /// <summary>
        /// Add an <see cref="IPointList"/> object to the head of the queue.
        /// </summary>
        /// <param name="pointList">A reference to the <see cref="IPointList"/> object to
        /// be added</param>
        public void Add(IPointList pointList)
        {
            throw new NotSupportedException();
            // A slightly more efficient approach would be to determine where the new points should placed within
            // the buffer and to then copy them in directly - updating the head and tail indexes appropriately.
            /*
            for (int i = 0; i < pointList.Count; i++)
                Add(pointList[i]);*/
        }

        /// <summary>
        /// Remove an old item from the tail of the queue.
        /// </summary>
        /// <returns>The removed item. Throws an <see cref="InvalidOperationException" />
        /// if the buffer was empty. 
        /// Check the buffer's length (<see cref="Count" />) or the <see cref="IsEmpty" />
        /// property to avoid exceptions.</returns>
        public PointPair Remove()
        {
            if (_tailIdx == -1)
            {	// buffer is currently empty.
                throw new InvalidOperationException("buffer is empty.");
            }

            PointPair o = new PointPair(_xValues[0], _yValues[_tailIdx]);

            if (_tailIdx == _headIdx)
            {	// The buffer is now empty.
                _headIdx = _tailIdx = -1;
                return o;
            }

            if (++_tailIdx == Capacity)
            {	// Wrap around.
                _tailIdx = 0;
            }

            return o;
        }

        /// <summary>
        /// Remove the <see cref="PointPair" /> at the specified index
        /// </summary>
        /// <remarks>
        /// All items in the queue that lie after <paramref name="index"/> will
        /// be shifted back by one, and the queue will be one item shorter.
        /// </remarks>
        /// <param name="index">The ordinal position of the item to be removed.
        /// Throws an <see cref="ArgumentOutOfRangeException" /> if index is less than
        /// zero or greater than or equal to <see cref="Count" />
        /// </param>
        public void RemoveAt(int index)
        {
            int count = Count;

            if (index >= count || index < 0)
                throw new ArgumentOutOfRangeException();

            // shift all the items that lie after index back by 1
            for (int i = index + _tailIdx; i < _tailIdx + count - 1; i++)
            {
                i = i >= Capacity ? 0 : i;
                int j = i + 1;
                j = j >= Capacity ? 0 : j;
                _yValues[i] = _yValues[j];
            }

            // Remove the item from the head (it's been duplicated already)
            Pop();
        }

        /// <summary>
        /// Remove a range of <see cref="PointPair" /> objects starting at the specified index
        /// </summary>
        /// <remarks>
        /// All items in the queue that lie after <paramref name="index"/> will
        /// be shifted back, and the queue will be <paramref name="count" /> items shorter.
        /// </remarks>
        /// <param name="index">The ordinal position of the item to be removed.
        /// Throws an <see cref="ArgumentOutOfRangeException" /> if index is less than
        /// zero or greater than or equal to <see cref="Count" />
        /// </param>
        /// <param name="count">The number of items to be removed.  Throws an
        /// <see cref="ArgumentOutOfRangeException" /> if <paramref name="count" /> is less than zero
        /// or greater than the total available items in the queue</param>
        public void RemoveRange(int index, int count)
        {
            int totalCount = Count;

            if (index >= totalCount || index < 0 || count < 0 || count > totalCount)
                throw new ArgumentOutOfRangeException();

            for (int i = 0; i < count; i++)
                RemoveAt(index);
        }

        /// <summary>
        /// Pop an item off the head of the queue.
        /// </summary>
        /// <returns>The popped item. Throws an exception if the buffer was empty.</returns>
        public PointPair Pop()
        {
            if (_tailIdx == -1)
            {	// buffer is currently empty.
                throw new InvalidOperationException("buffer is empty.");
            }

            PointPair o = new PointPair(_xValues[Capacity-1], _yValues[_headIdx]);

            if (_tailIdx == _headIdx)
            {	// The buffer is now empty.
                _headIdx = _tailIdx = -1;
                return o;
            }

            if (--_headIdx == -1)
            {	// Wrap around.
                _headIdx = Capacity - 1;
            }

            return o;
        }

        /// <summary>
        /// Peek at the <see cref="PointPair" /> item at the head of the queue.
        /// </summary>
        /// <returns>The <see cref="PointPair" /> item at the head of the queue.
        /// Throws an <see cref="InvalidOperationException" /> if the buffer was empty.
        /// </returns>
        public PointPair Peek()
        {
            if (_headIdx == -1)
            {	// buffer is currently empty.
                throw new InvalidOperationException("buffer is empty.");
            }

            return new PointPair(_xValues[Capacity - 1], _yValues[_headIdx]);
        }

        #endregion

        #region Auxilliary Methods

        public void Add(double y)
        {
            // advance the rolling list
            GetNextIndex();

            _yValues[_headIdx] = y;            
        }

        /// <summary>
        /// Add a set of values onto the head of the queue,
        /// overwriting old values if the buffer is full.
        /// </summary>
        /// <remarks>
        /// This method is much more efficient that the <see cref="Add(PointPair)">Add(PointPair)</see>
        /// method, since it does not require that a new PointPair instance be provided.
        /// If the buffer already contains a <see cref="PointPair"/> at the head position,
        /// then the x, y, z, and tag values will be copied into the existing PointPair.
        /// Otherwise, a new PointPair instance must be created.
        /// In this way, each PointPair position in the rolling list will only be allocated one time.
        /// To truly be memory efficient, the <see cref="Remove" />, <see cref="RemoveAt" />,
        /// and <see cref="Pop" /> methods should be avoided.
        /// </remarks>
        /// <param name="x">The X value</param>
        /// <param name="y">The Y value</param>
        public void Add(double x, double y)
        {
            throw new NotSupportedException();
            //Add(x, y, PointPair.Missing, null);
        }


        #endregion

        #region Serialization

        /// <summary>
        /// Current schema value that defines the version of the serialized file
        /// </summary>
        public const int Schema = 10;

        /// <summary>
        /// Constructor for deserializing objects
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
        /// </param>
        /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
        /// </param>
        protected RollingPointListWithStaticX(SerializationInfo info, StreamingContext context)
        {
            // The schema value is just a file version parameter.  You can use it to make future versions
            // backwards compatible as new member variables are added to classes
            // ReSharper disable once SuggestVarOrType_BuiltInTypes
            // ReSharper disable once UnusedVariable
            int sch = info.GetInt32("schema");

            _headIdx = info.GetInt32("headIdx");
            _tailIdx = info.GetInt32("tailIdx");
            
            Capacity = info.GetInt32("capacity");
            _min = info.GetDouble("minRange");
            _max = info.GetDouble("maxRange");
            _xValues = (double[])info.GetValue("xValues", typeof(double[]));
            _yValues = (double[])info.GetValue("yValues", typeof(double[]));            
        }
        /// <summary>
        /// Populates a <see cref="SerializationInfo"/> instance with the data needed to serialize the target object
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data</param>
        /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data</param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("schema", Schema);
            info.AddValue("headIdx", _headIdx);
            info.AddValue("tailIdx", _tailIdx);
            info.AddValue("capacity", Capacity);
            info.AddValue("minRange", _min);
            info.AddValue("maxRange", _max);
            info.AddValue("xValues", _xValues);
            info.AddValue("yValues", _yValues);            
        }

        #endregion

    }
}

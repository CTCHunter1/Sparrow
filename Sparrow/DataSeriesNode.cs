using System;
using System.Collections.Generic;
using System.Text;

namespace Sparrow
{
    public class DataSeriesNode : DataSeries
    {
        private DataSeriesNode mNextNode;
        private int mDownsamplingFactor = 0;

        public DataSeriesNode(int numPts, double sampleRate, AmpUnits fourierAmpUnits, double resistance, bool updateFFT,
            int downsamplingFactor, DataSeriesNode nextNode)
            : base(numPts, sampleRate, fourierAmpUnits, resistance)
        {
            mNextNode = nextNode;
            mDownsamplingFactor = downsamplingFactor;
        }

        public DataSeriesNode NextNode
        {
            get
            {
                return (mNextNode);
            }
            set
            {
                mNextNode = NextNode;
            }
        }

        public override void AddPoint(double pt)
        {
            // add the point to the array
            base.y_t[ptIndex] = pt;

            if ((base.ptIndex % mDownsamplingFactor) == (mDownsamplingFactor - 1))
            {
                if (mNextNode != null)
                {
                    // average the last mDowsamplingFactor points and add them to the next node
                    mNextNode.AddPoint(GetAverageFromDoubleArray(base.y_t,
                        ptIndex - mDownsamplingFactor + 1, mDownsamplingFactor));
                }
            }

            base.ptIndex++;

            // reset the point index if it exceeds the length of the array
            if (base.ptIndex >= base.NumPoints)
                ptIndex = 0;
        }

        /// <summary>
        /// Calculate the average of all elements in a double array.
        /// </summary>
        /// <param name="dblArray">The double array to get the 
        /// average from.</param>
        /// <returns>The average of the double array</returns>
        public double GetAverageFromDoubleArray(double[] dblArray, int startIndex, int len)
        {
            double dblResult = 0;
            int endIndex = startIndex + len;

            for (int i = startIndex; i < endIndex; i++)
            {
                dblResult += dblArray[i];
            }

            return dblResult / (len);
        }
    }
}

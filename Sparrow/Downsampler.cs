using System;
using System.Collections.Generic;
using System.Text;

namespace Sparrow
{
    public class DownSampler
    {
        private int mNumDecades;
        /// <summary>
        /// Must be power of 2
        /// </summary>
        private int mDownsamplingFactor;
        int mPointsPerDecade;
        private DataSeries origionalDataSeries;
        private DataSeriesNode[] downsampledNodes;
        AmpUnits ampUnits;
        double mResistance;

        double[] mFLogArr;      // the frequency scale for the downsampled data
        double[] y_logf = null;            // the data points on a log(f) scale

        // points per update
        private int pointsPerUpdate = 0;

        public DownSampler(DataSeries origioalDataSeriesObj, int downsamplingFactor, int pointsPerDecade, int numDecadesToDownSample, AmpUnits fourierAmpUnits, double resistance)
        {
            origionalDataSeries = origioalDataSeriesObj;

            mNumDecades = numDecadesToDownSample;
            mDownsamplingFactor = downsamplingFactor;
            mPointsPerDecade = pointsPerDecade;
            ampUnits = fourierAmpUnits;
            mResistance = resistance;
            InitDownsampledDataSeries();
        }

        public DataSeries OrigionalDataSeries
        {
            get
            {
                return (origionalDataSeries);
            }
            set
            {
                origionalDataSeries = value;
            }
        }

        public int NumDecades
        {
            get
            {
                return (mNumDecades);
            }
            set
            {
                InitDownsampledDataSeries();
                mNumDecades = value;
            }
        }

        public int DownsamplingFactor
        {
            get
            {
                return (mDownsamplingFactor);
            }
            set
            {
                InitDownsampledDataSeries();
                mDownsamplingFactor = value;
            }
        }

        public double[] DownsampledLogFrequency
        {
            get
            {
                return (mFLogArr);
            }
        }

        public double[] Y_fLog
        {
            get
            {
                y_logf = GetDownsampledY_fLog();
                return (y_logf);
            }
        }

        public void UpdateDownsampledData()
        {
            // because the data is coming in in 2^n multiple powers
            // and the downsampling factor must be a power of 2
            // the downsampled data sets can only add 1, 2, 4,.. 2^n points per  update
            // update the lowest downsampled data series
            for (int i = 0; i < pointsPerUpdate; i++)
            {
                downsampledNodes[0].AddPoint(
                    downsampledNodes[0].GetAverageFromDoubleArray(
                    origionalDataSeries.Y_t, i * mDownsamplingFactor, mDownsamplingFactor));
            }

        }

        public DataSeries GetDownsampledDataSeries(int decade)
        {
            return ((DataSeries)downsampledNodes[decade - 1]);
        }

        private void InitDownsampledDataSeries()
        {
            downsampledNodes = new DataSeriesNode[mNumDecades];

            // these data series nodes need to be created in reverse order so 
            // that the previous one can know about the next one
            for (int i = mNumDecades - 1; i >= 0; i--)
            {
                // the last decade has no next node pointer
                if (i == (mNumDecades - 1))
                    downsampledNodes[i] = new DataSeriesNode(mPointsPerDecade, origionalDataSeries.SampleRate / (Math.Pow((double)mDownsamplingFactor, (double)(i + 1))),
                        ampUnits, mResistance, false, mDownsamplingFactor, null);

                else
                    downsampledNodes[i] = new DataSeriesNode(mPointsPerDecade, origionalDataSeries.SampleRate / (Math.Pow((double)mDownsamplingFactor, (double)(i + 1))),
                        ampUnits, mResistance, false, mDownsamplingFactor, downsampledNodes[i + 1]);

            }

            // calculate the number of points updated per data set
            pointsPerUpdate = OrigionalDataSeries.Y_t.Length / mDownsamplingFactor;

            mFLogArr = CreateDownsampledFrequency();
        }

        private double[] CreateDownsampledFrequency()
        {
            int iNumLogfPts = (origionalDataSeries.NumPoints / 2) * (mDownsamplingFactor - 1) / mDownsamplingFactor +
                (mNumDecades - 1) * (mPointsPerDecade / 2 * (mDownsamplingFactor - 1)) / mDownsamplingFactor +
                mPointsPerDecade / 2;

            double[] fLogArr = new double[iNumLogfPts];

            //copy backwards
            int fIndex = iNumLogfPts - 1;      // index to use for the fLogArr

            // copy the origial data frequencies (do not copy this index)
            int stopIndex = (origionalDataSeries.NumPoints / 2) / mDownsamplingFactor;

            for (int i = origionalDataSeries.NumPoints / 2 - 1; i >= stopIndex; i--)
            {
                fLogArr[fIndex] = origionalDataSeries.FrequencyHalfArr[i];
                fIndex--;
            }

            // for each decade copy out the frequencies
            stopIndex = (mPointsPerDecade / 2) / mDownsamplingFactor;

            for (int i = 0; i < (mNumDecades - 1); i++)
            {
                for (int j = mPointsPerDecade / 2 - 1; j >= stopIndex; j--)
                {
                    fLogArr[fIndex] = downsampledNodes[i].FrequencyHalfArr[j];
                    fIndex--;
                }
            }

            for (int j = mPointsPerDecade / 2 - 1; j >= 0; j--)
            {
                fLogArr[fIndex] = downsampledNodes[mNumDecades - 1].FrequencyHalfArr[j];
                fIndex--;
            }

            return (fLogArr);
        }

        private double[] GetDownsampledY_fLog()
        {
            int iNumLogfPts = mFLogArr.Length;

            double[] y_fLog = new double[iNumLogfPts];

            //copy backwards
            int yIndex = iNumLogfPts - 1;      // index to use for the fLogArr

            // update the FFT first
            origionalDataSeries.UpdateFFT();
            // copy the origial data frequencies (do not copy this index)
            int stopIndex = (origionalDataSeries.NumPoints / 2) / mDownsamplingFactor;
            for (int i = origionalDataSeries.NumPoints / 2 - 1; i >= stopIndex; i--)
            {
                double[] y_temp = origionalDataSeries.YAbs_fHalf;
                y_fLog[yIndex] = y_temp[i];
                yIndex--;
            }

            // for each decade copy out the frequencies
            stopIndex = (mPointsPerDecade / 2) / mDownsamplingFactor;

            for (int i = 0; i < (mNumDecades - 1); i++)
            {
                downsampledNodes[i].UpdateFFT();        // update the fft for this node
                for (int j = mPointsPerDecade / 2 - 1; j >= stopIndex; j--)
                {
                    double[] y_temp = downsampledNodes[i].YAbs_fHalf;
                    y_fLog[yIndex] = y_temp[j];
                    yIndex--;
                }
            }

            downsampledNodes[mNumDecades - 1].UpdateFFT();
            for (int j = mPointsPerDecade / 2 - 1; j >= 0; j--)
            {
                double[] y_temp = downsampledNodes[mNumDecades - 1].YAbs_fHalf;
                y_fLog[yIndex] = y_temp[j];
                yIndex--;
            }

            return (y_fLog);
        }
    }
}

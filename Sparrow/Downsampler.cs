using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
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
        SOSFilter sosFilterObj;

        double[] mFLogArr;      // the frequency scale for the downsampled data
        double[] y_logf = null;            // the data points on a log(f) scale

        // points per update
        private int pointsPerUpdate = 0;

        private bool bFFTAveraging = false;

        private int pointsAdded = 0;

        // log variables
        int iNumLogfPts = 0;    // the number of logf points            
        int origionalStartIndex = 0;
        int origionalStopIndex = 0;
        int firstDecStartIndex = 0;
        int firstDecStopIndex = 0;
        int secondDecStartIndex = 0;
        int secondDecStopIndex = 0;
        int lastDecStartIndex = 0;
        int lastDecStopIndex = 0;

        public DownSampler(DataSeries origioalDataSeriesObj, int downsamplingFactor, int pointsPerDecade, int numDecadesToDownSample, AmpUnits fourierAmpUnits, double resistance)
        {
            origionalDataSeries = origioalDataSeriesObj;

            mNumDecades = numDecadesToDownSample;
            mDownsamplingFactor = downsamplingFactor;
            mPointsPerDecade = pointsPerDecade;
            ampUnits = fourierAmpUnits;
            mResistance = resistance;

            sosFilterObj = new SOSFilter(new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Filter.csv"));
            InitDownsampledDataSeriesNodes();
        }

        private void InitDownsampledDataSeriesNodes()
        {
            downsampledNodes = new DataSeriesNode[mNumDecades];

            // these data series nodes need to be created in reverse order so 
            // that the previous one can know about the next one
            for (int i = mNumDecades - 1; i >= 0; i--)
            {
                // the last decade has no next node pointer
                if (i == (mNumDecades - 1))
                    downsampledNodes[i] = new DataSeriesNode(mPointsPerDecade, origionalDataSeries.SampleRate / (Math.Pow((double)mDownsamplingFactor, (double)(i + 1))),
                        ampUnits, mResistance, false, mDownsamplingFactor, new SOSFilter(new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Filter.csv")), null);

                else
                    downsampledNodes[i] = new DataSeriesNode(mPointsPerDecade, origionalDataSeries.SampleRate / (Math.Pow((double)mDownsamplingFactor, (double)(i + 1))),
                        ampUnits, mResistance, false, mDownsamplingFactor, new SOSFilter(new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Filter.csv")), downsampledNodes[i + 1]);

            }

            // calculate the number of points updated per data set
            pointsPerUpdate = OrigionalDataSeries.Y_t.Length / mDownsamplingFactor;

            mFLogArr = CreateDownsampledFrequency();
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
                InitDownsampledDataSeriesNodes();
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
                InitDownsampledDataSeriesNodes();
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

        public int PointsPerDecade
        {
            get
            {
                return (mPointsPerDecade);
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

        public double[] YAvg_fLog
        {
            get
            {
                y_logf = GetDownsampledYAvg_fLog();
                return (y_logf);
            }
        }

        public AmpUnits FrequencyUnits
        {
            get             
            {
                return (ampUnits);
            }
        }

        public bool FFTAveraging
        {
            get
            {
                return (bFFTAveraging);
            }
            set
            {
                for (int i = 0; i < downsampledNodes.Length; i++)
                {
                    downsampledNodes[i].FFTAveraging = value;
                }

                bFFTAveraging = value;
            }
        }

        public void UpdateDownsampledData(ToolStripProgressBar pBar)
        {
            if (bFFTAveraging)
            {
                origionalDataSeries.UpdateFFT();
                origionalDataSeries.UpdateFFTAverage();
            }

            for (int i = 0; i < origionalDataSeries.NumPoints; i++)
            {
                double pt = sosFilterObj.AddPoint(origionalDataSeries.Y_t[i]);

                if ((i % mDownsamplingFactor) == (mDownsamplingFactor - 1))
                {
                    if (pointsAdded > sosFilterObj.Order)
                    {
                        downsampledNodes[0].AddPoint(pt, pBar);
                    }
                    else
                    {
                        pointsAdded++;
                    }

                }
            }

        }

        public DataSeries GetDownsampledDataSeries(int decade)
        {
            return ((DataSeries)downsampledNodes[decade - 1]);
        }



        private double[] CreateDownsampledFrequency()
        {
            int numOrigionalDataPoints = 0;
            int numFirstDecPoints = 0;
            int numSecondDecPonits = 0;
            int numLastDecPoints = 0;

            // decade 1 points per origioal data set ponits
            int dec1Ratio = (int)(origionalDataSeries.DeltaF/downsampledNodes[0].DeltaF); // radio of frequencies in origional series
                                                                                        // and downsampled series
            double downsampleFract = (double) (mDownsamplingFactor - 1) / mDownsamplingFactor;
            int rollOver = 20; // number of points to continue into the next decade

            if(mNumDecades == 0)
            {
                iNumLogfPts = (int) (origionalDataSeries.NumPoints / 2.0);
            } 
            else
            {
                // number of points to use from the origional data set
                numOrigionalDataPoints = (int) ((origionalDataSeries.NumPoints / 2.0) * downsampleFract); // number of points up to nyquist of next frequency
                numOrigionalDataPoints += rollOver; // number of addtional points in first series
            
                if(mNumDecades == 1)
                {
                    // there is only one decade
                    numFirstDecPoints = (int) ((mPointsPerDecade / 2.0)); // number of points up to nyquist of dec 2            
                    iNumLogfPts = numOrigionalDataPoints + numFirstDecPoints;
                }
                else 
                {
                    numFirstDecPoints = (int) ((mPointsPerDecade / 2.0) * downsampleFract); // number of points up to nyquist of dec 2
                        numFirstDecPoints -= rollOver*dec1Ratio; // number of points removed by rolling over from origional series
                        numFirstDecPoints += rollOver;           // number of points added past nyquist in next decade             

                    numSecondDecPonits = (int) ((mPointsPerDecade / 2.0) * downsampleFract); // number of points up to nyquist of dec 3
                        numSecondDecPonits -=  rollOver*mDownsamplingFactor;    // number of points removed by rolling over decade 1
                        numSecondDecPonits += rollOver;         // number of points added into nyquist in the next decade

                    numLastDecPoints = (mPointsPerDecade / 2);
                        numLastDecPoints -= rollOver*mDownsamplingFactor;   // number of points removed by rolling over decade 2                

                    iNumLogfPts = numOrigionalDataPoints + numFirstDecPoints + numSecondDecPonits * (mNumDecades - 2) + numLastDecPoints;

                }
            }

            /*
            int iNumLogfPts = (origionalDataSeries.NumPoints / 2) * (mDownsamplingFactor - 1) / mDownsamplingFactor +
                (mNumDecades - 1) * (mPointsPerDecade / 2 * (mDownsamplingFactor - 1)) / mDownsamplingFactor +
                mPointsPerDecade / 2; */

            double[] fLogArr = new double[iNumLogfPts];

            //copy backwards
            int fIndex = iNumLogfPts - 1;      // indexer for the fLogArr

            // copy the origial data frequencies (do not copy this index)
            origionalStartIndex = (origionalDataSeries.NumPoints / 2) - 1;
            origionalStopIndex = origionalStartIndex - numOrigionalDataPoints+1;

            for (int i = origionalStartIndex; i >= origionalStopIndex; i--)
            {
                fLogArr[fIndex] = origionalDataSeries.FrequencyHalfArr[i];
                fIndex--;
            }

            if(mNumDecades >= 1)
            {
                // copy out the first decade
                firstDecStartIndex = mPointsPerDecade / 2 - 1 - rollOver * dec1Ratio;
                firstDecStopIndex = firstDecStartIndex - numFirstDecPoints+1;

                for (int j = firstDecStartIndex; j >= firstDecStopIndex; j--)
                {
                    fLogArr[fIndex] = downsampledNodes[0].FrequencyHalfArr[j];
                    fIndex--;
                }
            }

            if(mNumDecades >= 2)
            {
                secondDecStartIndex = mPointsPerDecade / 2 - 1 - rollOver * mDownsamplingFactor;
                secondDecStopIndex = secondDecStartIndex - numSecondDecPonits+1;

                // copy the decades between the first and the last
                for (int i = 1; i <= (mNumDecades - 2); i++)
                {
                    for (int j = secondDecStartIndex; j >= secondDecStopIndex; j--)
                    {
                        fLogArr[fIndex] = downsampledNodes[i].FrequencyHalfArr[j];
                        fIndex--;
                    }
                }

                // copy the last decade
                lastDecStartIndex = mPointsPerDecade / 2 - 1 - rollOver * mDownsamplingFactor;
                lastDecStopIndex = lastDecStartIndex - numLastDecPoints+1;

                for (int j = lastDecStartIndex; j >= lastDecStopIndex; j--)
                {
                    fLogArr[fIndex] = downsampledNodes[mNumDecades - 1].FrequencyHalfArr[j];
                    fIndex--;
                }                
            }                                          

            return (fLogArr);
        }

        private double[] GetDownsampledY_fLog()
        {
            double[] y_fLog = new double[iNumLogfPts];

            // update the FFT first
            origionalDataSeries.UpdateFFT();
            

            //copy backwards
            int yIndex = iNumLogfPts - 1;      // indexer for the fLogArr
            
            for (int i = origionalStartIndex; i >= origionalStopIndex; i--)
            {
                y_fLog[yIndex] = origionalDataSeries.YAbs_fHalf[i];
                yIndex--;
            }

            if (mNumDecades >= 1)
            {
                // copy out the first decade                
                for (int j = firstDecStartIndex; j >= firstDecStopIndex; j--)
                {
                    y_fLog[yIndex] = downsampledNodes[0].YAbs_fHalf[j];
                    yIndex--;
                }
            }

            if (mNumDecades >= 2)
            {                
                // copy the decades between the first and the last
                for (int i = 1; i <= (mNumDecades - 2); i++)
                {
                    for (int j = secondDecStartIndex; j >= secondDecStopIndex; j--)
                    {
                        y_fLog[yIndex] = downsampledNodes[i].YAbs_fHalf[j];
                        yIndex--;
                    }
                }

                // copy the last decade                
                for (int j = lastDecStartIndex; j >= lastDecStopIndex; j--)
                {
                    y_fLog[yIndex] = downsampledNodes[mNumDecades - 1].YAbs_fHalf[j];
                    yIndex--;
                }
            }

            return (y_fLog);
        }

        private double[] GetDownsampledYAvg_fLog()
        {
            double[] y_fLog = new double[iNumLogfPts];

            // update the FFT first
            origionalDataSeries.UpdateFFT();
            

            //copy backwards
            int yIndex = iNumLogfPts - 1;      // indexer for the fLogArr
            
            for (int i = origionalStartIndex; i >= origionalStopIndex; i--)
            {
                y_fLog[yIndex] = origionalDataSeries.YAbsAvg_fHalf[i];
                yIndex--;
            }

            if (mNumDecades >= 1)
            {
                // copy out the first decade                
                for (int j = firstDecStartIndex; j >= firstDecStopIndex; j--)
                {
                    y_fLog[yIndex] = downsampledNodes[0].YAbsAvg_fHalf[j];
                    yIndex--;
                }
            }

            if (mNumDecades >= 2)
            {                
                // copy the decades between the first and the last
                for (int i = 1; i <= (mNumDecades - 2); i++)
                {
                    for (int j = secondDecStartIndex; j >= secondDecStopIndex; j--)
                    {
                        y_fLog[yIndex] = downsampledNodes[i].YAbsAvg_fHalf[j];
                        yIndex--;
                    }
                }

                // copy the last decade                
                for (int j = lastDecStartIndex; j >= lastDecStopIndex; j--)
                {
                    y_fLog[yIndex] = downsampledNodes[mNumDecades - 1].YAbsAvg_fHalf[j];
                    yIndex--;
                }
            }

            return (y_fLog);
        }
    }
}

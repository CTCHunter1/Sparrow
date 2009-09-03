using System;
using System.Collections.Generic;
using System.Text;

using MathNet.Numerics;
using MathNet.Numerics.Transformations;

namespace Sparrow
{
    public class DataSeries
    {
        // related to the data
        private double[] tArr;
        private double[] fArr;
        private double[] fHalfArr;
        protected double[] y_t;
        private bool bUpdateFFT = false;
        private double[] yAbs_f;
        private double[] yReal_f;
        private double[] yImag_f;
        protected int ptIndex = 0;
        private int mNumPts;
        private double fSample;
        private double mResistance;

        // the transformer
        private RealFourierTransformation rftObj = new RealFourierTransformation(TransformationConvention.Matlab);

        AmpUnits mAmpUnits;

        public DataSeries(int numPts, double sampleRate, AmpUnits fourierAmpUnits, double resistance)
        {
            mNumPts = numPts;
            fSample = sampleRate;

            // create the data arrays.
            CreateTimeArr();
            fArr = rftObj.GenerateFrequencyScale(fSample, numPts);
            fHalfArr = GetFreqHalfArr(fArr);

            y_t = new double[numPts];
            mAmpUnits = fourierAmpUnits;
            mResistance = resistance;
            UpdateFFT();
        }

        public DataSeries(int numPts, double sampleRate, AmpUnits fourierAmpUnits, double resistance, bool updateFFT)
        {
            mNumPts = numPts;
            fSample = sampleRate;

            // create the data arrays.
            CreateTimeArr();
            fArr = rftObj.GenerateFrequencyScale(fSample, numPts);
            fHalfArr = GetFreqHalfArr(fArr);

            y_t = new double[numPts];
            mAmpUnits = fourierAmpUnits;
            mResistance = resistance;
            UpdateFFT();

            bUpdateFFT = updateFFT;
        }

        public double[] Y_t
        {
            get
            {
                return (y_t);
            }
            set
            {
                y_t = value;

                if (bUpdateFFT)
                    UpdateFFT();
            }
        }

        public double[] TimeArr
        {
            get
            {
                return (tArr);
            }
        }

        public double[] FrequencyArr
        {
            get
            {
                return (fArr);
            }
        }

        public double[] FrequencyHalfArr
        {
            get
            {
                return (fHalfArr);
            }
        }

        public int NumPoints
        {
            get
            {
                return (mNumPts);
            }
            set
            {
                mNumPts = value;
                // create the data arrays.
                CreateTimeArr();
                fArr = rftObj.GenerateFrequencyScale(fSample, mNumPts);
                fHalfArr = GetFreqHalfArr(fArr);
                // update FFT
                UpdateFFT();
            }
        }

        public double SampleRate
        {
            get
            {
                return (fSample);
            }
            set
            {
                fSample = value;
                // Update the time and frequency arrays
                // create the data arrays.
                CreateTimeArr();
                fArr = rftObj.GenerateFrequencyScale(fSample, mNumPts);
                fHalfArr = GetFreqHalfArr(fArr);

            }
        }

        public double[] YAbs_f
        {
            get
            {
                return (yAbs_f);
            }
        }

        public double[] YAbs_fHalf
        {
            get
            {
                double[] halfArr = new double[yAbs_f.Length / 2];
                for (int i = 0; i < (yAbs_f.Length / 2); i++)
                {
                    halfArr[i] = yAbs_f[i + 1];
                }

                return (halfArr);
            }
        }

        public void UpdateFFT()
        {
            rftObj.TransformForward(Y_t, out yReal_f, out yImag_f);
            // any math that needs to be done should just be done in this function
            yAbs_f = GetModulous(yReal_f, yImag_f);
            // TODO: Implement Phase retrevial
        }

        public virtual void AddPoint(double pt)
        {
            y_t[ptIndex] = pt;

            ptIndex++;
            // check that we haven't reached the end of the array
            if (ptIndex >= mNumPts)
                ptIndex = 0;
        }

        public int PtIndex
        {
            get
            {
                return (ptIndex);
            }

            set
            {
                ptIndex = value;
            }
        }

        private void CreateTimeArr()
        {
            tArr = new double[mNumPts];

            tArr[0] = 0;
            double tSample = 1 / fSample;     // find the time between samples

            for (int i = 1; i < mNumPts; i++)
            {
                tArr[i] = tArr[i - 1] + tSample;
            }
        }

        private double[] GetModulous(double[] realArr, double[] imagArr)
        {
            double scale_factor = .5 / (Convert.ToDouble(realArr.Length)); // this scale factor asummes the sampled window is repeated for a full second

            // the scale factor is need because of implicit windowing of the transform                    
            double[] modArr = new double[realArr.Length];

            /*
            maxBroadArr[0] = -1E8;
            maxBroadArr[1] = -1E8;
            maxBroadArr[2] = -1E8;

            */
            switch (mAmpUnits)
            {
                case AmpUnits.dBmV:
                    // overwrite the scale factor with the addition for dBmV
                    scale_factor = 20 * Math.Log10(scale_factor) + 60;

                    for (int i = 0; i < realArr.Length; i++)
                    {
                        modArr[i] = 10 * Math.Log10((realArr[i] * realArr[i] + imagArr[i] * imagArr[i])) + scale_factor;
                        // Check that the abs was not zero
                        if (modArr[i] < -120)
                            modArr[i] = -120;

                        /* Max Capture Code
                        if (graphNum == 1)
                        {
                            if (modArr[i] > maxBroadArr[0])
                                maxBroadArr[0] = modArr[i];
                            else if (modArr[i] > maxBroadArr[1])
                                maxBroadArr[1] = modArr[i];
                            else if (modArr[i] > maxBroadArr[2])
                                maxBroadArr[2] = modArr[i];
                        }
                        */
                    }
                    break;

                case AmpUnits.V:
                    for (int i = 0; i < realArr.Length; i++)
                    {
                        modArr[i] = scale_factor * Math.Sqrt((realArr[i] * realArr[i] + imagArr[i] * imagArr[i]));
                        /* Max Capture Code
                        if (graphNum == 1)
                        {
                            if (modArr[i] > maxBroadArr[0])
                                maxBroadArr[0] = modArr[i];
                            else if (modArr[i] > maxBroadArr[1])
                                maxBroadArr[1] = modArr[i];
                            else if (modArr[i] > maxBroadArr[2])
                                maxBroadArr[2] = modArr[i];
                        }*/
                    }
                    break;

                case AmpUnits.dBm:
                    // overwrite scale factor with the addtion factor for dBm
                    scale_factor = 20 * Math.Log10(scale_factor) + -10 * Math.Log10(mResistance) + 30;

                    for (int i = 0; i < realArr.Length; i++)
                    {
                        modArr[i] = 10 * Math.Log10(realArr[i] * realArr[i] + imagArr[i] * imagArr[i]) + scale_factor;
                        // Check that the abs was not zero
                        if (modArr[i] < -120)
                            modArr[i] = -120;

                        /* Max Caputure Code
                        if (graphNum == 1)
                        {
                            if (modArr[i] > maxBroadArr[0])
                                maxBroadArr[0] = modArr[i];
                            else if (modArr[i] > maxBroadArr[1])
                                maxBroadArr[1] = modArr[i];
                            else if (modArr[i] > maxBroadArr[2])
                                maxBroadArr[2] = modArr[i];
                        }
                        */
                    }
                    break;

            }

            return (modArr);
        }

        private double[] GetFreqHalfArr(double[] fArr)
        {
            double[] retArr = new double[fArr.Length / 2];

            for (int i = 0; i < retArr.Length; i++)
            {
                retArr[i] = fArr[i + 1];
            }

            return (retArr);
        }
    }
}

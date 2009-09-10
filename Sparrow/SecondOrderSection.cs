using System;
using System.Collections.Generic;
using System.Text;

namespace Sparrow
{
    // implements second order sections
    // implemented as direct form II
    class SecondOrderSection
    {
        private double ms;
        private double mb1;
        private double mb2;
        private double mb3;
        private double ma1;
        private double ma2;
        private double ma3;

        private double delay1 = 0;
        private double delay2 = 0;

        public SecondOrderSection(double s, double b1, double b2, double b3, double a1, double a2, double a3)
        {
            ms = s;
            mb1 = b1;
            mb2 = b2;
            mb3 = b3;
            ma1 = 1 / a1;
            ma2 = a2;
            ma3 = a3;
        }

        public double AddPoint(double newPt)
        {
            newPt = ms * newPt;

            double returnPt = 0;
            double nodePt = ma1*(newPt - ma2 * delay1 - ma3 * delay2);

            returnPt += delay2 * mb3;
            returnPt += delay1 * mb2;
            returnPt += nodePt * mb1;

            // move update the delays;
            delay2 = delay1;
            delay1 = nodePt;

            return (returnPt);
        }
    }
}

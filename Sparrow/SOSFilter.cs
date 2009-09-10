using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Sparrow
{
    public class SOSFilter
    {
        SecondOrderSection[] sosArr;

        public SOSFilter(StreamReader srObj)
        {
            char [] delminterChars = {','};
            List<SecondOrderSection> sosList = new List<SecondOrderSection>();
        

            while (srObj.EndOfStream != true)
            {
                string strLine = srObj.ReadLine();            
                string[] values = strLine.Split(delminterChars);
                // values should only be 6 items long
                if (values.Length > 8)
                {
                    Exception ex = new Exception("Mailformed SOS Matrix");
                    throw (ex);
                } 

                // the 7th spot is empty
                sosList.Add(new SecondOrderSection(Convert.ToDouble(values[0]),
                    Convert.ToDouble(values[1]),
                    Convert.ToDouble(values[2]),
                    Convert.ToDouble(values[3]),
                    Convert.ToDouble(values[4]),
                    Convert.ToDouble(values[5]),
                    Convert.ToDouble(values[6])));                
            }

            srObj.Close();

            sosArr = sosList.ToArray();
        }

        public double AddPoint(double newPt)
        {
            double filtPt = newPt;

            for (int i = 0; i < sosArr.Length; i++)
            {
                filtPt = sosArr[i].AddPoint(filtPt);
            }

            return (filtPt);
        }
    }
}

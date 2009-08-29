using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using NationalInstruments.DAQmx;

namespace Sparrow
{
    public partial class NI6251_Options : Form
    {
        // store the current control parameters incase of cancel
        private int backupSelectedIndex;
        private decimal backupMinValue;
        private decimal backupMaxValue;
        private decimal backupSamplesChannel;
        private decimal backupRate;

        private Task taskObj;

        public NI6251_Options()
        {
            InitializeComponent();
        
            // Get the channels available
            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.AI, PhysicalChannelAccess.External));
            if( physicalChannelComboBox.Items.Count > 0)
                physicalChannelComboBox.SelectedIndex = 0;
            
            SetupDevice();
        }

        private void SetupDevice()
        {
            taskObj = new Task();

            // setup the channel
            taskObj.AIChannels.CreateVoltageChannel(physicalChannelComboBox.Text, "", AITerminalConfiguration.Differential,
                Convert.ToDouble(minimumValueNumeric.Value), Convert.ToDouble(maximumValueNumeric.Value), AIVoltageUnits.Volts);

            // setup the timing, last value is the number of samples to use in the buffer
            taskObj.Timing.ConfigureSampleClock("", Convert.ToDouble(rateNumeric.Value),
                SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples, 2*Convert.ToInt32(samplesPerChannelNumeric.Value));

            taskObj.Control(TaskAction.Verify);
        }
        
        private void NI6251_Options_Shown(object sender, EventArgs e)
        {
            // when the options dialog is shown back up the options in case of cancel
            backupSelectedIndex = physicalChannelComboBox.SelectedIndex;
            backupMinValue = minimumValueNumeric.Value;
            backupMaxValue = maximumValueNumeric.Value;
            backupSamplesChannel = samplesPerChannelNumeric.Value;
            backupRate = rateNumeric.Value;
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            physicalChannelComboBox.SelectedIndex = backupSelectedIndex;
            minimumValueNumeric.Value = backupMinValue;
            maximumValueNumeric.Value = backupMaxValue;
            samplesPerChannelNumeric.Value = backupSamplesChannel;
            rateNumeric.Value = backupRate;
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            // don't know if this is needed
            taskObj.Control(TaskAction.Stop);
            SetupDevice();
        }

        // get the task
        public Task TaskObj
        {
            get
            {
                return (taskObj);
            }
        }

        public int SamplesPerChannel
        {
            get
            {
                return (Convert.ToInt32(samplesPerChannelNumeric.Value));
            }
        }

        public int Rate
        {
            get
            {
                return (Convert.ToInt32(rateNumeric.Value));
            }
        }
    }
}
# Sparrow

Sparrow is a downsampling power spectral desnsity analyzer written in C# currently complied against the .NET 4.0 framework. 

Latest changes make compiling work in Visual Studio Community 2015 (14.0.25425.01 Update 3). Downloadable for free in DreamSpark as us August 13, 2015. 

This version depends on NI-DAQ 9.7.5f1. NI states newer versions of NI-DAQ will remain backwards compatible. Compilations of against newer Ni-DAQ dlls will not run on systems running older Ni-DAQ drivers.   

To do: Make this compatiable with more DAQ devices, perhaps remote devices such as beaglebone black/raspberry pi

External dependancies for building the project, loading as references in VS, are in Sparrow/External References. GraphControl is compiled from CTCHunter1\CSHarp\Libraries\GraphControl

To do: Add error handling routines for checking errors at start up. Possible errors: no NI-DAQ installed, no DAQ card, wrong version of NI-DAQ installed, no .NET framework installed. This version is targeting x86-64 systems. Not seeing a need for targeting 32 bit x86 in release but could be changed in the build options fairly easily. 
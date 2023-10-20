# DHI-ISISConverter

This tool is used to convert ISIS file data into MIKE Hydro (.mhydro)

## Requirements

The DHI-ISISConverter tool works on Windows only.

#### MIKE Software

It is not required to install any other MIKE Software. This tool is a stand-alone tool. 

## Download and installation

Download the DHI-ISISConverter release zip file: [DHI-ISISConverter Releases download](https://github.com/DHI/DHI-ISISConverter/releases), unzip and run the `ISISConverter.exe`.

## Building a new releases of DHI-ISISConverter

Part of DHI-ISISConverter references are using NugetPackage. The remaining missing references are present in the DHI-ISISConverter release zip file. 

To build the source code: Download the release zip file, unzip it, and move the content into the folder `Source\release`.

To run and debug, copy the content of the `Source\release` to the output bin folder,  `Source\ISISConverterUI\bin\Debug\`.

To make a new release, replace the three `ISISConverter` files in the `Source\release` folder.
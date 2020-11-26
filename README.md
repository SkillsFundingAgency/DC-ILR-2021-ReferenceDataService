# DC-ILR-2021-ReferenceDataService
This code will create the second part of the ILR pipeline. 

A file is validated for gross errors in File Validation Service and then passed to the reference data service. 
The reference data service analyses the file and 'pulls out' the unique reference data elements that will be required and then goes to get those bits of reference data. 
The entire reference data is then saved, as a JSON blob, so that the ref data is invariant for the lifetime of the file processing (which could be 10's of minutes).
Different ref data is pulled in for online vs desktop service but the basic concepts are the same.

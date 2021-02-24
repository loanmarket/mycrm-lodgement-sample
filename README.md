The **Lodgement API** is a single entry point which is utilised by MyCRM to lodge and validate a an existing deal. Lodgement has been built to allow MyCRM to lodge with multiple third party API's. 

Lodgement uses Lixi packages for both Australia and New Zealand.
* CAL - 2.6.34
* CNZ - 2.1.7

# Lodge

POST Application/{applicationId}/Lodgement/Residential

Lodge a residential deal.

![image](https://user-images.githubusercontent.com/60586239/108928136-6ca2b180-768d-11eb-93f6-09841289fe6f.png)

# Validate

POST Application/{applicationId}/Lodgement/Residential/Validate

Validate a residential deal.

![image](https://user-images.githubusercontent.com/60586239/108928187-847a3580-768d-11eb-8aba-148960866170.png)

# Lodgement Target

A lodgement target is implemented per lender, the target can lodge and validate a lixi package.
Each target is independent and can have different security requirements, but all endpoints will require at minimum authorization. 

## Required Endpoints 
* Lodge
* Validate

![image](https://user-images.githubusercontent.com/60586239/108803573-bc7a6d80-75e6-11eb-89d9-876fdedf7e83.png)

## Retry Strategy

Calling into a target endpoint a retry policy will be utilised. The implementation of the retry strategy is on specific status codes retry up to 3 attempts with a exponential back off of 2 seconds to the power of the retry attempt.

Conditions:
* Status Code 420 - Enhance your calm;
* Timeout exception;

## Client Authentication

This section outlines how participants in the CDR regime will authenticate clients seeking access to end points.
Lodgement Targets should support the authentication of the Lodgement API using a signed JWT using the client credentials flow.

## Transaction Security

### Use of TLS
All HTTP calls MUST be made using HTTPS incorporating TLS >= 1.2.

### Use of MTLS
All communication between Loan Market and the lodgement target systems MUST incorporate MTLS as part of the TLS handshake.

## Backchannel

The backchannel allows a call-back endpoint for the lender to update a deal after being lodged. 

![image](https://user-images.githubusercontent.com/60586239/108803611-d9af3c00-75e6-11eb-9b49-df5fd111493e.png)

#Samples

All samples included within this repository are using .NET Core 3.1. The sample provides a very basic application which provides an example OpenAPI schema.

Required Software installations:
* [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)

## Running the sample
Open a console within ./samples/MyCRM.Lodgement.Sample and execute "dotnet run"
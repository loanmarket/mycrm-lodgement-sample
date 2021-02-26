# My CRM Residential Application Lodgement

MyCRM aims to integrate with a wide range of lodgement targets either by communicating directly to a lender or via 3rd party gateways. At the point of lodgement, based on configuration and predefined rules, the user is presented a selection of available lodgement targets for their loan application. Once a target is selected a lodgement package is created within MyCRM containing all available loan application information. This lodgement package is then sent to the selected lodgement target for validation, if the validation is successful, the package can then be lodged.  After initial lodgement, MyCRM will listen for any updates regarding the staus of the application so these statuses can be visible within MyCRM. The intention of this readme is to assist potential lodgement targets in completing this integration with MyCRM.

# How to integrate with MyCRM as a lodgement target

## Make contact with mycrm to set up configuration     
    - discuss any commercial agreements
    - which lenders and markets should be supported
    - Work through configuration of security
    - How can MyCRM test this integration, sandbox configuration? Prod configuration?
    - How can the lodgement target test this integration?

## Expose an api
    General api info
        - security
        - Retry strategy 
            Calling into a target endpoint a retry policy will be utilised. The implementation of the retry strategy is on specific status codes retry up to 3 attempts with a exponential back off of 2 seconds to the power of the retry attempt.
            Conditions:
            * Status Code 420 - Enhance your calm;
            * Timeout exception;
                - Valdiation endpoint
                - Submission endpoint
                - Communicating status updates back to mycrm

# Additional information
    - Security
    - Testing harness and getting started scenarios
    - Information on Lixi
        - Lodgement uses Lixi packages for both Australia and New Zealand.
            * CAL - 2.6.34
            * CNZ - 2.1.7
    
# Testing harness

All samples included within this repository are using .NET Core 3.1. The sample provides a very basic application which provides an example OpenAPI schema.

Required Software installations:
* [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)

## Running the sample
Open a console within ./samples/MyCRM.Lodgement.Sample and execute "dotnet run"




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


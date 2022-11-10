# My CRM Residential Application Lodgement

MyCRM aims to integrate with a wide range of lodgement targets either by communicating directly to a lender or via 3rd party gateways. At the point of lodgement, based on configuration and predefined rules, the user is presented a selection of available lodgement targets for their loan application. Once a target is selected a lodgement package is created within MyCRM containing all available loan application information. This lodgement package is then sent to the selected lodgement target for validation, if the validation is successful, the package can then be lodged.  After initial lodgement, MyCRM will listen for any updates regarding the staus of the application so these statuses can be visible within MyCRM. The intention of this readme is to assist potential lodgement targets in completing this integration with MyCRM.

# Lixi Version

Lodgement uses Lixi packages for both Australia and New Zealand.
* CAL - 2.6.35
* CNZ - 2.1.8

# MyCRM Lodgement Overview

The following diagrams demonstrates a highlevel overview of MyCRM and integration into the Target API.

## Lodge

POST Application/{applicationId}/Lodgement/Residential

Lodge a residential deal.

![image](https://user-images.githubusercontent.com/60586239/108928136-6ca2b180-768d-11eb-93f6-09841289fe6f.png)

## Validate

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

## Backchannel

POST Lodgement/Residential/Backchannel
The Lixi Standard could be CNZ or CAL. 

The backchannel allows a call-back endpoint for the lender to update a deal after being lodged. The backchannel endpoint will be developed and managed by Loan Market.
The LIXI package for CAL and CNZ already has the required fields.
The Lodgement Target should send these updates using the Lixi Standard. 

![image](https://user-images.githubusercontent.com/60586239/108803611-d9af3c00-75e6-11eb-9b49-df5fd111493e.png)

### Samples

```xml
<?xml version="1.0" encoding="UTF-8"?>
<Package>
    <Content>
        <Application>
            <Overview BrokerApplicationReferenceNumber="LoanScenario-1558767-ZYCANU1" LenderApplicationReferenceNumber="BNZ-005045"/>
        </Application>
    </Content>
    <Instructions>
        <ApplicationInstructions>
            <Update>
                <Event DateTime="2021-04-24T01:01:24" Name="Application Received" Details="Thanks for submitting this application. We've started working on it and will get back to you soon."/>
            </Update>
        </ApplicationInstructions>
    </Instructions>
    <Publisher LIXICode="LIXICode"/>
    <Recipient Description="Simpology Pty Ltd" LIXICode="SPLMO1"/>
    <SchemaVersion LIXITransactionType="CAL" LIXIVersion="2.6.35"/>
</Package>
```

### Authorization
The back channel uses [Okta](https://developer.okta.com/docs/guides/implement-client-creds/use-flow/) to Authenticate using client credentials.

Loan Market will supply the following details which are all required to authenticate:
* **Okta Domain** - the okta domain is different per environment;
* **Client Id**;
* **Client Secret**;
* **Scope** - integration.lodgement;


### Type Definitions
A separate Open API definition has been specified for the back channel.  

# OpenAPI

The Open API definitions versions are backed up to OpenApi Definitions.

# Samples

All samples included within this repository are using .NET Core 6 The sample provides a very basic application which provides an example OpenAPI schema.

Required Software installations:
* [.NET Core 6](https://dotnet.microsoft.com/download/dotnet)

## Application

![image](https://user-images.githubusercontent.com/60586239/109752327-f45b6380-7c2b-11eb-825f-e52005d3cfc6.png)

* Test Harness - Console application which will run a series of tests against the sample.
* Open API Spec - An API which provide a swagger specification and also acts as a proxy calling into the Target API.

## Running the sample
Open a console within ./samples/MyCRM.Lodgement.Sample and execute "dotnet run"


# Schemas

---

## Generating Classes

There are many tools to generate classes from an Open API Definition file. 
* [AutoRest](https://github.com/Azure/autorest) - Supports the majority of languages and has documentation.


## ValidationResult

| Name              | Type               | Required  | Description |
| ------------------|--------------------| ----------| ------------|
| ReferenceId       | string             | optional  | Obsolete, supporting previous existing implementations. |
| ValidationErrors  | [ValidationError]  | mandatory | The list of validation errors. |

## ValidationError

| Name              | Type                        | Required  | Description                                                   |
| ------------------|-----------------------------| ----------|---------------------------------------------------------------|
| Name              | string                      | optional  | Obsolete, supporting previous existing implementations..      |
| Code              | string                      | mandatory | Const 401.                                                    |
| IsValid           | bool                        | mandatory | Const false.                                                  |
| ErrorType         | string                      | optional  | Obsolete, supporting previous existing implementations.       |
| Attributes        | [ValidationErrorAttributes] | optional  | Contains the Error message that will be shown on MyCRM pop-up |

## ValidationErrorAttributes

| Name              | Type                          | Required  | Description |
| ------------------|-------------------------------| ----------| ------------|
| Ids               | string                        | optional  | Obsolete, supporting previous existing implementations. |
| ErrorMessage      | [ErrorMessage]                | optional  |  |

## ErrorMessage

| Name              | Type                          | Required  | Description |
| ------------------|-------------------------------| ----------| ------------|
| Title             | string                        | mandatory |  |
| Detail            | string                        | optional  |Obsolete, supporting previous existing implementations.   |


### Samples

```xml
<?xml version="1.0" encoding="utf-16"?>
<ValidationResult xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                  xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ValidationErrors>
        <ValidationError>
            <Code>401</Code>
            <IsValid>false</IsValid>
            <Attributes>
                <Ids />
                <ErrorMessage>
                    <Title>Proof of income wasn't supplied </Title>
                </ErrorMessage>
            </Attributes>
        </ValidationError>
        <ValidationError>
            <Code>401</Code>
            <IsValid>false</IsValid>
            <Attributes>
                <Ids />
                <ErrorMessage>
                    <Title>Proof of Identity wasn't supplied</Title>
                </ErrorMessage>
            </Attributes>
        </ValidationError>
        <ValidationError>
            <Code>401</Code>
            <IsValid>false</IsValid>
            <Attributes>
                <Ids />
                <ErrorMessage>
                    <Title>Employment history must have minimum of 3 years</Title>
                </ErrorMessage>
            </Attributes>
        </ValidationError>
    </ValidationErrors>
</ValidationResult>

```

<img width="581" alt="Validation Pop-up" src="https://user-images.githubusercontent.com/3873306/156328480-17881211-55e6-4036-b6cc-de95da608f8c.png">

# Download Lixi package
* Use the Swagger api url https://api.integration.mycrm.finance/swagger/index.html
Make sure the Lodgement Api is selected as shown below, 
The /Lodgement/Residential/Admin/Package endpoint is used to generate the package, it also allows to select the format Json or XML

![ScreeShot 2022-10-11 at 15 46 42@2x](https://user-images.githubusercontent.com/3873306/200980851-1311d380-fa2f-45a6-9786-d8b87940fbac.png)

* Log in to MyCrm using the credetial provided.
once you logged in to MyCRM filter the network by getuser in order to get the Bearer token so that you can copy it into the Swagger Authorize (exclude the “Bearer” prefix)


![ScreeShot 2022-10-11 at 15 49 40@2x](https://user-images.githubusercontent.com/3873306/200981097-09c38ac4-bd4d-4725-9523-0f5709b3e453.png)

* Open an application you've created.
from the application link copy the opportunityId in this example 114390700 and palace in the swagger and generate the package in the format you need.
https://integration.mycrm.finance/app/opportunity/fact-find?opportunityId=114390700&loanId=12345&sourceFamilyId=4403391&isContactOpener=1




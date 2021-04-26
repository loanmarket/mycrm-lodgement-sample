# My CRM Residential Application Lodgement

MyCRM aims to integrate with a wide range of lodgement targets either by communicating directly to a lender or via 3rd party gateways. At the point of lodgement, based on configuration and predefined rules, the user is presented a selection of available lodgement targets for their loan application. Once a target is selected a lodgement package is created within MyCRM containing all available loan application information. This lodgement package is then sent to the selected lodgement target for validation, if the validation is successful, the package can then be lodged.  After initial lodgement, MyCRM will listen for any updates regarding the staus of the application so these statuses can be visible within MyCRM. The intention of this readme is to assist potential lodgement targets in completing this integration with MyCRM.

# Lixi Version

Lodgement uses Lixi packages for both Australia and New Zealand.
* CAL - 2.6.34
* CNZ - 2.1.7

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

### Use of MTLS
All communication between Loan Market and the lodgement target systems MUST incorporate MTLS as part of the TLS handshake.

## Backchannel

POST Lodgement/{LixiStandard}/Backchannel
The Lixi Standard could be CNZ or CAL. 

The backchannel allows a call-back endpoint for the lender to update a deal after being lodged. The backchannel endpoint will be developed and managed by Loan Market.
The LIXI package for CAL and CNZ already has the required fields.
The Lodgement Target should send these updates using the Lixi Standard. 

![image](https://user-images.githubusercontent.com/60586239/108803611-d9af3c00-75e6-11eb-9b49-df5fd111493e.png)

### Samples

```json
{
  "Content":{
    "Application":{
      "Overview":{
        "BrokerApplicationReferenceNumber":"699111",
        "LenderApplicationReferenceNumber":"PP-005045"
      }
    }
  },
  "Instructions":{
    "ApplicationInstructions":{
      "Update":{
        "Event":[
          {
            "DateTime":"2021-04-14T11:35:37",
            "Name":"Settlement_Completed"
          }
        ]
      }
    }
  },
  "Publisher":{
    "LIXICode":"LIXICode"
  },
  "Recipient":[
    {
      "Description":"Lodgement Target",
      "LIXICode":"SPLMO1"
    }
  ],
  "SchemaVersion":{
    "LIXITransactionType":"CNZ",
    "LIXIVersion":"2.1.7"
  }
}
```

```json
{
  "Content":{
    "Application":{
      "Overview":{
        "BrokerApplicationReferenceNumber":"699111",
        "LenderApplicationReferenceNumber":"PP-004573"
      }
    }
  },
  "Instructions":{
    "ApplicationInstructions":{
      "Update":{
        "Event":[
          {
            "DateTime":"2021-04-14T10:20:11",
            "Name":"Application_Received"
          }
        ]
      }
    }
  },
  "Publisher":{
    "LIXICode":"LIXICode"
  },
  "Recipient":[
    {
      "Description":"Lodgement Target",
      "LIXICode":"LIXICode"
    }
  ],
  "SchemaVersion":{
    "LIXITransactionType":"CNZ",
    "LIXIVersion":"2.1.7"
  }
}
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

All samples included within this repository are using .NET Core 3.1. The sample provides a very basic application which provides an example OpenAPI schema.

Required Software installations:
* [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)

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
| ReferenceId       | string             | optional  | The reference id used for logging and debugging purposes |
| ValidationErrors  | [ValidationError]  | mandatory | The list of validation errors. |

## ValidationError

| Name              | Type                          | Required  | Description |
| ------------------|-------------------------------| ----------| ------------|
| Name              | string                        | optional  | Obsolete, supporting previous existing implementations.. |
| Code              | string                        | mandatory | A unique error code. |
| ErrorType         | string                        | optional  | Obsolete, supporting previous existing implementations. |
| Attributes        | [ValidationErrorAttributes]   | optional  | Obsolete, supporting previous existing implementations. |

## ValidationErrorAttributes

| Name              | Type                          | Required  | Description |
| ------------------|-------------------------------| ----------| ------------|
| Ids               | string                        | optional  | Obsolete, supporting previous existing implementations. |
| ErrorMessage      | [ErrorMessage]                | optional  | Obsolete, supporting previous existing implementations..  |

## ErrorMessage

| Name              | Type                          | Required  | Description |
| ------------------|-------------------------------| ----------| ------------|
| Title             | string                        | mandatory | Obsolete, supporting previous existing implementations.. |
| Detail            | string                        | optional  | Obsolete, supporting previous existing implementations..  |
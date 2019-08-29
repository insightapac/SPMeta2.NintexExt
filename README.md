# Copyright
This project is created by [Insight APAC](http://au.insight.com) - formerly  [Ignia](https://ignia.com.au). It is distributed under [Microsoft Public license](https://opensource.org/licenses/MS-PL)

# Introduction 
This project is an extension to the SPMeta2 library. 
At the moment the project allows you to deploy nintex forms to the SP2013 on prem. I am sure 2016 should work too but it has not been specificly tested.

# Getting Started
Look into the SPMeta2.NintexExt.CSOM.SP13.Test program for the sample of how you can use the package to deploy a nintex form. 
Onprovisioning/Onprovisioned code is optional.

The sample programs use libraries as-is, but for your programs it is recommended to use the nuget. 

https://www.nuget.org/packages/SPMeta2.NintexExt.CSOM.SP13/
https://www.nuget.org/packages/SPMeta2.NintexExt.CSOM.O365/


# Build and Test
Build the projects, this would be enough :).

# Contribute
There is a list of possible improvements down below.

# revisions history

## 0.0.17 - NintexO365 - NintexApiSettings.SemiSuccessFullPublishHttpErrorCodes
changed NintexApiSettings.SemiSuccessFullPublishHttpErrorCodes to a function ShouldApplySmartRetry. 
also added SmartRetryCheckResult.



## 0.0.16 - NintexO365 - NintexApiSettings.SemiSuccessFullPublishHttpErrorCodes
Addded a setting NintexApiSettings.SemiSuccessFullPublishHttpErrorCodes that allows to check if assignedUse or publish operation did in fact work, so we avoid unnecessarily retries.
We have found out that we have an error 502 bad gateway that actually does not prevent the operations from working. 

## 0.0.15 - NintexO365 - adding delays between retry timeouts
 

## 0.0.14 - NintexO365 - adding retry logic
    Addded a setting NintexApiSettings.MaxRetries that allows the http request to be resent for the cases of network glitches, etc.
default value is 3.

## 0.0.13 - NintexO365 - skipped for superstitious reasons

## 0.0.12 - NintexO365
 Apparently you can only need to set assigned use for production for the form when it is published. 
so now the publish is before assigned use and also is forced if you set assigneduseforproduction to any non null value

## 0.0.11 - NintexO365
 removed the ThreadStatic attribute as it does not seem to work as expected.
 Besides, we dont ever use this in multi tenant scenarious, where you need separate api keys and domains, this is very unlikely, if we get there we will bring it back.


## 0.0.10 - NintexO365

Added support for http client timeout setting.
NinteFormsApikeys is now renamed to NintexApiSettings


## 0.0.9 - Onprem
The authentication should work better in the scenarious when it is fired by the event (i.e. when you steal the cookies from form).
this authentication is used in some other contexts as well at times.
i had no chance to check it but i checked it does not break anything at least. 


## 0.0.9 - NintexO365

Added support for list and site workflows. 


## 0.0.8 - NintexO365

Added support for form content types. 


## 0.0.8 - Onprem
Workflows are now published via webrequestexecutor factory (thanks Mark for the contribution).
This adds support for Kerberos and any other protocol you would make the clientcontex work with

 

## 0.0.7 both
Added assembly signatures.


## 0.0.6 on prem
added https support.


## 0.0.5
refactoring, removed the SPMeta2.NintexExt.Core project alltogether.

namespaces are left in place 

we do not plan for now to develop an SSOM version on premise,
and the O365 obviously only gives us the CSOM version.

Moreover, features and definitions are so different between on prem and O365 so it would make sense to move the definitions
out of the core into the respective projects.

## 0.0.4
added support for site workflows on prem (sp13)

## 0.0.3
We have added a first version of Nintex for Office 365 forms. For now the content type is not supported yet and the form will be published to the default content type as 
the API documentation and the samples that Nintex provide are rather blurry. Also keep in mind that you might encounter stragne errors - in our case we could not test the assigned use
as apparently some licenses do not allow that.

## 0.0.2
Fixed the csom references for the on-premise package (now we refer to 15.0.0.0 to be compatible with old SDKs)
Note that the rerference to the CSOM package has been removed on purpose so you can use any sharepoint csom package as long as microsoft.sharepoint.client is at least 15.0.0.0


Added an initial ability to provision list workflows. As we had to separately call the nintex wcf service, for now only windows authentication is supported.
Try it and let us know .

## 0.0.1
Initial release with nintex forms publishing on prem only



# Appendix
TODO: list of planned improvments/issies

Try this code for better adfs integration
https://github.com/SharePoint/PnP-Sites-Core/blob/master/Core/SAML authentication.md

Add better support for the errors checkign after the request has been submitted, especially for the case of AD proxy.
Add AD Proxy support.
done - Add workflows.
done Add site workflows.
Add workflow schedules.



Office 365 
Add support for list and site workflows 


# Notes
1) Nintex recently added an "Assigned Use" for workflows on premises. 
   Unfortunately there is no API for this, so, based on the answer we got from support, 
      the workflows are published "as production". 
2) When requesting the API key for Office365, do not use "tenant-my" sites. Seems that you will be able to 
However, if you already have a web service url for mysites (https://tenant-my.nintexo365.com) you can still use it as a web service url and work across your main site
(https://tenant.sharepoint.com/...)

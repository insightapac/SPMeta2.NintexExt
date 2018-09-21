# Copyright
This project is created by [Ignia](https://ignia.com.au). It is distributed under [Microsoft Public license](https://opensource.org/licenses/MS-PL)

# Introduction 
This project is an extension to the SPMeta2 library. 
At the moment the project allows you to deploy nintex forms to the SP2013 on prem. I am sure 2016 should work too but it has not been specificly tested.

# Getting Started
Look into the SPMeta2.NintexExt.CSOM.SP13.Test program for the sample of how you can use the package to deploy a nintex form. 
Onprovisioning/Onprovisioned code is optional.

# Build and Test
Build the projects, this would be enough :).

# Contribute
There is a list of possible improvements down below.

# revisions history

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

Add better support for the errors checkign after the request hsa been submitted, especially for the case of AD proxy.
Add AD Proxy support.
Add workflows.
Add site workflows.
Add workflow schedules.


Office 365 forms
add support for all http responses storage 
add calculation of the content type

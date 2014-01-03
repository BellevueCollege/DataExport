## Overview

The **DataExport** tool is a web service that can be configured to collect, format and deliver internal data
to 3rd-party vendors. It is intended to be run from inside the network of organization who is providing the
data (e.g. the college).

## Requirements

+ .NET Framework 4 (the **Full** platform, not the **Client**)
+ [ASP.NET MVC3](http://www.microsoft.com/en-us/download/details.aspx?displaylang=en&id=1491&WT.mc_id=aff-n-in-loc--hr) (you may also need to see [this article](http://geekswithblogs.net/ranganh/archive/2011/10/26/installing-mvc-3-for-visual-studio-2010-on-windows-developer.aspx))
+ Visual Studio 2012 (later versions will probably work)
+ NuGet Package Manager (included in Visual Studio) - for various other 3rd-party libraries
+ [CtcApi](https://github.com/BellevueCollege/CtcApi) - included via [Bellevue College's NuGet package server](http://www.bellevuecollege.edu/dev/).

### Optional

The following are not required, stricly speaking, but will greatly improve your development experience with this project. Some features and/or functionality may also be unavailable without the following.

+ [Red-Gate's SQL Developer Bundle](https://www.red-gate.com/products/sql-development/sql-developer-bundle/) - Visual Studio will complain that it cannot open the **ClassSchedule.DB** project without this installed, but the files can - **and should** - still be modified and managed via source control. The included **.sdc* data migration project files also require these tools. Database installation, setup, deployment and synchronization will need to be managed manually.
+ [ReSharper](https://www.jetbrains.com/resharper/) - Some of the source code contains *ReSharper* comments that temporarily disable specific code suggestions. Please do not remove these.
+ [Git Source Control Provider](http://visualstudiogallery.msdn.microsoft.com/63a7e40d-4d71-4fbb-a23b-d262124b8f4c) - This *solution* uses this 3rd-party provider instead of the Git support build into Visual Studio 2012. (This is primarily a legacy decision.)


## The projects

+ **Database** - DataExport.DB
+ **Web service** - DataExport.WS
+ **Unit tests** - Test.DataExport.WS

### DataExport.DB

This project template is provided by the Red Gate SQL Connect product, which allows managing an MS SQL Server database as files, integration w/ source control.

### DataExport.WS

**NOTE:** If you are using CAS for Single Sign-On (our current configuration), you will need to set up IIS on your local machine,
so that the app is hosted at http://localhost/dataexport. **CAS login WILL NOT work with Visual Studio's built-in web server.**

#### Important files/folders

+ **_configSource** - Location for configuration settings that are specific to your organization. Use the *EXAMPLE-___.config* files as a guide to creating your own. The *Web.config* file contains references to these files, and **the application will not run without them**.
+ **SupportFiles** - Location for files needed for the application, such as SSH keys.
+ **Templates** - The application will look here for XSLT files used in formatting data.

### Test.DataExport.WS

The project contains unit tests for the **DataExport.WS** project.

## See also
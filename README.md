# Telemetry UI-WebAPI Swagger Read ME

### The onion architecture is employed in this sample application
> ####The onion arhicture follows the inversion princple. Dependencies are inverted in ward to the domain models. An image will help clarify the intent
![Image of onion architecture](https://sbrakl.files.wordpress.com/2014/11/111814_1006_onionarchit6.png?w=625)

Solution Folder Structure
---
1. 01. Core
2. 02. Infrastructure
3. 03. API
4. 04. Test

Implented Stories are
---
+ Story MR-001
+ Story MR-002
+ Story MR-003
+ Story MR-004
+ Story MR-007 --> Drop down for comparison

## Important Notes
---
> To run the solution successfuly, ensure the following is check or should be in place

1. Ensure the telemetry.json file in the folder 02. Infrastructure/Data/ is copied to the output directory
    +  To do this, right-click on the file and select property. In the advanced tab, ensure **Build Action** is set to **None** and **Copy        to Output Directory** is set to **Copy Always** <br/><br/>

    > The reason for this is that we want to maintain clean seperation of concerns and let each layer in the app does it's job.          **Remember!** Dependencies are inverted
    
2. Ensure Microsoft Message Queue is enabled on your machine. If this is not enabled, it will result to the following issues
   + Application will throw **NullReferenceException**, but this has been handled by checking windows services to ensure MSMQ is enabled        and running. _THIS WILL NOT HAPPEN as Message Queue will not initialize with the check in place_
   
   + Unit tests involving the need for data to be written to/read from Queue will fail gracefully and horribly :boom: and that's a :thumbsdown:
   
   > The following tests will fail as a result of MessageQueue not enabled
   
    :x: ShouldAddNewTelemetry <br/>
    :x:  ShouldReturnBadReQuestIfDuplicateTelemetryPosted
    
    > To enable Message Queue on your machine, go to control panel>Programs and Features -> Turn Windows features on or off and select Microsoft Message. Restart your machine if need be
   
3. To run swagger
 + navigate to localhost:<#your-port-number>/swagger
 
 
4. To get to the view for lap by lap compairon
  + navigate to localhost:<#your-port-number>/home/compare
  
5. Make MANOR.API your start-up project and you are good to go! :heavy_check_mark:


###HAPPY CODING!!

Feature: SmokeFormsTests
 	As a CWC user
	I want to open CWC forms
	So that I can create/update/delete business entities or execute business processes

Scenario Outline: Open new instance form
	Given I open "<page>" page
	When I click new button	
	Then Form with correct title is opened
Examples:
	| page                                            |
#Cash Point
	| Events                                          |
	| Configure Stock Positions                       | 
#Optimization
	| Optimization Settings                           |
	
Scenario Outline: Verify adding new Countries
	Given I open "<page>" page
	When I click create button
	Then I see add row is shown	
Examples:	
	| page|
#Optimization
	| Countries  |									 

Scenario Outline: Verify a create new instance form
	Given I open "<page>" page
	When I click create button	
	Then I see a form is opened successul
Examples:
	| page                                            |
#Cash Point
	| Manage                                          |	
#Stock
	| Internal Orders                                 |  
	| Inventory Checks Page                           |	
#Route
	| Routes                                          | 
#Asset
	| Asset Management                                |   
	| Region Asset Management Tables                  |   
	| Manufacturer Asset Management Tables            | 
	| Denomination Asset Management Tables            | 
	| Maintenance_Reason Asset Management Tables      | 
	| Failure_Reason Asset Management Tables          | 
	| Maintenance_Activity Asset Management Tables    | 
#Accounting	
	| Billing Lines                                   |  
														  

Scenario Outline: Open create instance form
	Given I open "<page>" page
	When I click create button	
	Then Form with correct title is opened
Examples:
	| page                                            |
	| Regions                                         |
	| Companies                                       |
	| Service Points                                  |
	| Visit Addresses                                 |
	| Location Groups                                 |
	| Contact persons                                 |
	| Debtors                                         |
	| Banks                                           |
	| Bank Account                                    |
	| Contracts                                       |
	| Sites                                           |
	| Location Type                                   |
	| CashPoint Types                                 |
	| Products                                        |
	| Materials                                       |
	| Service Types                                   |
	| Package Types                                   |
	| Bag Types                                       |
	| Destination Locations                           |
	| Bag Types Matching Material Types               |
	| Exchange Rates                                  |
	| Code Formats                                    |
#Cash Point
	| Cash Point Model                                |
	| Cash Point Status                               |
	| Category                                        |
	| Status Category                                 |
	| Cash Point Reason Codes                         |
#Call
	 | Call Monitor                                    |
	 | Sla Conditions                                  |
	 | Call Categories                                 |
	 | Requestors                                      |
	 | Solution Codes                                  |
	 | Failure Codes                                   |
#Order Processing
	 | Outbound orders view                            |
	 | Dispatch Orders View                            | 															
	 | Cash Center Reason Codes                        |
	 | Count Identifier                                |
	 | Packing Lines                                   |
	 | Streams view                                    |
#Stock
	 | Interbank Orders                                |
	 | Take into storage orders view                   |
	 | Stock Locations View                            |
	 | Stock Location Types View                       |
	 | Movement types page                             |
	 | Stock Owners View                               |
#Route
     #две стр с ексепш.  |
	 | Performance Indicators                          |
#Resource
	 | Workstations                                    |
	 | Cameras                                         |
	 | Employees                                       |
	 | Production Machines View                        |
	 | Person roles                                    |
#Asset
	 | Maintenance Orders                              |
	 | Maintenance Locations                           |
	 | Asset Groups                                    |
#Accounting	
	 | Invoice Configuration Lines                     |

 Scenario Outline: Verify Add image working
	Given I open "local" portal
	And I am login on portal
	And I open "<page>" page
	When I click add image	
	Then I see add row is shown
Examples:
	| page                                           |
#Order
	| Email Addresses for New Order Notification     |
#Cash Point	
	| Bank Number Labels                             |
	| Insurance Settings                             |
 #Order Processing - Settings
	| Banknote Series                                |
#Accounting
	| SETTLEMENT Profiles Settings and Manual Export |


Scenario Outline: Verify add new Cash Center Setting forms
	Given I open "local" portal
	And I am login on portal
	And I open "<page>" page
	When I click add image	
	Then I see a form is opened successul
Examples:
	| page                        |
	| Cash Center Site Setting    |
	| Cash Center Process Setting |

Scenario: Verify opening new Pre-announcements View(Capture Deposit Specification) form
	Given I open "local" portal
	And I am login on portal
	And I open "Pre-announcements View" page
	When I click new button	
	Then I see a form is opened successul

Scenario: Verify opening CIT Process Settings form
	Given I open "local" portal
	And I am login on portal
	And I open "CIT Process Settings" page
	When I click add image
	And I confirm copy all settings
	Then I see a form is opened successul
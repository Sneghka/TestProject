Feature: Smoke Tests
 	As a CWC administrator
	I want to be able to open any CWC page
	So that I have access to all functionality	   

Scenario Outline: Open all pages on CWC site	
	When I open "<page>" page	
	Then I see page is opened successul
Examples:
	| page                                            |
	| User accounts                                   |
	| Groups                                          |
	| Departments                                     |
	| Set Workstation                                 |
	| Security Settings                               |
	| Locked IP addresses                             |
	| Locks                                           |
	| Password Hash                                   |
	| Job instances                                   |
	| Servers                                         |
	| Migration of Contracts                          |
	| Log Manager                                     |
	| Log Notifications                               |
	| Test Feedings                                   |
	| Automated Transaction Creation                  |
	| Archiving Settings                              |
	| Configuration Keys                              |
	| Performance Settings                            |
	| Replication Parties                             |
	| Replication Packs                               |
	| Format Settings                                 |
	| Regions                                         |
	| Companies                                       |
	| Service Points                                  |
	| Visit Addresses                                 |
	| Location Services                               |
	| Location Groups                                 |
	| Location Open Time Settings                     |
	| Contact persons                                 |
	| Contact Person Notifications                    |
	| Debtors                                         |
	| Banks                                           |
	| Bank Account                                    |
	| Contracts                                       |
	| Sites                                           |
	| Customer Groups                                 |
	| Market Segments                                 |
	| Location Handling Types                         |
	| Location Type                                   |
	| CashPoint Types                                 |
	| Products                                        |
	| Product Groups                                  |
	| Materials                                       |
	| Service Types                                   |
	| Package Types                                   |
	| Bag Types                                       |
	| Item Types                                      |
	| Reason Codes                                    |
	| Servicing Codes                                 |
	| Destination Locations                           |
	| Bag Types Matching Material Types               |
	| Bank Holiday Settings                           |
	| Currencies                                      |
	| Exchange Rates                                  |
	| Cash Center Site Setting                        |
	| Cash Center Process Setting                     |
	| CIT Process Settings                            |
	| Code Formats                                    |
	| Countries                                       |
	| Monitor                                         |
	| Manage                                          |
	| Events                                          |
	| POS Reconciliation                              |
	| Configure Stock Positions                       |
	| Automated Order Creation - Optimization Level 1 |
	| Bank Number Labels                              |
	| Stock Value                                     |
	| Transactions                                    |
	| Transaction Details                             |
	| Status Messages                                 |
	| Errors per Cash point                           |
	| Errors per Category                             |
	| Transactions per Cash Point                     |
	| Collected and Issued Values                     |
	| Cash Point Uptime                               |
	| Cash Availability                               |
	| Residual Percentage                             |
	| Cash Point Model                                |
	| Cash Point Status                               |
	| Category                                        |
	| Status Category                                 |
	| Insurance Settings                              |
	| Cash Point Reason Codes                         |
	| Stock View                                      |
	| Order Propositions                              |
	| Optimization Settings                           |
	| Order Entry                                     |
	| All Orders                                      |
	| My Orders                                       |
	| Orders per Denomination                         |
	| Update Order Status Configuration               |
	| Email Addresses for New Order Notification      |
	| Call Monitor                                    |
	| Task Monitor                                    |
	| Solution time                                   |
	| Number of Calls per status                      |
	| SLA Performance                                 |
	| Sla Conditions                                  |
	| Call Categories                                 |
	| Requestors                                      |
	| Solution Codes                                  |
	| Failure Codes                                   |
	| Notification Settings                           |
	| Receive Container                               |
	| Unpack Deposit                                  |
	| Capture Deposit Specification                   |
	| Count Deposit                                   |
	| Second Step Counting Form                       |
	| Exception Management                            |
	| Inbound Day Closure                             |
	| Pre-announcements View                          |
	| Received Containers View                        |
	| Count orders                                    |
	| Containers Batches View                         |
	| First step count results view                   |
	| 2nd step count results view                     |
	| Stock owner count results view                  |
	| Counting In Progress View                       |
	| Outbound orders view                            |
	| Orders Batches                                  |
	| Pick and Pack Transport Units                   |
	| Outbound containers                             |
	| Dispatch Orders View                            |
	| Outbound Day Closure                            |
	| General Day Closure                             |
	| Order processing discrepancies                  |
	| Status for Today                                |
	| Day Closure History View                        |
	| Cash Center Reason Codes                        |
	| Count Identifier                                |
	| Packing Lines                                   |
	| Streams view                                    |
	| Banknote Series                                 |
	| Seal Container                                  |
	| Unseal Container                                |
	| Reconcile                                       |
	| Stock Correction                                |
	| Confirm Storage Device Delivery                 |
	| Internal Orders                                 |
	| Inventory Checks Page                           |
	| Take into Storage                               |
	| Production                                      |
	| Interbank Orders                                |
	| Stock transactions view                         |
	| Take into storage orders view                   |
	| Stock containers view                           |
	| Value movements view                            |
	| Container movement view                         |
	| Actual stock view                               |
	| Stock value history view                        |
	| Stock Dashboard                                 |
	| Stock Locations View                            |
	| Stock Location Types View                       |
	| Movement types page                             |
	| Stock Owners View                               |
	| Visit Monitor                                   |
	| Routes                                          |
	| Search by Routes                                |
	| Stock Companies                                 |
	| Branches                                        |
	| Vehicles                                        |
	| Perfomance Routes                               |
	| Missed Visits and Recovered Services            |
	| Performance Indicators                          |
	| Workstations                                    |
	| Cameras                                         |
	| Resource Vehicles                               |
	| Employees                                       |
	| Production Machines View                        |
	| Vehicle Types                                   |
	| Person roles                                    |
	| Counting Performance View                       |
	| Asset Management                                |
	| Asset Planning                                  |
	| Maintenance Orders                              |
	| Asset                                           |
	| Asset Stock View                                |
	| Scan Asset                                      |
	| Region Asset Management Tables                  |
	| Manufacturer Asset Management Tables            |
	| Denomination Asset Management Tables            |
	| Maintenance_Reason Asset Management Tables      |
	| Failure_Reason Asset Management Tables          |
	| Maintenance_Activity Asset Management Tables    |
	| Maintenance Locations                           |
	| Asset Groups                                    |
	| Billing Lines                                   |
	| Cash Flow Information                           |
	| Reconciliation Frame                            |
	| Reconciliation View Frame                       |
	| SETTLEMENT Profiles Settings and Manual Export  |
	| Order Pre-announcements                         |
	| Pre-crediting value view                        |
	| Invoice Lines                                   |
	| Invoice Configuration Lines                     |
	| Locations                                       |
	| Pack Lifecycle                                  |
	| Usage Statistics                                |
	| About WebPortal                                 |
	| Personal details                                |
	| Change password                                 |








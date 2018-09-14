@cashcenter-data-generation-required
@contract-data-generation-required
@service-order-import-format-b
Feature: Service Order Import Format B Interface
		As a Sanid employee 
		I want to import Bank Orders
		So that delivery orders could be processed in CWC

Scenario: Service order is imported correctly by Service Order Import Format B Interface
	Given Import Express Delivery Order File is created with following Leading record
			| Record type	| Bank identification   | 
			| 1				| 100020				| 
	  And Import Express Delivery Order File is created with following Order record 
			| Record type	| Address code | Account number	| Bag type	| Reference		| Bank reference		|
			| 2				| A18841       | 8839123008		| 3306		| Referenceb	| BRef77a485fbec894378	|
	  And Import Express Delivery Order File is created with following Order item record  
			| Record type	| Article code	| Quantity	|
			| 3				| B005B001		| 2			|
			| 3				| B200B001		| 3			|
	  And Import Express Delivery Order File is created with following Ordered delivery information record  
			| Record type	| Name			| Date of birth		| Phone number		| Street	| House number	| House number addition	| Post code	| City	|
			| 4				| 1225ATM010A	| 19500101			| 987263567990123	| 7Street	| 46			| A						| 1000AA	| Kyiv	|
	  And Import Express Delivery Order File is created with following Close record 
			| Record type	| Number Of Detail Records	|
			| 9				| 4							|
	 When Service Order Import Format B Interface processes file
	 Then Service order was created with correct attributes
	  And Service products was created correctly
	  #And Service Order Import Format B Log record is created with 'Ok' result
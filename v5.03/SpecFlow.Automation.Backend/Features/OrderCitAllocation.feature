@contract-data-generation-required
@cit-order-allocation
@transport-data-clearing-required
Feature: OrderCitAllocation	
		As a cash center operator 
		I want service orders to be allocated to CIT Site in the form of transport orders
		So that transport orders could be processed by CIT


Scenario Outline: Service Order is allocated correctly in the form of transport orders	
	Given <serviceTypes> service order with service date in 0 days, '<genericStatus>' generic status and following content is created for '5505ATM01' location
			| Product			   | Quantity | IsLoose |
			| 10 EUR Bundle		   | 2        | false   |
			| 20 EUR Loose Product | 2        | true    |
	When CIT Allocation job processes Service Order
	Then System creates Transport Order for '5505ATM01' location  with Delivery service type
	And System creates correct Transport Order products	
			| Product				| Quantity |		
			| 20 EUR Loose Product  | 2        |
	And System creates successful Order CIT Allocation Job Log record with result OK for current Transport Order

	#IN DEVELOPMENT
	Examples: ServiceTypes
      | serviceTypes | genericStatus |
      | Delivery     | Registered    |
      | Delivery     | Unconfirmed   |
      | Delivery     | Confirmed     |
      | Collection   | Unconfirmed   |
      



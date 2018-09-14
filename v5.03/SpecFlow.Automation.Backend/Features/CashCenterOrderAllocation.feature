@cashcenter-data-generation-required
@contract-data-generation-required
@transport-data-clearing-required
Feature: CashCenterOrderAllocation
		As a cash center operator 
		I want service orders to be allocated into correct outbound orders
		So that requested items can be properly prepared for delivery

Scenario: Allocate service order for service point serviced by notes cash center
	Given Delivery service order with service date in 0 days, 'Registered' generic status and following content is created for '5505ATM01' location
			| Product        | Quantity | IsLoose |
			| 10 EUR Bundle  | 2        | false   |
			| 100 EUR Bundle | 2        | false   |
	 When CC Orders Allocation job processes service order
	 Then System creates outbound order for 'NCC' cash center and '5505ATM01' location
	  And Outbound order got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 4        | 22000 | 0.4   |
	  And System creates 1 CC Orders Allocation Log record with 'Ok' result for current service order

Scenario: Allocate service order for service point serviced by coins cash center
	Given Delivery service order with service date in 0 days, 'Registered' generic status and following content is created for '5506ATM01' location
			| Product    | Quantity | IsLoose |
			| 1 EUR Roll | 2        | false   |
			| 2 EUR Roll | 2        | false   |			
	 When CC Orders Allocation job processes service order
	 Then System creates outbound order for 'CCC' cash center and '5506ATM01' location
	  And Outbound order got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 4        | 150   | 0.8    |
	  And System creates 1 CC Orders Allocation Log record with 'Ok' result for current service order

Scenario: Allocate service order for service point serviced by foreign currency cash centers
	Given Delivery service order with service date in 0 days, 'Registered' generic status and following content is created for '5507ATM01' location
			| Product        | Quantity | IsLoose |
			| 10 USD Bundle  | 2        | false   |
			| 100 USD Bundle | 2        | false   |			
	 When CC Orders Allocation job processes service order
	 Then System creates outbound order for 'FCC' cash center and '5507ATM01' location
	  And Outbound order got correct quantity, value and weight
			| Quantity | Value   | Weight |
			| 4        | 22000   | 0.4    |
	  And System creates 1 CC Orders Allocation Log record with 'Ok' result for current service order

Scenario: Allocate service order for service point serviced by notes and coins cash centers
	Given Delivery service order with service date in 0 days, 'Registered' generic status and following content is created for '5507ATM02' location
			| Product        | Quantity | IsLoose |
			| 10 EUR Bundle  | 2        | false   |
			| 100 EUR Bundle | 2        | false   |
			| 1 EUR Roll     | 2        | false   |
			| 2 EUR Roll     | 2        | false   |	
	 When CC Orders Allocation job processes service order
	 Then System creates 2 outbound orders for current service order
	  And Outbound order for 'NCC' cash center and regular products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 4        | 22000 | 0.4    |
	  And Outbound order for 'CCC' cash center and regular products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 4        | 150   | 0.8    |
	  And System creates 2 CC Orders Allocation Log records with 'Ok' result for current service order

Scenario: Allocate service order for service point serviced by notes and foreign currency cash centers
	Given Delivery service order with service date in 0 days, 'Registered' generic status and following content is created for '5507ATM03' location
			| Product        | Quantity | IsLoose |
			| 10 EUR Bundle  | 2        | false   |
			| 100 EUR Bundle | 2        | false   |
			| 10 USD Bundle  | 2        | false   |
			| 100 USD Bundle | 2        | false   |
	 When CC Orders Allocation job processes service order
	 Then System creates 2 outbound orders for current service order
	  And Outbound order for 'NCC' cash center and regular products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 4        | 22000 | 0.4    |
	  And Outbound order for 'FCC' cash center and regular products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 4        | 22000 | 0.4    |
	  And System creates 2 CC Orders Allocation Log records with 'Ok' result for current service order

Scenario: Allocate service order for service point serviced by coins and foreign currency cash centers
	Given Delivery service order with service date in 0 days, 'Registered' generic status and following content is created for '5507ATM04' location
			| Product        | Quantity | IsLoose |
			| 1 EUR Roll     | 2        | false   |
			| 2 EUR Roll     | 2        | false   |
			| 10 USD Bundle  | 2        | false   |
			| 100 USD Bundle | 2        | false   |
	 When CC Orders Allocation job processes service order
	 Then System creates 2 outbound orders for current service order
	  And Outbound order for 'CCC' cash center and regular products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 4        | 150   | 0.8    |
	  And Outbound order for 'FCC' cash center and regular products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 4        | 22000 | 0.4    |
	  And System creates 2 CC Orders Allocation Log records with 'Ok' result for current service order

Scenario: Allocate service order for service point not serviced by any site
	Given Delivery service order with service date in 0 days, 'Registered' generic status and following content is created for '5507ATM99' location
			| Product        | Quantity | IsLoose |
			| 10 EUR Bundle  | 2        | false   |			
	 When CC Orders Allocation job processes service order
	 Then System creates 1 CC Orders Allocation Log record with 'Failed' result for current service order
	  And CC Orders Allocation Log record got following message
			| Message                                                                                                   |
			| Order ‘{0}’ cannot be allocated - no stream is found per CC site(s) configurated at location ‘5507ATM99’. |

Scenario: Allocate service order with all kind of products for service point serviced by notes, coins and foreign currency cash centers
	Given Delivery service order with service date in 0 days, 'Registered' generic status and following content is created for '5507ATM05' location
			| Product                | Quantity | IsLoose | IsBarcoded |
			| 1 EUR Roll             | 2        | false   | false      |
			| 10 EUR Bundle          | 2        | false   | false      |
			| 1 USD Roll             | 2        | false   | false      |
			| 10 USD Bundle          | 2        | false   | false      |
			| 1 EUR Barcoded Bundle  | 2        | false   | true       |
			| 10 EUR Barcoded Bundle | 2        | false   | true       |
			| 1 USD Barcoded Bundle  | 2        | false   | true       |
			| 10 USD Barcoded Bundle | 2        | false   | true       |
			| 1 EUR Loose Product    | 2        | true    | false      |
			| 20 EUR Loose Product   | 2        | true    | false      |
			| 1 USD Loose Product    | 2        | true    | false      |
			| 20 USD Loose Product   | 2        | true    | false      |
	 When CC Orders Allocation job processes service order
	 Then System creates 10 outbound orders for current service order
	  And Outbound order for 'NCC' cash center and regular products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 2        | 2000  | 0.2    |
	  And Outbound order for 'NCC' cash center and barcoded products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 2        | 200   | 0.02   |
	  And Outbound order for 'NCC' cash center and notes loose products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 2        | 4000  | 0.2    |	  
	  And Outbound order for 'CCC' cash center and regular products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 2        | 50    | 0.375  |
	  And Outbound order for 'CCC' cash center and barcoded products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 2        | 20    | 0.15   |
	  And Outbound order for 'CCC' cash center and coins loose products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 2        | 200   | 1.5    |	  
	  And Outbound order for 'FCC' cash center and regular products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 4        | 2050  | 0.575  |
	  And Outbound order for 'FCC' cash center and barcoded products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 4        | 220   | 0.17   |
	  And Outbound order for 'FCC' cash center and notes loose products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 2        | 4000  | 0.2    |
	  And Outbound order for 'FCC' cash center and coins loose products got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 2        | 200   | 1.5    |	  
	  And System creates 10 CC Orders Allocation Log records with 'Ok' result for current service order

Scenario: Allocate service order with note, coin and foreign currency products for service point serviced by notes cash center
	Given Delivery service order with service date in 0 days, 'Registered' generic status and following content is created for '5505ATM01' location
			| Product                | Quantity |
			| 1 EUR Roll             | 2        | 
			| 10 EUR Bundle          | 2        | 		
			| 10 USD Bundle          | 2        |		
	 When CC Orders Allocation job processes service order
	 Then System creates outbound order for 'NCC' cash center and '5505ATM01' location
	  And Outbound order got correct quantity, value and weight
			| Quantity | Value | Weight |
			| 6        | 4050  | 0.775   |
	  And System creates 1 CC Orders Allocation Log record with 'Ok' result for current service order




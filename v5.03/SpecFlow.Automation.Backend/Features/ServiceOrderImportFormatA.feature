@service-order-import-format-a
Feature: Service Order Import Format A Interface
		As a Sanid employee 
		I want to import Bank Orders
		So that delivery orders could be processed in CWC

Scenario: Service order is imported correctly by Service Order Import Format A Interface
	Given Excel file is created with following header attributes and service date in 2 days
			| Key            | Value      |
			| Company Name   | Blamburlam |
			| Company Number | 3303       |
			| Service Type   | DELV       |
	  And Row with following attributes is added to created excel file
			| Location Name     | Location Code | SAR 10 | SAR 50 | SAR 100 | SAR 500 | USD 100 |
			| Order Import FA01 | ATM01         | 2      | 2      | 2       | 2       | 2       |
	 When Service Order Import Format A Interface processes excel file
	 Then System creates service order with correct attributes
	  And System creates service products correctly
	  And Service Order Import Format A Log record is created with 'Ok' result
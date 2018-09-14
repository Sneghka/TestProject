@cashcenter-data-generation-required
Feature: CashCenterFirstStepCounting
		As a cash center operator 
		I want incoming containers to be counted correctly on First Step Count form
		So that the stock in cash center is increased properly

Scenario: TBD
	Given Service point with following attributes is configured
	  And I have entered 70 into the calculator
	 When I press add
	 Then the result should be 120 on the screen

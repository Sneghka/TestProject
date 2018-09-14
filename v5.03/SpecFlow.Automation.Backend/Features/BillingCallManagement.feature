@contract-data-generation-required
@callmanagement-data-generation-required
Feature: BillingCallManagement	
	As an operator
	I want to configure price lines for Call related activities
	So that the incidents can be invoiced to customer

Scenario: Add two numbers
   Given Price level is configured
		 | PriceRule        | UnitOfMeasure | PriceRuleLevelName | PriceRuleLevelValueType | Entity			| Code			|
		 | PricePerIncident | Incident      | CallCategory       | CallCategory            | CallCategory	| Incident		|	 
		 | PricePerIncident | Incident      | SolutionCode       | SolutionCode            | SolutionCode	| 001			|
	 And Price line is saved
		 | PriceRule        | UnitOfMeasure | Units | Price |
		 | PricePerIncident | Incident      | 1     | 10    |
	 And Call is created
		 | Customer | Location  | Number | Status    | Priority | CallCategory | SolutionCode | Requestor	|
		 | 5505     | 5505ATM01 | C0001  | Completed | High     | Incident     | 001          | kyrychek	|
	When Billing job processes price line
	Then System creates billed case		 
	 And System creates billing line
		 | Value |
		 | 10    |

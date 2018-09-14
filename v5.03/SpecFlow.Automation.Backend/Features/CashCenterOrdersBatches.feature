@cashcenter-data-generation-required
@contract-data-generation-required
@transport-data-clearing-required
Feature: CashCenterOrdersBatches
		As a cash center operator 
		I want stock orders to be grouped into orders batches
		So that processing of multiple orders can be optimized

		  # 3.4.4 Validate Linkage of Stock Orders to Orders Batch
Scenario: Validate linkage of two stock orders with regular products to orders batch
	Given Stock order with 'regular' type is created for '5505ATM01' location 
	  And Stock order with 'regular' type is created for '5505ATM02' location 
	 When User validates linkage of created stock orders
	 Then Validation is successful

		  # 3.4.4 Validate Linkage of Stock Orders to Orders Batch
Scenario: Validate linkage of two stock orders with loose products to orders batch
	Given Stock order with 'notes loose' type is created for '5505ATM01' location 
	  And Stock order with 'coins loose' type is created for '5505ATM02' location 
	 When User validates linkage of created stock orders
	 Then Validation is successful
		
		  # 3.4.4 Validate Linkage of Stock Orders to Orders Batch
Scenario: Validate linkage of two stock orders with regular and barcoded products to orders batch
	Given Stock order with 'regular' type is created for '5505ATM01' location 
	  And Stock order with 'barcoded' type is created for '5505ATM02' location 
	 When User validates linkage of created stock orders
	 Then Validation is successful

		  # 3.4.5 Link Multiple Stock Orders to Orders Batch
Scenario: Link two stock orders with regular products to new orders batch
	Given Stock order with 'regular' type is created for '5505ATM01' location 
	  And Stock order with 'regular' type is created for '5505ATM02' location 
	 When User links stock orders for the next locations to new orders batch
		  | Location  |
		  | 5505ATM01 |
		  | 5505ATM02 |
	 Then System links stock orders for the next locations to the orders batch
	      | Location  |
		  | 5505ATM01 |
		  | 5505ATM02 |

		  # 3.4.5 Link Multiple Stock Orders to Orders Batch
Scenario: Link stock order with regular products to existing orders batch
	Given Stock order with 'regular' type is created for '5505ATM01' location 
	  And Stock order with 'regular' type is created for '5505ATM02' location 
	  And Stock orders for the next locations are linked to new orders batch
	      | Location  |
		  | 5505ATM01 |
	 When User links stock orders for the next locations to created orders batch
	      | Location  |
		  | 5505ATM02 |
	 Then System links stock orders for the next locations to the orders batch
	      | Location  |		  
		  | 5505ATM02 |

		  # 3.4.5 Link Multiple Stock Orders to Orders Batch
Scenario: Link stock order with loose products to existing orders batch
	Given Stock order with 'notes loose' type is created for '5505ATM01' location 
	  And Stock order with 'coins loose' type is created for '5505ATM02' location 
	  And Stock orders for the next locations are linked to new orders batch
	      | Location  |
		  | 5505ATM01 |
	 When User links stock orders for the next locations to created orders batch
	      | Location  |
		  | 5505ATM02 |
	 Then System links stock orders for the next locations to the orders batch
	      | Location  |		  
		  | 5505ATM02 |

		  # 3.4.5 Link Multiple Stock Orders to Orders Batch, extension 1b
Scenario: Link stock order with loose products to existing orders batch with regular products order
	Given Stock order with 'regular' type is created for '5505ATM01' location 
	  And Stock order with 'coins loose' type is created for '5505ATM02' location 
	  And Stock orders for the next locations are linked to new orders batch
	      | Location  |
		  | 5505ATM01 |
	 When User links stock orders for the next locations to created orders batch
	      | Location  |
		  | 5505ATM02 |
	 Then Linkage is failed
	      | Message                                                                                                                  |
	      | Outbound orders of loose products type cannot be assigned to orders batch together with outbound orders of another type. |

		  # 3.4.5 Link Multiple Stock Orders to Orders Batch, extension 1b
Scenario: Link stock order with regular products to existing orders batch with loose products order
	Given Stock order with 'regular' type is created for '5505ATM01' location 
	  And Stock order with 'coins loose' type is created for '5505ATM02' location 
	  And Stock orders for the next locations are linked to new orders batch
	      | Location  |
		  | 5505ATM02 |
	 When User links stock orders for the next locations to created orders batch
	      | Location  |
		  | 5505ATM01 |
	 Then Linkage is failed
	      | Message                                                                                                                  |
	      | Outbound orders of loose products type cannot be assigned to orders batch together with outbound orders of another type. |

		  # 3.4.4 Validate Linkage of Stock Orders to Orders Batch, extension 1b
		  # For at least one Stock Order: Stock Order → Service Order does not exist
Scenario: Validate linkage to orders batch when service order is not replicated
	Given Stock order with 'regular' type is created for '5505ATM01' location
	  And Service order of created stock order is not yet replicated
	 When User validates linkage of created stock order
	 Then Validation is failed
	      | Message                                                                                         |
	      | Required service order(s) is(are) not replicated to current site. Please contact administrator. |

		  # 3.4.4 Validate Linkage of Stock Orders to Orders Batch, extension 1c
		  # When “local CIT Depots” of selected Stock Orders are not the same in Stock Orders List {1}
Scenario: Validate linkage of two stock orders with different CIT depots to orders batch
	Given Stock order with 'regular' type is created for '5505ATM01' location
	  And Stock order with 'regular' type is created for '5506ATM01' location
	 When User validates linkage of created stock orders
	 Then Validation is failed
	      | Message                                                |
	      | Please, select orders addressed to the same CIT depot. |

		  # 3.4.4 Validate Linkage of Stock Orders to Orders Batch, extension 1d
		  # When Sites of selected Stock Orders are not the same in Stock Orders List {1}.
		  # bad example of using THEN step for post-scenario actions. It's only because there is no hooks class for current feature. DO NOT USE SUCH APPROACH!
Scenario: Validate linkage of two stock orders with different cash centers to orders batch
	Given Servicing CIT depot of '5506ATM01' location is set to 'NCIT' CIT
	  And Stock order with 'regular' type is created for '5505ATM01' location
	  And Stock order with 'regular' type is created for '5506ATM01' location
	 When User validates linkage of created stock orders
	 Then Validation is failed
	      | Message                                           |
	      | Please, select orders addressed to the same site. |
	  And Servicing CIT depot of '5506ATM01' location is set to 'CCIT' CIT

		  # 3.4.4 Validate Linkage of Stock Orders to Orders Batch, extension 1e
		  # At least one selected Stock Order in Stock Order List {1} has [Status <> ‘registered’ OR Stock Order → Batch → Status <> ‘registered’]
Scenario: Validate linkage of stock orders with inappropriate status to orders batch
	Given Stock order with 'regular' type is created for '5505ATM01' location
	  And Stock order with 'regular' type is created for '5505ATM02' location
	  And Stock order for '5505ATM01' location has 'in progress' status
	  And Stock order for '5505ATM02' location has 'in progress' status
	 When User validates linkage of created stock orders
	 Then Validation is failed with parameter
	      | Message                                                                                                                                                                                                           |
	      | Stock Orders cannot be linked to Order Batch. At least one selected Stock Order has Status which is not ‘registered’ or linked to Orders Batch with Status which is not ‘registered’. Incorrect Stock Orders: {0} |


		  # 3.4.4 Validate Linkage of Stock Orders to Orders Batch, extension 1f
Scenario: Validate linkage of two stock orders with incompatible types to orders batch
	Given Stock order with 'regular' type is created for '5505ATM01' location 
	  And Stock order with 'notes loose' type is created for '5505ATM02' location 
	 When User validates linkage of created stock orders
	 Then Validation is failed
		  | Message                                                                                                                  |
		  | Outbound orders of loose products type cannot be assigned to orders batch together with outbound orders of another type. |
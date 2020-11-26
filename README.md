# DC-ILR-2021-FundingService-SF
DC-ILR-2021-FundingService-SF


** Rulebase Update WITHOUT interface changes. **

Zip file to be replaced as Embedded Resource in the funding model specific service project. E.g. src\FM36\ESFA.DC.ILR.FundingService.FM36.Service\Rulebase. 

Run Unit tests. 

If any Rulebase interface tests fail this indicates there was a change from the previous rulebase version that was not expected. Further investigation required. 

If tests pass - copy the same zip file as Content to the funding model specific test project, "RulebaseMasterFiles". E.g. src\FM36\ESFA.DC.ILR.FundingService.FM36.Service.Tests\RulebaseMasterFiles. 
This is used as the comparison and last known working copy for the unit tests on subsequent rulebase drops. 

Commit. 
# DC-ILR-2021-FundingService
This code will create the fourth part of the ILR pipeline. 
Valid learners will be output from the validation service and joined to the ref data from earlier in the pipeline. The funding service will then iterate through each learner and learning delivery and construct an Oracle Policy Automation XML fragment (XDS) and invoke OPA accordingly. The results are then collated back together and saved to storage for each fudning model. So FM36, FM25, etc...
The Online version is a service fabric stateless service with actors for each of the funding models. The desktop version turns into a signle threaded nuget package for instantiation within the desktop service application.

---

**Rulebase Update WITHOUT interface changes.**

Zip file to be replaced as Embedded Resource in the funding model specific service project. E.g. src\FM36\ESFA.DC.ILR.FundingService.FM36.Service\Rulebase. 

Run Unit tests. 

If any Rulebase interface tests fail this indicates there was a change from the previous rulebase version that was not expected. Further investigation required. 

If tests pass - copy the same zip file as Content to the funding model specific test project, "RulebaseMasterFiles". E.g. src\FM36\ESFA.DC.ILR.FundingService.FM36.Service.Tests\RulebaseMasterFiles. 
This is used as the comparison and last known working copy for the unit tests on subsequent rulebase drops. 

Commit. 



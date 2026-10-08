# workload-management endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/workload-management-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `workloads/categories` | IWorkloadCategories.ListAsync | WorkloadCategoriesTests.ListAsync_SendsGet |
| POST | `workloads/categories/{name}` | IWorkloadCategories.UpdateAsync | WorkloadCategoriesTests.UpdateAsync_PostsTheWeightsToTheCategory |
| GET | `workloads/pools` | IWorkloadPools.ListAsync | WorkloadPoolsTests.ListAsync_SendsGet |
| POST | `workloads/pools` | IWorkloadPools.CreateAsync | WorkloadPoolsTests.CreateAsync_PostsEveryField |
| GET | `workloads/rules` | IWorkloadRules.ListAsync | WorkloadRulesTests.ListAsync_SendsTheRuleType |
| POST | `workloads/rules` | IWorkloadRules.CreateAsync | WorkloadRulesTests.CreateAsync_PostsEveryField |
| DELETE | `workloads/rules/{name}` | IWorkloadRules.DeleteAsync | WorkloadRulesTests.DeleteAsync_SendsDeleteForTheRule |
| POST | `workloads/config/enable` | IWorkloadConfig.EnableAsync | WorkloadConfigTests.EnableAsync_PostsWithNoBody |
| POST | `workloads/config/disable` | IWorkloadConfig.DisableAsync | WorkloadConfigTests.DisableAsync_PostsWithNoBody |
| GET | `workloads/config/get-base-dirname` | IWorkloadConfig.GetBaseDirectoryAsync | WorkloadConfigTests.GetBaseDirectoryAsync_SendsGet |
| GET | `workloads/config/preflight-checks` | IWorkloadConfig.GetPreflightChecksAsync | WorkloadConfigTests.GetPreflightChecksAsync_SendsGet |
| POST | `workloads/config/set-base-dirname` | IWorkloadConfig.SetBaseDirectoryAsync | WorkloadConfigTests.SetBaseDirectoryAsync_PostsTheName |
| GET | `workloads/policy/search_admission_control` | IWorkloadPolicy.GetSearchAdmissionControlAsync | WorkloadPolicyTests.GetSearchAdmissionControlAsync_SendsGet |
| POST | `workloads/policy/search_admission_control` | IWorkloadPolicy.UpdateSearchAdmissionControlAsync | WorkloadPolicyTests.UpdateSearchAdmissionControlAsync_PostsTheFlag |
| GET | `workloads/status` | IWorkloadStatus.GetAsync | WorkloadStatusTests.GetAsync_SendsAdvanced |

# knowledge endpoints

Source: https://help.splunk.com/en/splunk-enterprise/rest-api-reference/10.6/knowledge-endpoints

Paths are relative to `services/` (or `servicesNS/{owner}/{app}/`). One row per operation. `Client method` is `IInterface.Method`; `Test` is `TestClass.TestMethod`. Deprecated operations are marked and not implemented.

The reference lists DELETE for `data/transforms/metric-schema` and `saved/bookmarks/monitoring_console` on the collection path; its own examples and Splunk 10.6 need the entity name (a DELETE without one fails with "Cannot perform action DELETE without a target name to act on"), so those two rows name `{name}`.

| Method | Path | Client method | Test |
|---|---|---|---|
| GET | `admin/summarization` | `IDataModelSummaries.ListAsync` | `DataModelSummariesTests.ListAsync_SendsByTstats` |
| GET | `admin/summarization/tstats:DM_{app}_{data_model_ID}` | `IDataModelSummaries.GetAsync` | `DataModelSummariesTests.GetAsync_BuildsTheTstatsSummaryName` |
| GET | `data/lookup-table-files` | `ILookupTableFiles.ListAsync` | `LookupTableFilesTests.ListAsync_SendsGetToLookupTableFiles` |
| POST | `data/lookup-table-files` | `ILookupTableFiles.CreateAsync` | `LookupTableFilesTests.CreateAsync_PostsNameAndStagedPath_InTheNamespace` |
| GET | `data/lookup-table-files/{name}` | `ILookupTableFiles.GetAsync` | `LookupTableFilesTests.GetAsync_SendsGetToTheFile` |
| POST | `data/lookup-table-files/{name}` | `ILookupTableFiles.UpdateAsync` | `LookupTableFilesTests.UpdateAsync_PostsTheStagedPath` |
| DELETE | `data/lookup-table-files/{name}` | `ILookupTableFiles.DeleteAsync` | `LookupTableFilesTests.DeleteAsync_SendsDelete` |
| GET | `data/props/calcfields` | `ICalculatedFields.ListAsync` | `CalculatedFieldsTests.ListAsync_SendsGetToCalcfields` |
| POST | `data/props/calcfields` | `ICalculatedFields.CreateAsync` | `CalculatedFieldsTests.CreateAsync_PostsNameStanzaAndValue` |
| GET | `data/props/calcfields/{name}` | `ICalculatedFields.GetAsync` | `CalculatedFieldsTests.GetAsync_SendsGetToTheEscapedEntryName` |
| POST | `data/props/calcfields/{name}` | `ICalculatedFields.UpdateAsync` | `CalculatedFieldsTests.UpdateAsync_PostsTheValue` |
| DELETE | `data/props/calcfields/{name}` | `ICalculatedFields.DeleteAsync` | `CalculatedFieldsTests.DeleteAsync_SendsDelete` |
| GET | `data/props/extractions` | `IFieldExtractions.ListAsync` | `FieldExtractionsTests.ListAsync_SendsGetToExtractions` |
| POST | `data/props/extractions` | `IFieldExtractions.CreateAsync` | `FieldExtractionsTests.CreateAsync_PostsTheTypeByItsWireName` |
| GET | `data/props/extractions/{name}` | `IFieldExtractions.GetAsync` | `FieldExtractionsTests.GetAsync_SendsGetToTheEntry` |
| POST | `data/props/extractions/{name}` | `IFieldExtractions.UpdateAsync` | `FieldExtractionsTests.UpdateAsync_PostsTheValue` |
| DELETE | `data/props/extractions/{name}` | `IFieldExtractions.DeleteAsync` | `FieldExtractionsTests.DeleteAsync_SendsDelete` |
| GET | `data/props/fieldaliases` | `IFieldAliases.ListAsync` | `FieldAliasesTests.ListAsync_SendsGetToFieldaliases` |
| POST | `data/props/fieldaliases` | `IFieldAliases.CreateAsync` | `FieldAliasesTests.CreateAsync_PostsEachAliasAsAnAliasField` |
| GET | `data/props/fieldaliases/{name}` | `IFieldAliases.GetAsync` | `FieldAliasesTests.GetAsync_SendsGetToTheEntry` |
| POST | `data/props/fieldaliases/{name}` | `IFieldAliases.UpdateAsync` | `FieldAliasesTests.UpdateAsync_PostsTheAliases` |
| DELETE | `data/props/fieldaliases/{name}` | `IFieldAliases.DeleteAsync` | `FieldAliasesTests.DeleteAsync_SendsDelete` |
| GET | `data/props/lookups` | `IAutomaticLookups.ListAsync` | `AutomaticLookupsTests.ListAsync_SendsGetToLookups` |
| POST | `data/props/lookups` | `IAutomaticLookups.CreateAsync` | `AutomaticLookupsTests.CreateAsync_PostsTheSettingsAndFieldFamilies` |
| GET | `data/props/lookups/{name}` | `IAutomaticLookups.GetAsync` | `AutomaticLookupsTests.GetAsync_SendsGetToTheEntry` |
| POST | `data/props/lookups/{name}` | `IAutomaticLookups.UpdateAsync` | `AutomaticLookupsTests.UpdateAsync_PostsTheSettings` |
| DELETE | `data/props/lookups/{name}` | `IAutomaticLookups.DeleteAsync` | `AutomaticLookupsTests.DeleteAsync_SendsDelete` |
| GET | `data/props/sourcetype-rename` | `ISourcetypeRenames.ListAsync` | `SourcetypeRenamesTests.ListAsync_SendsGetToSourcetypeRename` |
| POST | `data/props/sourcetype-rename` | `ISourcetypeRenames.CreateAsync` | `SourcetypeRenamesTests.CreateAsync_PostsNameAndValue` |
| GET | `data/props/sourcetype-rename/{name}` | `ISourcetypeRenames.GetAsync` | `SourcetypeRenamesTests.GetAsync_SendsGetToTheEntry` |
| POST | `data/props/sourcetype-rename/{name}` | `ISourcetypeRenames.UpdateAsync` | `SourcetypeRenamesTests.UpdateAsync_PostsTheValue` |
| DELETE | `data/props/sourcetype-rename/{name}` | `ISourcetypeRenames.DeleteAsync` | `SourcetypeRenamesTests.DeleteAsync_SendsDelete` |
| GET | `data/transforms/extractions` | `IFieldTransforms.ListAsync` | `FieldTransformsTests.ListAsync_SendsGetToTransformsExtractions` |
| POST | `data/transforms/extractions` | `IFieldTransforms.CreateAsync` | `FieldTransformsTests.CreateAsync_PostsTheUppercaseSettings` |
| GET | `data/transforms/extractions/{name}` | `IFieldTransforms.GetAsync` | `FieldTransformsTests.GetAsync_SendsGetToTheEntry` |
| POST | `data/transforms/extractions/{name}` | `IFieldTransforms.UpdateAsync` | `FieldTransformsTests.UpdateAsync_PostsOnlyTheSettingsGiven` |
| DELETE | `data/transforms/extractions/{name}` | `IFieldTransforms.DeleteAsync` | `FieldTransformsTests.DeleteAsync_SendsDelete` |
| GET | `data/transforms/lookups` | `ILookupDefinitions.ListAsync` | `LookupDefinitionsTests.ListAsync_SendsGetSize` |
| POST | `data/transforms/lookups` | `ILookupDefinitions.CreateAsync` | `LookupDefinitionsTests.CreateAsync_PostsEverySetting` |
| GET | `data/transforms/lookups/{name}` | `ILookupDefinitions.GetAsync` | `LookupDefinitionsTests.GetAsync_SendsGetToTheEntry` |
| POST | `data/transforms/lookups/{name}` | `ILookupDefinitions.UpdateAsync` | `LookupDefinitionsTests.UpdateAsync_PostsTheSettings` |
| DELETE | `data/transforms/lookups/{name}` | `ILookupDefinitions.DeleteAsync` | `LookupDefinitionsTests.DeleteAsync_SendsDelete` |
| GET | `data/transforms/metric-schema` | `IMetricSchemas.ListAsync` | `MetricSchemasTests.ListAsync_SendsGetToMetricSchema` |
| POST | `data/transforms/metric-schema` | `IMetricSchemas.CreateAsync` | `MetricSchemasTests.CreateAsync_PostsThePluralFieldNames` |
| DELETE | `data/transforms/metric-schema/{name}` | `IMetricSchemas.DeleteAsync` | `MetricSchemasTests.DeleteAsync_SendsDeleteToTheSchema` |
| POST | `data/transforms/statsdextractions` | `IStatsdExtractions.CreateAsync` | `StatsdExtractionsTests.CreateAsync_PostsNameRegexAndFlag` |
| GET | `data/ui/global-banner` | `IGlobalBanners.ListAsync` | `GlobalBannersTests.ListAsync_SendsGetToGlobalBanner` |
| POST | `data/ui/global-banner` | `IGlobalBanners.CreateAsync` | `GlobalBannersTests.CreateAsync_PostsThePrefixedFields` |
| GET | `data/ui/panels` | `IPanels.ListAsync` | `PanelsTests.ListAsync_SendsGetInTheNamespace` |
| POST | `data/ui/panels` | `IPanels.CreateAsync` | `PanelsTests.CreateAsync_PostsNameAndXml` |
| GET | `data/ui/views` | `IViews.ListAsync` | `ViewsTests.ListAsync_SendsGetWithTheFilter` |
| POST | `data/ui/views` | `IViews.CreateAsync` | `ViewsTests.CreateAsync_PostsNameAndSource` |
| POST | `data/ui/views/{dashboard_id}/disable` | `IViews.DisableAsync` | `ViewsTests.DisableAsync_PostsToDisable` |
| POST | `data/ui/views/{dashboard_id}/enable` | `IViews.EnableAsync` | `ViewsTests.EnableAsync_PostsToEnable` |
| GET | `data/ui/views/{name}` | `IViews.GetAsync` | `ViewsTests.GetAsync_SendsGetToTheView` |
| POST | `data/ui/views/{name}` | `IViews.UpdateAsync` | `ViewsTests.UpdateAsync_PostsSourceAndChangeLog` |
| DELETE | `data/ui/views/{name}` | `IViews.DeleteAsync` | `ViewsTests.DeleteAsync_SendsDeleteWithoutBody`, `ViewsTests.DeleteAsync_WithChangeLog_SendsItAsAFormBody` |
| GET | `data/ui/views/{name}/history` | `IViews.ListHistoryAsync`, `IViews.ListHistoryWithMessagesAsync` | `ViewsTests.ListHistoryAsync_SendsGetToHistory`, `ViewsTests.ListHistoryWithMessagesAsync_SendsTheEmptyFlag` |
| GET | `data/ui/views/{name}/revision` | `IViews.GetRevisionAsync` | `ViewsTests.GetRevisionAsync_SendsTheRevisionIdInTheQuery` |
| GET | `datamodel/acceleration (deprecated)` |  |  |
| GET | `datamodel/acceleration/{name} (deprecated)` |  |  |
| GET | `datamodel/model` | `IDataModels.ListAsync` | `DataModelsTests.ListAsync_SendsConcise` |
| POST | `datamodel/model` | `IDataModels.CreateAsync` | `DataModelsTests.CreateAsync_PostsNameDefinitionAndAcceleration` |
| GET | `datamodel/model/{name}` | `IDataModels.GetAsync` | `DataModelsTests.GetAsync_SendsGetWithConcise` |
| POST | `datamodel/model/{name}` | `IDataModels.UpdateAsync` | `DataModelsTests.UpdateAsync_PostsProvisional` |
| DELETE | `datamodel/model/{name}` | `IDataModels.DeleteAsync` | `DataModelsTests.DeleteAsync_SendsDelete` |
| GET | `datamodel/pivot/{name}` | `IDataModels.GetPivotAsync` | `DataModelsTests.GetPivotAsync_SendsThePivotSearch`, `DataModelsTests.GetPivotAsync_SendsThePivotJson` |
| GET | `directory` | `IDirectoryEntries.ListAsync` | `DirectoryEntriesTests.ListAsync_SendsGetToDirectory` |
| GET | `directory/{name}` | `IDirectoryEntries.GetAsync` | `DirectoryEntriesTests.GetAsync_SendsGetToTheEntry` |
| GET | `saved/bookmarks/monitoring_console` | `IMonitoringConsoleBookmarks.ListAsync` | `MonitoringConsoleBookmarksTests.ListAsync_SendsSorting` |
| POST | `saved/bookmarks/monitoring_console` | `IMonitoringConsoleBookmarks.CreateAsync` | `MonitoringConsoleBookmarksTests.CreateAsync_PostsNameAndUrl` |
| DELETE | `saved/bookmarks/monitoring_console/{name}` | `IMonitoringConsoleBookmarks.DeleteAsync` | `MonitoringConsoleBookmarksTests.DeleteAsync_SendsDeleteToTheBookmark` |
| GET | `saved/eventtypes` | `IEventTypes.ListAsync` | `EventTypesTests.ListAsync_SendsGetToEventtypes` |
| POST | `saved/eventtypes` | `IEventTypes.CreateAsync` | `EventTypesTests.CreateAsync_PostsEverySetting` |
| GET | `saved/eventtypes/{name}` | `IEventTypes.GetAsync` | `EventTypesTests.GetAsync_SendsGetToTheEscapedName` |
| POST | `saved/eventtypes/{name}` | `IEventTypes.UpdateAsync` | `EventTypesTests.UpdateAsync_PostsTheSearchAgain` |
| DELETE | `saved/eventtypes/{name}` | `IEventTypes.DeleteAsync` | `EventTypesTests.DeleteAsync_SendsDelete` |
| GET | `search/fields` | `ISearchFields.ListAsync` | `SearchFieldsTests.ListAsync_SendsGetToFields` |
| GET | `search/fields/{field_name}` | `ISearchFields.GetAsync` | `SearchFieldsTests.GetAsync_SendsGetToTheField` |
| GET | `search/fields/{field_name}/tags` | `ISearchFields.ListTagsAsync` | `SearchFieldsTests.ListTagsAsync_SendsGetToTheFieldsTags` |
| POST | `search/fields/{field_name}/tags` | `ISearchFields.UpdateTagsAsync` | `SearchFieldsTests.UpdateTagsAsync_PostsValueAndRepeatedTags` |
| GET | `search/tags` | `ISearchTags.ListAsync` | `SearchTagsTests.ListAsync_SendsGetToTags` |
| GET | `search/tags/{tag_name}` | `ISearchTags.GetAsync` | `SearchTagsTests.GetAsync_SendsGetToTheTag` |
| POST | `search/tags/{tag_name}` | `ISearchTags.UpdateAsync` | `SearchTagsTests.UpdateAsync_PostsRepeatedPairs` |
| DELETE | `search/tags/{tag_name}` | `ISearchTags.DeleteAsync` | `SearchTagsTests.DeleteAsync_SendsDelete` |

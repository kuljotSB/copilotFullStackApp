@echo off

rem set values for your search service
set url="YOUR_SEARCH_SERVICE_URL"
set admin_key="YOUR_SEARCH_SERVICE_ADMIN_KEY"

echo -----
echo Creating the data source
call curl -X POST %url%/datasources?api-version=2020-06-30 -H "Content-Type: application/json" -H "api-key: %admin_key%" -d @data_source.json

echo -----
echo creating the skillset
call curl -X PUT %url%/skillsets/margies-custom-skillset?api-version=2020-06-30 -H "Content-Type: application/json" -H "api-key: %admin_key%" -d @skillset.json

echo -----
echo creating the index
call curl -X PUT %url%/indexes/margies-custom-index?api-version=2020-06-30 -H "Content-Type: application/json" -H "api-key: %admin_key%" -d @index.json

rem wait
timeout /t 5 /nobreak

echo -----
echo creating the indexer
call curl -X PUT %url%/indexers/margies-custom-indexer?api-version=2020-06-30 -H "Content-Type: application/json" -H "api-key: %admin_key%" -d @indexer.json
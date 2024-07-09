Write-Host "starting azure deployment..."
#setting all the environment variables

$location = "eastus"
$suffix = [System.Guid]::NewGuid().ToString().Substring(0, 8) #generate an 8-character random suffix
$grp = "copilotrg${suffix}"
$storageAccountName = "copilotstr${suffix}"
$AIMultiServiceAccountName = "copilotaisvc${suffix}"
$openAIResourceName = "copilotopenai${suffix}"
$searchResourceName = "copilotsearch${suffix}"
$cosmosdbAccountName = "copilotcosmosdb${suffix}"


Write-Host "Creating resource group..."
az group create --name $grp --location $location

Write-Host "Creating storage account...."
az storage account create --name $storageAccountName --resource-group $grp --sku Standard_LRS --kind StorageV2 --location $location

Write-Host "Creating ai multi-service account"
az cognitiveservices account create --name $AIMultiServiceAccountName --resource-group $grp --kind CognitiveServices --sku S0 --location $location --yes

Write-Host "Creating OpenAI resource.... "
az cognitiveservices account create --name $openAIResourceName --resource-group $grp --location swedencentral --kind OpenAI --sku s0 

Write-Host "Creating search resource ....."
az search service create --name $searchResourceName --resource-group $grp --location $location --sku basic

Write-Host "Creating cosmosdb account for nosql....."
az cosmosdb create --name $cosmosdbAccountName --resource-group $grp --kind GlobalDocumentDB --capabilities EnableServerless --locations regionName="Sweden Central"


# Custom Copilot Full Stack App

## Prerequisites 
1) download `VSCode` from the following URL - https://code.visualstudio.com/download
2) download `GIT` from the following URL - https://git-scm.com/downloads
3) download `Node.js` from the following URL - https://nodejs.org/en
5) download `.NET 8` from the following URL - https://dotnet.microsoft.com/en-us/download/dotnet/8.0
6) Install `Azure CLI` from the following URL - https://learn.microsoft.com/en-us/cli/azure/install-azure-cli
7) Install `Azure Functions Core Tool` from the following URL - https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows%2Cisolated-process%2Cnode-v4%2Cpython-v2%2Chttp-trigger%2Ccontainer-apps&pivots=programming-language-csharp

## Grass Root Concept For This App
![Alt text](https://github.com/kuljotSB/assets/blob/main/copilot%20stack.png?raw=true)

This full stack web application is built to provide a working proof of concept of the `Copilot Stack` that talks about how you can, as a developer, build your own custom copilot experience just like Microsoft Copilot. 

The web application is built with `React` as the frontend nd the backend is written in `C#` programming langauge. The web application integrates `Semantic Kernel` with a lot of other Azure Cloud-based offerings such as Azure functions, Cosmos DB for NoSQL, Azure AI Search for RAG (Retrieval Augmented Generation), Microsoft Grpah Plugin, Web Searcher Plugin and a bunch of other Prompt Template plugins and Native Plugins to build a completely automated AI agent for your organisation using Azure OpenAI (built on principles of enterprise-level security). 

### The Copilot Stack
The copilot stack was first introduced by Satya Nadella in the Microsoft Build conference 2024, allowiing developers to build their own version of completely automated GenAI agents. The copilot stack is built on the principle of extensibility, extensibility of GenAI with other Azure Cloud-based services/offerings. The main heart of the copilot stack that integrates your GenAI models with other underlying core Azure services is the AI orchestration layer or the `Semantic Kernel`. This web application is built majorly using the Semantic Kernel SDK for C#, although a python SDK is also available.

### Semantic Kernel - AI orchestration layer
![Alt text](https://github.com/kuljotSB/assets/blob/main/enterprise-ready.png?raw=true)

Microsoft and other Fortune 500 companies are already leveraging Semantic Kernel because it’s flexible, modular, and observable. Backed with security enhancing capabilities like telemetry support, and hooks and filters so you’ll feel confident you’re delivering responsible AI solutions at scale. 

Semantic Kernel combines prompts with existing APIs to perform actions. By describing your existing code to AI models, they’ll be called to address requests. When a request is made the model calls a function, and Semantic Kernel is the middleware translating the model's request to a function call and passes the results back to the model.

## Web App Architecture 
![Alt text](https://github.com/kuljotSB/assets/blob/main/copilot_app_architecture.png?raw=true)

The overall architecture makes use of the following services:
1) `React.js` for frontend
2) `dotnet core Web API` for backend (written in C#).
3) `Semantic Kernel SDK for C#` for acting as the AI orchestration layer.
4) `Azure OpenAI` for LLM models.
5) `Azure Storage Account` for blob storage (making up for the datastore of Azure AI Search) and for queue service (Azure Storage Queue).
6) `Bing Search` service for making the Bing web Searcher Plugin
7) `Microsoft Graph APIs` for making the Graph Plugin.
8) `Azure AI Search` for RAG (Retrieval Augmented Generation). Extracting business and organizational data proprietary to a specific organization.
9) `Azure Cosmos DB for NoSQL` for making up as the persisted, global database for auditing purposes, chat history and making the app real-time.
10) `Azure Function` which is built on a queue based trigger and writes chat history data from Azure Storage Queue to the Cosmos DB database.

# Getting Started With Running The Web App

### Running The Front-End

1) open the `COPILOTFULLSTACKAPP` folder in command prompt and enter the following commands:

    ` cd frontend`
   
    ` cd frontend`
   
3) now install the npm modules with this command:


   ` npm install `
   
5) now run these commands:

   `cd src`
   
   ` npm run dev`

The Front-end uses vite to run on a local server http://localhost:5173

![Alt text](https://github.com/kuljotSB/assets/blob/main/Screenshot%202024-07-10%20142243.png?raw=true)

### Running The Backend

#### Deploying Infrastructure on Azure

1) make sure you have Azure CLI installed on your device through the following link (https://learn.microsoft.com/en-us/cli/azure/install-azure-cli).
2) open the `COPILOTFULLSTACKAPP` folder in powershell and enter the following commands:

    `cd infrastructure setup`

3) now you need to login to your azure account using the CLI:

   `az login`

4) now run the powershell script to deploy the infra on Azure.

   `./deploy.ps1`

#### Creating An Azure AI Search Solution

1) navigate to `backend/AISearch`
   
2) go to the `upload-docs.cmd` file and fill in the Subscription_id, azure_storage_account and azure_storage_key variables with appropriate values and run the following command in powershell to upload documents contained in the`AISearch/data` folder to your storage account's container.

   `./upload-docs.cmd`

3) now naviagate to `AISearch/data_source.json` and fill in the appropriate value of `connectionString` with your storage account's connection string.
   
4) navigate to `AISearch/skillset.json` and fill in the appropriate value of `key` with the key of your multi-service AI account.
   
5) navigate to `AISearch/create-search.cmd` and fill in the appropriate values of `url` and `admin_key` and run the following command in powershell to create a complete AI Search solution for RAG purpose.

   `./create-search.cmd`


#### Creating An App Registration For Microsoft Graph Plugin

1) Navigate to your Azure account and create an app registration with the following configurations:

   a) The redirect URI needs to be kept blank and the name could be anything for the app registration.

   b) The API permissions need to be assigned in the following manner.

     ![Alt text](https://github.com/kuljotSB/assets/blob/main/app_registration.png?raw=true)


#### Running The Whole Backend Locally

1) Navigate to the "backend/appsetting.json" file and fill in the required values appropriately.

2) Open The `COPILOTFULLSTACKAPP` folder in command prompt and enter the following commands:

    `cd backend`

    `dotnet build` 

    `dotnet run`

The Backend will be up and running locally on http://localhost:5124



### Running Azure Function Locally

1) Download The `Azure Functions Core Tools` from this link (https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows%2Cisolated-process%2Cnode-v4%2Cpython-v2%2Chttp-trigger%2Ccontainer-apps&pivots=programming-language-csharp) to locally test and run your Azure Function.

2) navigate to `azureFunctions/StorageQueueFunction/local.settings.json` and fill in the value of `AzureWebJobsStorage` with the value of connection string of your storage account

3) navigate to `azureFunctions/StorageQueueFunction/Data/Secrets.cs` and fill in the value of `storageConnectionString` and `cosmosDBConnectionString` with appropriate values.

4) Open The `COPILOTFULLSTACKAPP` folder in command prompt and enter the following commands:

    `cd azureFunctions`

    `cd StorageQueueFunction`

    `func start`


   `

   







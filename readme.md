# Custom Copilot Full Stack App

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
![Alt text](https://github.com/kuljotSB/assets/blob/main/Screenshot%202024-07-10%20135848.png?raw=true)

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
2) now install the npm modules with this command:
   ` npm install `
3) now run these commands:
   `cd src`
   ` npm run dev`

So the frontend uses `vite` to run locally on a localhost server and the url of this localhost server is: `http://localhost:5173`

![Alt text](https://github.com/kuljotSB/assets/blob/main/Screenshot%202024-07-10%20142243.png?raw=true)


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

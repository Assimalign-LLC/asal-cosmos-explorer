![Alt text](/images/cosmos-db-icon-1.png) 

# Assimalign Cosmos Explorer
Traverse the Cosmos and all the data it has to offer with an out of the box .NET Core library build on Microsoft Cosmos DB SDK for standard API querying. 

---
## Why use this Library?
1. **Out Of The Box Querying**: The library was designed as an extensible Web API Query interface for Cosmos DB, meaning a ready to go query language that is converted from supported LINQ-To-SQL apis.

2. **Extensibility**: 

3. **Fine Grain Security**: Implement field level security by defining roles & policies and associate them to your Cosmos C# Model 
---

# Getting Started
> Clone the repository and get started running API queries locally via Azure Functions and Azure Cosmos DB Emulator. See the simplicity of creating 

## Requirements
Before getting started the following requirements listed below will be needed
- Visual Studio Code or Visual Studios 2019 or above (2022 is coming!) 
- Azure Functions Core Tools
- Postman


# Important Notes: 

## Unsupported Functionality
Due to some limitations of Microsoft Cosmos DB SDK and universal API standards, some functionality was either left out or could not be added.
### 1. Group By Not Supported
This functionality was left out purposefully
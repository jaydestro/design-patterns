---
page_type: sample
languages:
- csharp
products:
- azure-cosmos-db
name: |
  Azure Cosmos DB design pattern: Attribute Array counter
urlFragment: attribute-array
description: Review this example of using attribute array to count inventory and hotel room currency prices.
---

# Azure Cosmos DB design pattern: Attribute Array

The attribute pattern can be defined as a JSON object consisting of multiple similar properties or fields that should be grouped together in a collection for better indexing and sorting.

The main advantage to this pattern is that rather creating multiple indexes for every property/field, we can now focus on one particular path to index.  And in the case where you have to add another property/field later, it can be easily added to the collection versus a data model change and the adding of a new index to support it.

> This sample demonstrates:
>
> - ✅ Creation of several objects based on attribute and non-attribute data patterns.
> - ✅ Querying both attribute and non-attribute objects in a container.
>
## Common scenario

A common scenario for using a NoSQL attribute array design pattern is when you have entities with a large number of attributes that can vary in number and type. This pattern is particularly useful when the attributes of an entity are not well-defined or fixed, and you want to avoid schema changes or migrations.

Let's say you're developing an e-commerce platform where sellers can list their products. Each product can have various attributes such as size, color, brand, price, description, and more. However, different sellers may have different attributes for their products. Some sellers might want to include additional attributes like weight, material, or warranty.

In a relational database, you would typically create a table with predefined columns for each attribute. However, in this case, the number and type of attributes can vary greatly between different products and sellers, making it difficult to define a fixed schema.

Using a NoSQL database with an attribute array design pattern, you can store the product attributes as a flexible array or document within the product entity. Each attribute can be represented as a key-value pair, where the key is the attribute name (e.g., "color") and the value is the corresponding attribute value (e.g., "red").

With this design pattern, you can easily accommodate the varying attributes of different products and sellers. Sellers can add or remove attributes as needed without requiring schema modifications or complex migrations. Queries can be performed on specific attribute values, and you can even index certain attributes for efficient searching.

Overall, the NoSQL attribute array design pattern is suitable when you have entities with dynamic and variable attributes, allowing for flexibility, scalability, and easy adaptability to changing requirements.

## Sample implementation
### Products with sizes

Products like shirts and sweaters tend to have multiple sizes that may be in inventory. Based on the size, you could design your model to look like the following with each size count as a property/field:

```csharp
AttributeBasedProduct m1 = new AttributeBasedProduct();
m1.Id = "product_1";
m1.ProductId = m1.Id;
m1.Size_Small = 100;
m1.Size_Medium = 50;
m1.Size_Large = 75;
```

The object json saved to Comsos DB would look like the following:

```json
{
    "Size_Small": 100,
    "Size_Medium": 50,
    "Size_Large": 75,
    "id": "product_1",
    "productId": "product_1",
    "Title": null
}
```

A non-attribute based approach would create a list property where the sizes are in a collection:

```csharp
NonAttributeBasedProduct m2 = new NonAttributeBasedProduct();
m2.Id = "product_2";
m2.ProductId = m2.Id;
m2.Sizes.Add(new Size { Name = "Small", Count = 100 });
m2.Sizes.Add(new Size { Name = "Medium", Count = 50 });
m2.Sizes.Add(new Size { Name = "Large", Count = 75 });
```

The object json saved to Comsos DB would look like the following:

```json
{
    "Sizes": [
        {
            "Name": "Small",
            "Count": 100
        },
        {
            "Name": "Medium",
            "Count": 50
        },
        {
            "Name": "Large",
            "Count": 75
        }
    ],
    "id": "product_2",
    "productId": "product_2",
    "Title": null
}
```

### Rooms with different prices

Another example could utilize hotel rooms with different prices based on currency.  In the following example the `Price_EUR` and `Price_USD` are properties that hold similar information.

```csharp
//attribute based price
List<Room> rooms = new List<Room>();
rooms.Add(new RoomAttibuteBased { Id = "1", Price_EUR = 1000, Price_USD = 1000, Size_SquareFeet = 1000, Size_Meters = 50 });
rooms.Add(new RoomAttibuteBased { Id = "2", Price_EUR = 1000, Price_USD = 1000, Size_SquareFeet = 2000, Size_Meters = 100 });
```

The object json saved to Comsos DB would look like the following.  Notice the Attibute for each room's price for the currency:

```json
{
    "Price_USD": 1000,
    "Price_EUR": 1000,
    "Size_Meters": 50,
    "Size_SquareFeet": 1000,
    "EntityType": "room",
    "hotelId": "hotel_1",
    "Name": null,
    "Type": null,
    "Status": null,
    "NoBeds": 0,
    "SizeInSqFt": 0,
    "Price": 0,
    "Available": false,
    "Description": null,
    "MaximumGuests": 0
}
```

The alternative would be to again create a collection of prices:

```csharp
//non-attribute based
List<Room> rooms = new List<Room>();
RoomNonAttibuteBased r = new RoomNonAttibuteBased();
r.RoomPrices.Add(new RoomPrice { Currency = "USD", Price = 1000 });
r.RoomPrices.Add(new RoomPrice { Currency = "EUR", Price = 1000 });
rooms.Add(r);
```

The object json saved to Comsos DB would look like the following, notice how the prices are now part of a collection called `RoomPrices`:

```json
{
    "RoomPrices": [
        {
            "Currency": "EUR",
            "Price": 1000
        },
        {
            "Currency": "USD",
            "Price": 1000
        }
    ],
    "RoomSizes": [],
    "EntityType": "room",
    "hotelId": "hotel_1",
    "Name": null,
    "Type": null,
    "Status": null,
    "NoBeds": 0,
    "SizeInSqFt": 0,
    "Price": 0,
    "Available": false,
    "Description": null,
    "MaximumGuests": 0
}
```
## Try this implementation

You can try out this implementation by running the code in [GitHub Codespaces](https://docs.github.com/codespaces/overview) with a [free Azure Cosmos DB account](https://learn.microsoft.com/azure/cosmos-db/try-free). (*This option doesn't require an Azure subscription, just a GitHub account.*)

1. Create a free Azure Cosmos DB for NoSQL account: (<https://cosmos.azure.com/try>)

1. Open the new account in the Azure portal and record the **URI** and **PRIMARY KEY** fields. These fields can be found in the **Keys** section of the account's page within the portal.

## Create Database and Containers
1. In the Data Explorer, create a new **CosmosPatterns** database and a **Hotels** container with the following values:

    | | Value |
    | --- | --- |
    | **Database name** | `CosmosPatterns` |
    | **Container name** | `Hotels` |
    | **Partition key path** | `/hotelId` |
    | **Throughput** | `400` (*Manual*) |

1. Create another new container using the **CosmosPatters** database for the **Products** container with the following values:

    | | Value |
    | --- | --- |
    | **Database name** | `CosmosPatterns` |
    | **Container name** | `Products` |
    | **Partition key path** | `/productId` |
    | **Throughput** | `400` (*Manual*) |

1. Open the application code in a GitHub Codespace:

    [![Illustration of a button with the GitHub icon and the text "Open in GitHub Codespaces."](../media/open-github-codespace-button.svg)](https://github.com/codespaces/new?hide_repo_select=true&ref=main&repo=613998360)

## Set up environment variables

1. Once the template deployment is complete, select **Go to resource group**.
1. Select the new Azure Cosmos DB for NoSQL account.
1. From the navigation, under **Settings**, select **Keys**. The values you need for the environment variables for the demo are here.

1. Create 2 environment variables to run the demos:

    - `COSMOS_ENDPOINT`: set to the `URI` value on the Azure Cosmos DB account Keys blade.
    - `COSMOS_KEY`: set to the Read-Write `PRIMARY KEY` for the Azure Cosmos DB for NoSQL account

1. Open a terminal in your GitHub Codespace and create your Bash variables with the following syntax:

    ```bash
    export COSMOS_ENDPOINT="YOUR_COSMOS_ENDPOINT"
    export COSMOS_KEY="YOUR_COSMOS_KEY"
    ```

## Run the demo

1. From your GitHub Codespace terminal, start the app by running the following:

    ```bash
    dotnet build
    dotnet run
    ```
1. Once complete, the progam will create several objects based on attribute and non-attribute.  Reference the [README.md](README.md) file for the queries you can run against the Cosmos DB to see the differences.

## Example queries (Products)

1. In Azure Portal, browse to your Cosmos DB resource.
1. Select **Data Explorer** in the left menu.
1. Select the `Products` container, then choose **New SQL Query**.

The following queries would be needed to query attribue based products for an available size:

```sql
select value c from c where c.Size_Small >= 100 or c.Size_Medium >= 100 or c.Size_Large >= 100
```

The following query can be used to query non-attribute based products for an available size:

```sql
select value c from c JOIN r IN c.Sizes where r.Count >= 100
```

## Example queries (Hotel Rooms)

1. Select the `Hotels` container, then choose **New SQL Query**.
1. The following queries would be needed to query attribue based rooms for a price:

```sql
select * from c where c.Price_USD >= 1000
```

The following query can be used to query non-attribute based rooms for a price:

```sql
select * from c JOIN rp in c.RoomPrices where rp.Currency = 'USD' and rp.Price >= 1000
```

## Summary

By converting similar properties\fields to collections you can improve many aspects of your data model and the queries that run against them.  You can also reduce and simplify the indexing settings on a container and make queries easier to write and also execute.

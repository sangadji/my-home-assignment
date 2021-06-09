# My Home Assignment
This repository contains proof-of-concept of a warehouse software. The warehouse have at least the following functionality;
* Get all products and quantity of each that is an available with the current inventory
* Remove(Sell) a product and update the inventory accordingly

## Framework used
Framework: ASP.NET Core 5.0

Language: C#

## Getting started
To try this app you can either run it locally or run it remotely by accessing [this link](https://web-homeassignment-sangadji.azurewebsites.net/) on a web browser. The easiest way, of course, to
run it remotely on the web browser. I have deployed it on a web server so you can access it conveniently. Here are the steps that you need to follow:
1. Go to the homepage of the web app.
2. Upload inventory.json and products.json through the provided file upload slots on the home page. This will populate the database.
3. Go to the product page by selecting "product" on the navigation pane. You'll see the list of available products along with its quantity on this page.
4. Select "details" on a particular product to simulate a "selling" use-case.
5. Click "sell" button on the product details page.
6. Enter the number item that you'd like to sell.
7. If the product is sold successfuly you will be redirected to the product listing page with the number of quantities updated.
8. Go to the inventory page by selecting "inventory" on the navigation pane to see the updated stock quantity.

## Solution structure
This solution is applying N-tier architecture to separate code concerns into several layers. Since it is a small codebase I've decided not to put the layers on a different projects(.csproj)/class libraries but instead put them on a single project but within different folders (whilst also applying different namespace naming). Here is the list:
- Data - contains everything related to data including entities, data models and repository classes. 
  - location /HomeAssignment.Web/Data 
- Repository - contains classes that are responsible for retrieving and storing data to the database. 
  - location /HomeAssignment.Web/Data/Repository
- Services - contains the domain service as well as the main business logic of the application.
  - location: /HomeAssignment.Web/Services
- Controllers - contains the ASPNET Core MVC Controllers to handle the incoming requests and responses.
  - location : /HomeAssignment.Web/Controllers
- Views - contains the presentation layer of the application
  - location: /HomeAssignment.Web/Views

## Q&A
If you have any question or feedback feel free to contact me at sangadji.prabowo@outlook.com!

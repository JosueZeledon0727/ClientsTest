# Project Overview

This project is divided into two microservices, built using **.Net Core Web API**, each in different .sln solutions:

- **CuentasService**: Manages the endpoints for Cuentas/Movimientos.
- **ClientsService**: Manages the endpoints for Clientes.

Both projects need to be run simultaneously for optimal functionality.

A database script named `BaseDatos.sql` is included to create the test database `testbd`, which will be used by both microservices.

Additionally, images demonstrating the functionality of the endpoints through SwaggerUI, as well as the Integration and Unit testing used within the `ClientsService` microservice, are included.

## Database diagram:
![Diagram_Clients](https://github.com/user-attachments/assets/e6ce4cf7-1e96-4627-a4a6-c01b98f279fb)

## Notes: 
1. The ClientService and part of CuentasService uses *Repository* pattern to access the data layer and the way of access it to be apart of the endpoint core functionality.
2. The error management could be improved by using a *Middleware* or *GeneralErrorHandling* class, but due to lack of time this could no be possible.



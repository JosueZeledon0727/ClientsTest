# Project Overview

This project is divided into two microservices, each in different .sln solutions:

- **CuentasService**: Manages the endpoints for Cuentas/Movimientos.
- **ClientsService**: Manages the endpoints for Clientes.

Both projects need to be run simultaneously for optimal functionality.

A database script named `BaseDatos.sql` is included to create the test database `testbd`, which will be used by both microservices.

Additionally, images demonstrating the functionality of the endpoints through SwaggerUI, as well as the Integration and Unit testing used within the `ClientsService` microservice, are included.

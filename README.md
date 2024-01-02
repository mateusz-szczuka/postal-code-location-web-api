# web-api

## 1. Project description:

### PL
Projekt zawiera implementację aplikacji Web API w oparciu o technologię .NET, która:
a) udostępnia endpoint do obliczania średniej współrzędnej geograficznej dla losowych kodów pocztowych w Polsce. Dodatkowo, endpoint ten zwraca liczbę użytych kodów w procesie generowania średniej.
b) integruje się z API opisanym w dokumentacji dostępnej pod tym adresem: https://www.zippopotam.us
 
### EN
The project includes a .NET-based implementation of a Web API application that:
a) provides an endpoint for calculating the average geographic coordinate for random postal codes in Poland. In addition, this endpoint returns the number of codes used in the process of generating the average.
b) integrates with the API described in the documentation available at this address: https://www.zippopotam.us

## 2. Project schema:

- WebApi.Api - Project startup, configuration, endpoints
- WebApi.Core - Business logic
- WebApi.DTO - Models/messages

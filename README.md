﻿# Rezervo

Rezervo is a Web API designed for efficiently managing appointment scheduling with specialists. It enables users to book appointments with specialists across various fields.

# Built with

- ASP.NET Core
- Entity Framework
- SQLServer
- XUnit
- Mvc.Testing
- Mediatr
- JWT
- Visual Studio 2022
- Minimal API

# API Documentation

| **Method** | **Endpoint**                     | **Parameters** | **Data**                                                   | **Description**                                      |
| ---------- | -------------------------------- | -------------- | ---------------------------------------------------------- | ---------------------------------------------------- |
| POST       | /users/register                  | -              | email, username, firstName, lastName, password, role       | Register a new user                                  |
| POST       | /users/login                     | -              | login, password                                            | Log in an existing user                              |
| POST       | /specializations                 | -              | name                                                       | Add a new specialization                             |
| DELETE     | /specializations/{id}            | (Integer) id   | -                                                          | Delete an existing specialization                    |
| GET        | /specializations                 | -              | -                                                          | Get a list of existing specializations               |
| POST       | /specialists                     | -              | userId, specializationName, phoneNumber, description, city | Add a new specialist                                 |
| DELETE     | /specialists{id}                 | (Integer) id   | -                                                          | Delete an existing specialist                        |
| PUT        | /specialists/{id}                | (Integer) id,  | userId, phoneNumber, description, city                     | Edit an existing specialist                          |
| GET        | /specialists                     | -              | -                                                          | Get a list of existing specialists                   |
| GET        | /specialists/{id}                | (Integer) id   | -                                                          | Get an existing specialist                           |
| GET        | /specialists/specialization/{id} | (Integer) id   | -                                                          | Get a list of existing specialists by specialization |
| POST       | /slots                           | (Integer) id   | scheduleId, startTime                                      | Add a new slot to schedule                           |
| DELETE     | /slots/{id}                      | (Integer) id   | -                                                          | Delete an existing slot                              |
| PUT        | /slots/{id}                      | (Integer) id   | startTime                                                  | Edit an existing slot                                |
| POST       | /schedules                       | -              | specialistId, startTime, endTime, slotDuration, date       | Add a new schedule                                   |
| DELETE     | /schedules/{id}                  | (Integer) id   | -                                                          | Delete an existing schedule                          |
| PUT        | /schedules/{id}                  | (Integer) id   | startTime, endTime                                         | Edit an existing schedule                            |
| GET        | /schedules                       | -              | specialistId                                               | Get a specialist's schedule                          |
| GET        | /schedules/{id}                  | (Integer) id   | -                                                          | Get slots for a specific schedule                    |
| POST       | /bookings                        | -              | userId, slotId                                             | Create a new booking for a specific slot             |
| DELETE     | /bookings/{id}                   | (Integer) id   | -                                                          | Delete an existing booking                           |
| GET        | /bookings                        |                | -                                                          | Get bookings for the current user                    |
| GET        | /bookings/{id}                   | (Integer) id   | -                                                          | Get an existing booking                              |

Nebula
======

[![Build status](https://ci.appveyor.com/api/projects/status/s98el7ttk7isnays/branch/master?svg=true)](https://ci.appveyor.com/project/inkadnb/nebula/branch/master) [![Coverage Status](https://coveralls.io/repos/inkadnb/Nebula/badge.svg?branch=master&service=github)](https://coveralls.io/github/inkadnb/Nebula?branch=master)

Nebula is a simple framework for Service Oriented Architecture in .NET.

This project is an exercise in implementing my own SOA from the experience I've gained from writing enterprise software.

Releases
========
Releases are available via [NuGet](https://www.nuget.org/packages/Nebula.Core).
To install Nebula, run the following command in the Package Manager Console
```
PM> Install-Package Nebula.Core
```
## Features and Roadmap
- [x] [Remote Procedure Calls over HTTP utilizing JSON.](https://github.com/inkadnb/Nebula/blob/master/Nebula.Core.Tests/RemoteProcedureCallTests.cs#L-19-34)
- [x] [Catch Remote Service exceptions.](https://github.com/inkadnb/Nebula/blob/master/Nebula.Core.Tests/RemoteProcedureCallTests.cs#L-168-179)
- [x] Change tracking.
- [x] [Support for Asynchronous Programming.](https://github.com/inkadnb/Nebula/blob/master/Nebula.Core.Tests/RemoteProcedureCallTests.cs#L139-152)
- [x] [Support for Remote Procedure Calls with interfaces as parameters and/or return type](https://github.com/inkadnb/Nebula/blob/master/Nebula.Core.Tests/RemoteProcedureCallTests.cs#L182-196).
- [ ] Data synchronization between client and services.

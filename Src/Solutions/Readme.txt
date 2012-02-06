==========================================================
- use MockServices or Debug configuration to start up framework
- use SingleDispatcher configuration to run application on one thread
- use Release configuration to test WCF connection (server needs to be running)
==========================================================
- add a new Module and set Build output path to /Modules/
- add a new Service and set Build output path to /Services/
- add a new Framework project and set Build output path to /Bin/
- add a new Shell and set Build output to /Bin/
- Write a Shell bootstrapper to load modules and services into the Shell
==========================================================
- Services observe events from the outside world and emit data contracts on a separate thread
- Modules receive contracts on separate GUI threads via Mediator and update their Views
- Shell's thread is only used to load/unload modules/services and monitor changes


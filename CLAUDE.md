# Intellimap

Intellimap is a .NET OSINT (Open Source Intelligence) framework for gathering intelligence about targets by running modules against them.
Each module queries an external data source and returns structured knowledge about the target - DNS records, geolocation data, network ownership information.

The framework is designed to be extensible without touching its own source.
Modules can be authored in a separate assembly and dropped into a folder at runtime;
Core discovers and loads them automatically, with no source changes or explicit registration required.

Modules are intentionally uniform: no constructor injection, no DI container, no per-module exceptions to the contract.
Configuration flows only through options objects.
If a need feels important enough to break that uniformity, it belongs in Core, not in a one-off module.

---

## Intellimap.Core

Core is the framework's foundation:
it defines what a target is, what knowledge is, what a module is, and how modules are discovered and executed,
and it ships the built-in modules that come with the framework out of the box.

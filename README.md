# Intellimap

> ⚠️ **Early development.** Intellimap is in an early, actively evolving stage. Its design, structure, and scope may change without notice. Not ready for production use.

Intellimap is a .NET OSINT (Open Source Intelligence) framework for gathering intelligence about targets by running modules against them. 
Each module queries an external data source and returns structured **knowledge** about a target - not raw, unstructured data.

## Knowledge, not just data

Modules don't just return whatever an external source gives back - they translate raw responses into structured knowledge: DNS records, geolocation, network ownership, and so on. Knowledge is the framework's core abstraction: it's what lets results from unrelated sources compose into a coherent picture of a target, and what future modules and consumers can rely on as a stable shape regardless of which external service produced it.

## How it works

Intelligence gathering in Intellimap revolves around four ideas:

- **Targets** - the thing being investigated, e.g. a hostname or an IP address.
- **Modules** - units of work that query an external data source for a given target and return structured knowledge.
- **Knowledge** - the structured, typed output a module produces. Instead of exposing whatever shape an external API happens to return, each module normalizes its findings into knowledge with a well-defined meaning (a DNS record, a location, a network owner). This is what lets results from completely different sources be combined, compared, and reasoned about consistently, and what future modules and consumers can build on as a stable contract regardless of which external service originally produced the data.
- **Discovery** - modules are found and loaded automatically, whether they ship with the framework or are added later, with no manual registration required.

The framework is designed to be extensible without touching its own source. Modules can be authored separately and dropped in at runtime, and they're picked up automatically.

Modules follow a single, standard structure on purpose - every module looks and behaves the same way, regardless of who wrote it or what it queries. There's no per-module configuration, injection, or special-casing.

## Built-in modules

TBD - Intellimap currently ships a couple of starter modules (DNS resolution, IP geolocation/ownership lookup) to validate the framework's design. The module catalog is expected to grow substantially; a proper list will land here once it stabilizes.

## Contributing & writing modules

Formal documentation for writing custom modules and for contributing to the project is coming soon.

In the meantime, modules are intentionally uniform in structure - the easiest way to understand the shape a module should take is to look at one of the existing built-in modules and follow the same pattern.

## Requirements

- .NET 10 SDK

## Building

```
dotnet build
```

## License

MIT - see [LICENSE](LICENSE).

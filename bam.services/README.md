# bam.services

Service registry management for dependency injection with persistent service descriptors.

## Overview

`bam.services` provides the data model and builder infrastructure for managing service registries in the BAM framework. A service registry maps interface types to their concrete implementations, and this library provides the classes needed to describe, persist, and reconstruct those mappings. The `ServiceRegistryBuilder` uses a fluent `For(type).Use(type)` pattern to construct `ServiceRegistry` instances.

The library includes a set of persistable descriptor classes (`ServiceRegistryDescriptor`, `ServiceDescriptor`, `ServiceRegistryLoaderDescriptor`, `ServiceTypeIdentifier`) that extend the BAM repository data model. These descriptors can be saved to and loaded from repositories (databases), enabling service registries to be stored, versioned, and shared across machines. The `ServiceTypeIdentifier` class uniquely identifies types using durable hashes based on namespace, type name, assembly, and build number.

A `ServiceRegistryService` class is included but is entirely commented out, indicating it is legacy or in-progress code. When active, it would serve as a centralized service for registering, retrieving, locking, and scanning for service registries. The data classes (`MachineRegistries`, `ServiceRegistryLock`) support distributed scenarios where service registries are associated with specific machines.

## Key Classes

| Class | Description |
|---|---|
| `ServiceRegistryBuilder` | Fluent builder that constructs a `ServiceRegistry` from paired `For(type)` / `Use(type)` calls. Validates that the count of "for" types matches "use" types. |
| `ServiceDefinition` | Holds a resolved interface-to-implementation pair with their `Type` and `Assembly` references. |
| `ServiceRegistryContainerRegistrationResult` | Result metadata from registering a service registry container, including success/failure status, the registry, and the loader attribute. |
| `ServiceRegistryService` | **(Commented out)** Central service for managing service registries: scanning assemblies, registering loaders, locking/unlocking registries, and resolving types. |
| `ServiceDescriptor` | Persistable descriptor for a single interface-implementation pair, identified by durable hashes of `ServiceTypeIdentifier`. |
| `ServiceRegistryDescriptor` | Persistable descriptor for a named collection of `ServiceDescriptor` entries, representing a full service registry. |
| `ServiceRegistryLoaderDescriptor` | Metadata describing a method (by type, assembly, and method name) used to load a `ServiceRegistry` at runtime. |
| `ServiceTypeIdentifier` | Uniquely identifies a .NET type via namespace, type name, assembly name, file hash, and build number. Produces durable and secondary hashes. |
| `MachineRegistries` | Associates a machine (by name and DNS) with a set of named service registries. |
| `ServiceRegistryLock` | Persistable flag to lock a service registry and prevent updates. |

## Dependencies

### Project References
- `bam.base` -- core framework types, extension methods, `Args`, `ServiceRegistry`, `ServiceRegistryContainerAttribute`, `ServiceRegistryLoaderAttribute`
- `bam.data.repositories` -- `AuditRepoData`, `KeyedAuditRepoData`, `RepoData`, `IRepository`, `IMetaProvider`
- `bam.data` -- data extensions and repository support

### Package References
- None

## Target Framework
- `net10.0`

## Usage Examples

### Building a service registry with ServiceRegistryBuilder
```csharp
using Bam.CoreServices.ServiceRegistration;
using Bam.Services;

ServiceRegistryBuilder builder = new ServiceRegistryBuilder();
ServiceRegistry registry = builder
    .For(typeof(IMyService))
    .Use(typeof(MyServiceImpl))
    .For(typeof(ILogger))
    .Use(typeof(ConsoleLogger))
    .Build();
```

### Creating a ServiceRegistryDescriptor from an Incubator
```csharp
using Bam.CoreServices.ServiceRegistration.Data;
using Bam.DependencyInjection;

DependencyProvider provider = new DependencyProvider();
provider.Set<IMyService>(new MyServiceImpl());

ServiceRegistryDescriptor descriptor = ServiceRegistryDescriptor.FromIncubator("MyRegistry", provider, "My app services");
descriptor.Save(repository);
```

### Identifying a type with ServiceTypeIdentifier
```csharp
using Bam.CoreServices.ServiceRegistration.Data;

ServiceTypeIdentifier identifier = ServiceTypeIdentifier.FromType(typeof(MyServiceImpl));
// identifier.Namespace == "MyApp.Services"
// identifier.TypeName == "MyServiceImpl"
// identifier.DurableHash -- stable int hash for persistence
```

### Describing a single service binding
```csharp
ServiceDescriptor descriptor = new ServiceDescriptor(typeof(IMyService), typeof(MyServiceImpl));
descriptor.Save(repository);
```

## Known Gaps / Not Yet Implemented

- **ServiceRegistryService is entirely commented out**: The main service class (~500 lines) is wrapped in block comments. Methods for scanning assemblies, registering loaders, locking/unlocking registries, and type resolution are non-functional. This class depended on `DaoRepository`, `AppConf`, `IFileService`, `IAssemblyService`, and `ServiceRegistryRepository` -- types from other modules not currently referenced.
- **MachineRegistries.RegistryNames**: Stored as a delimited string rather than a proper collection, suggesting incomplete normalization.
- **ServiceRegistryLock**: The lock/unlock mechanism is defined but cannot function while `ServiceRegistryService` is commented out.

# DataSeeding

## Introduction

From the [database seeding](https://en.wikipedia.org/wiki/Database_seeding) article in Wikipedia:
> Database seeding is populating a database with an initial set of data. It's common to load seed data such as initial user accounts or dummy data upon initial setup of an application.

The code needed to create an initial state of data for an application can get overwhelmingly large, even for smaller applications. This means that the source code file responsible for creating, generating, initializing, loading, transforming (...) your data, will eventually end up having thousands of lines of code and tons of different dependencies. When not properly organized, this could create maintainability issues for your application.

This library aims to help developers to divide the whole data seeding logic of an application into small chunks of logic with robust dependency management and proper service lifetime handling.

## Key Features

- **Multi-Dependency Support**: Seeds can depend on multiple other seeds using the `DependsOnTypes` property
- **Circular Dependency Detection**: Automatic detection of circular dependencies with clear error messages
- **Advanced Scoped Service Support**: Robust handling of scoped dependencies with proper lifetime management
- **Topological Sorting**: Intelligent dependency resolution using Kahn's algorithm for optimal execution order  
- **Service Lifetime Safety**: Prevents common DI issues like ObjectDisposedException and scope validation errors
- **Abstract Seed Support**: Full support for both `ISeed` interface and `Seed` abstract class implementations
- **Legacy Compatibility**: Backward compatible with existing Priority and DependsOn properties

## Getting Started

### Add the functionality to the dependency injection container

For Microsoft Dependency Injection, there is already an extension method built in:

```csharp
services.AddDataSeeding(typeof(Startup).Assembly);
```

This configures DataSeeding to scan the passed assembly to look for `ISeed` implementations. All found implementations will then prepared and properly ordered for the data seeding.

### The `ISeed` interface

The class that contains a chunk of the data seeding logic is called **seed**. To make the library pick up your seeds, they have to implement `ISeed` interface.

```csharp
public class UserSeed : ISeed
{
    public Type[] DependsOnTypes => new[] { typeof(TenantSeed), typeof(RoleSeed) };

    public async Task SeedAsync()
    {
        // Your seeding logic here
    }
}
```

In the `SeedAsync()` method you can add your data seeding logic. The seed will be instantiated by the dependency injection container, so you can use the constructor to inject services, including scoped services.

#### Dependency Management

**Multi-Dependencies (Recommended)**: Use the `DependsOnTypes` property to specify multiple dependencies:

```csharp
public Type[] DependsOnTypes => new[] { typeof(TenantSeed), typeof(RoleSeed) };
```

**Legacy Single Dependency**: The `DependsOn` property is still supported but deprecated:

```csharp
[Obsolete("Use DependsOnTypes property instead for multiple dependency support.")]
public Type? DependsOn => typeof(TenantSeed);
```

#### Scoped Service Injection

The library provides **enterprise-grade scoped service support** with proper lifecycle management:

```csharp
public class UserSeed : ISeed
{
    private readonly UserManager<User> userManager;
    private readonly ApplicationDbContext context;
    private readonly ILogger<UserSeed> logger;

    public UserSeed(
        UserManager<User> userManager, 
        ApplicationDbContext context,
        ILogger<UserSeed> logger)
    {
        this.userManager = userManager;
        this.context = context;
        this.logger = logger;
    }

    public async Task SeedAsync()
    {
        // Scoped services are properly injected and disposed
        var user = new User { UserName = "admin" };
        await this.userManager.CreateAsync(user, "Password123!");
    }
}
```

**Key Benefits:**

- **Fresh Scope Creation**: Each seeding operation uses a fresh DI scope, ensuring proper service isolation
- **Automatic Disposal**: Scoped services are automatically disposed after each seed execution
- **Dependency Analysis Safety**: Seeds are analyzed using temporary scopes that don't interfere with execution
- **No ObjectDisposedException**: Robust scope management prevents common disposal-related errors

### The `Seed` Abstract Class

For seeds that require manual execution or complex inheritance scenarios, you can inherit from the abstract `Seed` class:

```csharp
public class ManualSeed : Seed
{
    private readonly ILogger<ManualSeed> logger;

    public ManualSeed(ILogger<ManualSeed> logger)
    {
        this.logger = logger;
    }

    public override async Task SeedAsync()
    {
        this.logger.LogInformation("Manual seed executed");
        // Your seeding logic here
    }
}

// Manual execution via ISeeder
await seeder.SeedAsync<ManualSeed>();
```

### The `ISeeder` interface

The `ISeeder` interface can be resolved from the service provider. It contains the logic to find all seeds in a specified assembly and seed them in an appropriate order.

```csharp
public class DataInitializer
{
    private readonly ISeeder seeder;

    public DataInitializer(ISeeder seeder)
    {
        this.seeder = seeder;
    }

    public async Task RunAsync()
    {
        await this.seeder.SeedAsync();
    }
}
```

## Advanced Features

### Circular Dependency Detection

The library automatically detects circular dependencies and throws meaningful error messages:

```csharp
// This will throw an InvalidOperationException with details about the circular dependency
public class SeedA : ISeed 
{
    public Type[] DependsOnTypes => new[] { typeof(SeedB) };
    // ...
}

public class SeedB : ISeed 
{
    public Type[] DependsOnTypes => new[] { typeof(SeedA) }; // Circular dependency!
    // ...
}
```

### Complex Dependency Scenarios

```csharp
public class ComplexSeed : ISeed
{
    // This seed depends on multiple foundation seeds
    public Type[] DependsOnTypes => new[] 
    { 
        typeof(TenantSeed), 
        typeof(RoleSeed), 
        typeof(PermissionSeed) 
    };

    // Will only execute after ALL dependencies are completed
    public async Task SeedAsync() { /* ... */ }
}
```

### Scoped Service Lifecycle Management

The library implements robust scoped service handling to prevent common DI pitfalls:

```csharp
public class DatabaseSeed : ISeed
{
    private readonly ApplicationDbContext context;
    private readonly UserManager<User> userManager;

    public DatabaseSeed(ApplicationDbContext context, UserManager<User> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }

    public async Task SeedAsync()
    {
        // Both context and userManager are fresh instances within their own scope
        // They will be properly disposed when this seed completes
        var user = new User { UserName = "admin" };
        await this.userManager.CreateAsync(user);
        await this.context.SaveChangesAsync();
    }
}
```

**Lifecycle Benefits:**

- **Scope Isolation**: Each seed gets its own DI scope with fresh service instances
- **Memory Efficiency**: Services are disposed immediately after each seed completion
- **Transaction Safety**: Database contexts and other scoped services maintain proper transaction boundaries
- **Error Prevention**: Eliminates ObjectDisposedException and scope validation errors

### Execution Order Logging

The library provides detailed logging of the execution order:

```text
[Debug] Dependency resolution completed. Seeds will be executed in the following order:
[Debug] 1. TenantSeed (no dependencies)
[Debug] 2. RoleSeed (no dependencies) 
[Debug] 3. PermissionSeed (depends on: RoleSeed)
[Debug] 4. UserSeed (depends on: TenantSeed, RoleSeed)
[Debug] 5. ComplexSeed (depends on: TenantSeed, RoleSeed, PermissionSeed)
```

## Migration Guide

### From Priority-based to Dependency-based

**Old approach (deprecated):**

```csharp
public class UserSeed : ISeed
{
    public int Priority => 2; // Will be executed after priority 1
}
```

**New approach (recommended):**

```csharp
public class UserSeed : ISeed
{
    public Type[] DependsOnTypes => new[] { typeof(TenantSeed) };
}
```

### From Single to Multiple Dependencies

**Old approach (deprecated):**

```csharp
public Type? DependsOn => typeof(TenantSeed);
```

**New approach:**

```csharp
public Type[] DependsOnTypes => new[] { typeof(TenantSeed), typeof(RoleSeed) };
```

## Samples

Check out the sample console application and the comprehensive unit tests in the repository for more examples and usage patterns.

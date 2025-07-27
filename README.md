# DataSeeding

## What's this about?

From the [database seeding](https://en.wikipedia.org/wiki/Database_seeding) article in Wikipedia:

> Database seeding is populating a database with an initial set of data. It's common to load seed data such as initial user accounts or dummy data upon initial setup of an application.

We've all been there - your data seeding code starts simple but grows into a monster file with thousands of lines and complex dependencies. This library helps you split that mess into manageable chunks with proper dependency handling.

## Key Features

- **Multi-Dependency Support**: Seeds can depend on multiple other seeds using `DependsOnTypes`
- **Single Dependency**: Use `DependsOnType` for simpler syntax when you only have one dependency  
- **Circular Dependency Detection**: Catches circular dependencies with clear error messages
- **Scoped Service Support**: Proper handling of scoped dependencies with lifetime management
- **Topological Sorting**: Dependency resolution using Kahn's algorithm for execution order
- **Service Lifetime Safety**: Prevents common DI issues like ObjectDisposedException
- **Abstract Seed Support**: Support for both `ISeed` interface and `Seed` abstract class (being phased out)

## Quick Start

### Add to dependency injection

For Microsoft Dependency Injection:

```csharp
services.AddDataSeeding(typeof(Startup).Assembly);
```

This scans the assembly for `ISeed` implementations and prepares them for ordered execution.

### The `ISeed` interface

Create seeds by implementing the `ISeed` interface:

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

Seeds are instantiated by the dependency injection container, so you can inject services in the constructor, including scoped services.

## Dependency Management

### Multiple Dependencies

Use `DependsOnTypes` to specify multiple dependencies:

```csharp
public Type[] DependsOnTypes => new[] { typeof(TenantSeed), typeof(RoleSeed) };
```

### Single Dependency

For seeds with only one dependency, use `DependsOnType`:

```csharp
public class UserSeed : ISeed
{
    public Type? DependsOnType => typeof(TenantSeed);
    
    public Task SeedAsync()
    {
        // Your seeding logic here
        return Task.CompletedTask;
    }
}
```

### Dependency Priority

If both properties are specified, `DependsOnTypes` takes precedence over `DependsOnType`.

### Scoped Service Injection

The library handles scoped services properly:

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
        var user = new User { UserName = "admin" };
        await this.userManager.CreateAsync(user, "Password123!");
    }
}
```

**Benefits:**

- Each seed gets a fresh DI scope
- Scoped services are automatically disposed after execution
- Prevents ObjectDisposedException and scope validation errors

### The `Seed` Abstract Class (Being Deprecated)

> **⚠️ Deprecation Notice**: The abstract `Seed` class is being phased out and will be removed in a future release. New code should use the `ISeed` interface instead. We recommend migrating existing seeds to `ISeed` for better testability and cleaner dependency injection.

For legacy seeds that need manual execution, inherit from the abstract `Seed` class:

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

Resolve `ISeeder` from the service provider to execute seeds:

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

## Features

### Circular Dependency Detection

The library detects circular dependencies and throws clear error messages:

```csharp
// This will throw an InvalidOperationException
public class SeedA : ISeed 
{
    public Type[] DependsOnTypes => new[] { typeof(SeedB) };
    // ...
}

public class SeedB : ISeed 
{
    public Type[] DependsOnTypes => new[] { typeof(SeedA) }; // Circular!
    // ...
}
```

Detects all types of cycles:

- Simple cycles: A → B → A
- Complex cycles: A → B → C → A  
- Multi-level cycles: A → B → C → D → A

### Complex Dependencies

Seeds can have multiple dependencies:

```csharp
public class ComplexSeed : ISeed
{
    public Type[] DependsOnTypes => new[] 
    { 
        typeof(TenantSeed), 
        typeof(RoleSeed), 
        typeof(PermissionSeed) 
    };

    // Executes only after ALL dependencies complete
    public async Task SeedAsync() { /* ... */ }
}
```

## Example Usage

```csharp
// Base seed with no dependencies
public class UserRolesSeed : ISeed
{
    public Task SeedAsync()
    {
        // Seed user roles
        return Task.CompletedTask;
    }
}

// Single dependency
public class UsersSeed : ISeed
{
    public Type? DependsOnType => typeof(UserRolesSeed);
    
    public Task SeedAsync()
    {
        // Seed users (requires roles first)
        return Task.CompletedTask;
    }
}

// Multiple dependencies
public class UserPermissionsSeed : ISeed
{
    public Type[] DependsOnTypes => new[] 
    { 
        typeof(UsersSeed), 
        typeof(PermissionsSeed) 
    };
    
    public Task SeedAsync()
    {
        // Seed user permissions (requires both users and permissions)
        return Task.CompletedTask;
    }
}
```

## Demo

Check out the [demo project](./Neolution.Extensions.DataSeeding.Demo) for a complete CMS seeding example with 13 interconnected seeds.

# Dependency Management Improvements

This document describes the recent improvements to dependency management in the DataSeeding library.

## New Features

### 1. Pretty-Print Dependency Visualization

The seeder now includes visual logging of dependency relationships to help debug complex dependency trees:

```csharp
public async Task SeedAsync()
{
    await seeder.SeedAsync(seeds);
    // Output includes a tree-like visualization:
    // Dependency Graph:
    // ├── SimpleSeed
    // └── AnotherSeed
    //     └── DependsOn: SimpleSeed
}
```

### 2. Simplified Single Dependency Syntax

For seeds that depend on only one other seed, you can now use the `DependsOnType` property instead of `DependsOnTypes`:

**Old syntax (still supported):**
```csharp
public class MySeed : ISeed
{
    public Type[] DependsOnTypes => new[] { typeof(OtherSeed) };
    
    public Task SeedAsync()
    {
        // Your seeding logic
        return Task.CompletedTask;
    }
}
```

**New simplified syntax:**
```csharp
public class MySeed : ISeed
{
    public Type? DependsOnType => typeof(OtherSeed);
    
    public Task SeedAsync()
    {
        // Your seeding logic
        return Task.CompletedTask;
    }
}
```

### 3. Dependency Priority Order

The dependency resolver now supports multiple ways to declare dependencies with the following priority order:

1. **DependsOnTypes** (highest priority) - For multiple dependencies
2. **DependsOnType** (medium priority) - For single dependency (new!)
3. **DependsOn** (lowest priority) - Legacy Guid-based dependencies (obsolete)

## Circular Dependency Detection

The system automatically detects circular dependencies of any complexity:
- Simple cycles: A → B → A
- Complex cycles: A → B → C → A
- Multi-level cycles: A → B → C → D → A

When circular dependencies are detected, a detailed error message is logged showing the cycle path.

## Backward Compatibility

All existing code continues to work without changes. The new features are additive and don't break existing functionality.

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

// Seed that depends on UserRolesSeed (using new syntax)
public class UsersSeed : ISeed
{
    public Type? DependsOnType => typeof(UserRolesSeed);
    
    public Task SeedAsync()
    {
        // Seed users (requires roles to exist first)
        return Task.CompletedTask;
    }
}

// Seed with multiple dependencies (existing syntax)
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

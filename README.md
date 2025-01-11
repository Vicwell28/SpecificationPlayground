
```mermaid
classDiagram
    direction LR

    class ISpecification~T~ {
        <<interface>>
        +bool IsSatisfiedBy(T entity)
    }

    class CompositeSpecification~T~ {
        +ISpecification~T~ Left
        +ISpecification~T~ Right
        +bool IsSatisfiedBy(T entity)
    }

    class FluentSpecification~T~ {
        +FluentSpecification~T~ And(ISpecification~T~ other)
        +bool IsSatisfiedBy(T entity)
    }

    class LinqSpecification~T~ {
        +Expression~Func~T, bool~~ ToExpression()
        +bool IsSatisfiedBy(T entity)
    }

    class NameSpecification {
        +string Name
        +bool IsSatisfiedBy(string entity)
    }

    class NameStartsWithSpecification {
        +string StartsWith
        +Expression~Func~string, bool~~ ToExpression()
    }

    ISpecification~T~ <|.. CompositeSpecification~T~
    ISpecification~T~ <|.. FluentSpecification~T~
    ISpecification~T~ <|.. LinqSpecification~T~
    ISpecification~T~ <|.. NameSpecification
    LinqSpecification~string~ <|-- NameStartsWithSpecification
    CompositeSpecification~T~ o-- ISpecification~T~ : "Left"
    CompositeSpecification~T~ o-- ISpecification~T~ : "Right"
```

### Explicación del Diagrama
1. **ISpecification**: Es la interfaz común para todos los enfoques, definiendo el método `IsSatisfiedBy`.
2. **CompositeSpecification**: Implementa `ISpecification` y representa especificaciones compuestas con operadores lógicos como `And`, `Or`, etc.
3. **FluentSpecification**: Implementa `ISpecification` y añade métodos para construir especificaciones encadenadas.
4. **LinqSpecification**: Implementa `ISpecification` y agrega soporte para trabajar con expresiones LINQ.
5. **NameSpecification** y **NameStartsWithSpecification**: Son implementaciones específicas, donde `NameSpecification` es un ejemplo simple y `NameStartsWithSpecification` extiende `LinqSpecification` para aprovechar las expresiones.


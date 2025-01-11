# SpecificationPlayground

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

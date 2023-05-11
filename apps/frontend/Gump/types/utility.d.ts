// Generic Type Names
// - T: Type
// - K: Key
// - P: Property (key + value)
// - U: Another Type
// - R: Return Type
// - A, B, C, etc: Types that are related to each other in some way

// Given a type T, make K properties optional
type Optional<T, K extends keyof T> = Pick<Partial<T>, K> & Omit<T, K>

// Given a type T, make K properties required
type Required<T, K extends keyof T> = Pick<T, K> & Partial<Omit<T, K>>

// Remove the id property from T
type NoId<T> = Omit<T, 'id'>

// Make all properties of T readonly
type Readonly<T> = { readonly [P in keyof T]: T[P] }

// A type that makes all properties of T optional except for K
type PartialExcept<T, K extends keyof T> = Partial<T> & Pick<T, K>

// A type that makes all properties of T nullable
type Nullable<T> = { [P in keyof T]: T[P] | null }

// A type that picks only the properties of T that are functions
type FunctionProperties<T> = Pick<T, { [K in keyof T]: T[K] extends Function ? K : never }[keyof T]>

// A type that excludes the properties of T that are assignable to U
type ExcludeProperties<T, U> = Pick<T, { [K in keyof T]: T[K] extends U ? never : K }[keyof T]>

// A type that picks only the properties of T that are assignable to U
type FilterProperties<T, U> = Pick<T, { [K in keyof T]: T[K] extends U ? K : never }[keyof T]>

// A type that maps the properties of T to a different type U
type MapProperties<T, U> = { [P in keyof T]: U }

// A type that merges two types A and B
type Merge<A, B> = { [P in keyof A | keyof B]: P extends keyof A ? A[P] : P extends keyof B ? B[P] : never }

// A type that returns the union of the values of T
type ValueOf<T> = T[keyof T]

// A type that returns the intersection of two types A and B
type Intersect<A, B> = A & B extends infer R ? { [P in keyof R]: R[P] } : never

// A type that returns the difference of two types A and B
type Diff<A, B> = Omit<A, keyof B> & Omit<B, keyof A>

// A type that returns the keys of T that are not in U
type ExcludeKeys<T, U> = Exclude<keyof T, keyof U>

// A type that returns the keys of T that are in U
type IncludeKeys<T, U> = Extract<keyof T, keyof U>

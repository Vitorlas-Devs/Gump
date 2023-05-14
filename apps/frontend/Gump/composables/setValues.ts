/**
 * Sets the value of a property of an object if it exists.
 * @param obj The object to modify. It can be a partial object that may not have all properties of T.
 * @param key The property name to set.
 * @param value The new value to assign to the property.
 * @returns The new value of the property, or undefined if the property does not exist on the object.
 * @template T The type of the object.
 * @template K The type of the property name, which must be a key of T.
 * @example
 * const user = { name: "Alice", age: 25 };
 * setValues(user, "name", "Bob"); // user is now { name: "Bob", age: 25 }
 * setValues(user, "age", 30); // user is now { name: "Bob", age: 30 }
 * const partialUser: Partial<User> = { name: "Charlie" };
 * setValues(partialUser, "name", "David"); // partialUser is now { name: "David" }
 * setValues(partialUser, "age", 40); // partialUser is still { name: "David" }, returns undefined
 * @see https://stackoverflow.com/questions/73032080/typescript-type-string-number-is-not-assignable-to-type-never
 */
export function setValues<T, K extends keyof T>(obj: Partial<T>, key: K, value: T[K]) {
  // Check if the key exists on the object
  if (key in obj) {
    // Set the value and return it
    return obj[key] = value
  } else {
    // Return undefined
    return undefined
  }
}

/**
 * Checks if an object has a name property of type string.
 * @param obj - The object to check.
 * @returns True if the object has a name property of type string, false otherwise.
 * @summary Insanely powerful use of typescript "is" and "in" keywords. 🔥🔥
 */
export function hasName(obj: unknown): obj is { name: string } {
  return typeof obj === 'object' && obj !== null && 'name' in obj && typeof obj.name === 'string'
}

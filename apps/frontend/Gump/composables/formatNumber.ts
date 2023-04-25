/**
 * Format a number to a short string with a suffix (k, M, B, T), and keep it to 3 digits
 * @param value The number to format
 * @returns The formatted number
 * @example
 * formatNumber(123) // 123
 * formatNumber(1234) // 1.23k
 * formatNumber(12345) // 12.3k
 * formatNumber(123456) // 123k
 */
export function formatNumber(value: number): string {
  const ending = [' ', 'k', 'M', 'B', 'T']
  const suffix = Math.floor((`${value}`).length / 4)
  const shortValue = parseFloat((suffix !== 0 ? (value / 1000 ** suffix) : value).toPrecision(3))
  return shortValue + ending[suffix]
}

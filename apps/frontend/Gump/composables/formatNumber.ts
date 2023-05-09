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
  if (value === 0)
    return '0'

  const negative = value < 0
  if (negative)
    value = -value

  const suffixes = ['', 'k', 'M', 'B', 'T']
  const suffixNum = Math.floor(Math.log10(value) / 3)
  const shortValue = parseFloat((suffixNum !== 0 ? (value / 1000 ** suffixNum) : value).toPrecision(3))
  return (negative ? '-' : '') + shortValue + suffixes[suffixNum]
}

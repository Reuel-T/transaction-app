export function numberToCurrency(value: number): string {
  return value.toLocaleString('en-ZA', { style: 'currency', currency: 'ZAR' })
}

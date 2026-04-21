/**
 * Represents a single trip to the Schengen Area.
 * Ported from TripDate.vb
 */
export class TripDate {
  startDate: Date;
  endDate: Date;

  constructor(startDate: Date, endDate: Date) {
    this.startDate = startDate;
    this.endDate = endDate;
  }

  /** Returns the number of days of the trip (inclusive of start and end). */
  numberOfDays(): number {
    const diffMs = this.endDate.getTime() - this.startDate.getTime();
    return Math.floor(diffMs / (1000 * 60 * 60 * 24)) + 1;
  }

  /** Returns true if the given date falls within this trip. */
  wasInAreaOnDate(date: Date): boolean {
    return date >= this.startDate && date <= this.endDate;
  }

  toString(): string {
    return `${this.startDate.toLocaleDateString()} to ${this.endDate.toLocaleDateString()} (${this.numberOfDays()} days)`;
  }
}

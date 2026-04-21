import { TripDates } from './tripDates';

/**
 * Core Schengen day calculator.
 * Counts days spent in the Schengen Area within a rolling window ending on
 * the review date.  Ported from SchengenCalculator.vb
 */
export class SchengenCalculator {
  private tripDates: TripDates;
  private maximumDaysInArea: number;
  private reviewDaysInArea: number;

  constructor(
    tripDates: TripDates,
    maximumDaysInArea: number,
    reviewDaysInArea: number
  ) {
    this.tripDates = tripDates;
    this.maximumDaysInArea = maximumDaysInArea;
    this.reviewDaysInArea = reviewDaysInArea;
  }

  get maxDays(): number {
    return this.maximumDaysInArea;
  }

  /**
   * Returns the number of days spent in the Schengen Area within the
   * rolling window [reviewDay - reviewDaysInArea, reviewDay].
   * Faithful port of the VB loop: counts a day once per matching trip entry
   * (overlapping trips would be counted multiple times, matching original).
   */
  numberOfDaysInAreaOnDay(reviewDay: Date): number {
    let noOfDays = 0;

    // Start of the 180-day window
    const windowStart = new Date(reviewDay);
    windowStart.setDate(windowStart.getDate() - this.reviewDaysInArea);

    const calcDate = new Date(windowStart);
    while (calcDate <= reviewDay) {
      for (const trip of this.tripDates.entries) {
        if (trip.wasInAreaOnDate(new Date(calcDate))) {
          noOfDays++;
        }
      }
      calcDate.setDate(calcDate.getDate() + 1);
    }

    return noOfDays;
  }

  /** Returns a human-readable summary string for a given date. */
  summaryForDay(reviewDay: Date): string {
    const used = this.numberOfDaysInAreaOnDay(reviewDay);
    const remaining = this.maximumDaysInArea - used;
    if (remaining >= 0) {
      return `${used} used, ${remaining} remaining`;
    }
    return `${used} used, ${Math.abs(remaining)} over!`;
  }
}
